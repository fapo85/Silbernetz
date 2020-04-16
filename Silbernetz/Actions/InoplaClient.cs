using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Silbernetz.Controllers.SignalHub;
using Silbernetz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Silbernetz.Actions
{
    public class InoplaClient
    {
        private readonly string ApiId;
        private readonly string ApiKey;
        private readonly IHubContext<SignalRHub> hubContext;
        public LiveData LastOne { get; private set; }
        private static readonly TimeSpan MaxAge = TimeSpan.FromMinutes(1);
        public InoplaClient(IOptions<SecretConf> Secrets, IHubContext<SignalRHub> hubContext)
        {
            SecretConf secrets = Secrets.Value;
            this.ApiId = secrets.InoplaApiId;
            this.ApiKey = secrets.InoplaApiKey;
            this.hubContext = hubContext;
        }
        private string ApiUrl(String enpoint) => $"https://api.inopla.de/v1000/json/{ApiId}/{ApiKey}/{enpoint}";
        public void StartRenewAsync() => new Task(()=> GetLiveData(true)).Start();
        public LiveData GetLiveData(bool forceRenew = false) => GetLiveDataAsync(forceRenew).Result;
        public async Task<LiveData> GetLiveDataAsync(bool forceRenew = false)
        {
            if(forceRenew == false && LastOne != null && LastOne.TimeStamp.Add(MaxAge) < DateTime.Now)
            {
                return LastOne;
            }
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(ApiUrl("Live/SIP")))
                {
                    if (response.IsSuccessStatusCode) {
                        LiveData NewData = await JsonSerializer.DeserializeAsync<LiveData>(await response.Content.ReadAsStreamAsync());
                        CheckForUpdate(LastOne, NewData);
                        LastOne = NewData;
                    }
                    else{
                        Console.WriteLine("Error in Abfrage: " + response.StatusCode);
                    }
                    return LastOne;
                }
            }
        }

        private void CheckForUpdate(LiveData oldData, LiveData newData)
        {
            new Task(() =>
            {
                Stats stats = Stats.FromLiveData(newData);
                if (oldData == null || oldData.AmTelefon != stats.AmTelefon || oldData.Angemeldet != stats.Angemeldet || oldData.Benutzer != stats.Benutzer)
                {
                    hubContext.Clients.All.SendAsync("stats", stats);
                }
            }).Start();
        }
    }
}
