using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Silbernetz.Actions;
using Silbernetz.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Silbernetz.Controllers
{
    public class ApiController : Controller
    {
        private readonly InoplaClient inoplaClient;
        public ApiController(InoplaClient inoplaClient)
        {
            this.inoplaClient = inoplaClient;
        }


        [HttpGet("/Api/Stats")]
        public async Task<Stats> GetStats()
        {
            return Stats.FromLiveData(await inoplaClient.GetLiveDataAsync());
        }

        [HttpPost("/Api/PushCall")]
        public async Task<IActionResult> PushCall([FromBody]CallConnectEvent callevent)
        {
            Console.WriteLine(JsonSerializer.Serialize<CallConnectEvent>(callevent));
            if(callevent.Event == EventType.connect)
            {
                LiveData livedata = await inoplaClient.GetLiveDataAsync(true);
                Anruf anruf = new Anruf()
                {
                    Uuid = callevent.Uuid,
                    TimeStamp = callevent.Timestamp,
                    Benutzer = livedata.Benutzer,
                    Angemeldet = livedata.Angemeldet,
                    AmTelefon = livedata.AmTelefon
                };
                Console.WriteLine(JsonSerializer.Serialize<CallConnectEvent>(callevent));
                //Jetzt nur noch Speichern :-D
            }
            return Ok();
        }

    }
}
