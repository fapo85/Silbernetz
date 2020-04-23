using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Silbernetz.Controllers.SignalHub;
using Silbernetz.Models;
using Silbernetz.Models.Api;
using Silbernetz.Models.Conf;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Silbernetz.Actions
{
    public class InoplaClient
    {
        private readonly string ApiId;
        private readonly string ApiKey;
        private readonly IHubContext<SignalRHub> hubContext;
        public InoplaClient(IOptions<InoplaConf> Secrets, IHubContext<SignalRHub> hubContext)
        {
            InoplaConf secrets = Secrets.Value;
            this.ApiId = secrets.ApiId;
            this.ApiKey = secrets.ApiKey;
            this.hubContext = hubContext;
        }

        //private string ApiUrl(String enpoint) => $"https://api.inopla.de/{version}/{format}/{ApiId}/{ApiKey}/{enpoint}";
        private string ApiUrl(String enpoint) => $"https://api.inopla.de/v1000/json/{ApiId}/{ApiKey}/{enpoint}";
        public async Task<LiveData> GetLiveDataAsync() => await GetAsync<LiveData>(ApiUrl("Live/SIP"));
        public async Task<LiveCalls> GetLiveCallsAsync() => await GetAsync<LiveCalls>(ApiUrl("Live/Calls"));
        public async Task<BlackListItems> GetBlackAsync() => await GetAsync<BlackListItems>(ApiUrl("Lists/Callerlists/1407/Items"));

        internal async void AddToBlackList(BLAction action)
        {
            using (HttpClient client = new HttpClient())
            {
                string json = JsonSerializer.Serialize<BLAction>(action);
                HttpContent tosend = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(ApiUrl("Lists/Callerlists/1407/Items") , tosend))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Error in Add To Blacklist Request: {response.StatusCode}");
                    }
                }
            }
        }

        public async Task<Evn> GetEVNDataAsync(long offset = 0) => await GetAsync<Evn>(ApiUrl($"/Live/EVN?offset={offset}"));
        private async Task<T> GetAsync<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Error in {nameof(T)} Request: {response.StatusCode}");
                    }
                    return await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());
                }
            }
        }

    }
}
