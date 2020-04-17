using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Silbernetz.Actions;
using Silbernetz.Database;
using Silbernetz.Models;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Silbernetz.Controllers
{
    public class ApiController : Controller
    {
        private readonly InoplaClient inoplaClient;
        private readonly AnrufSafe database = new AnrufSafe();
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
            if (ModelState.IsValid)
            {
                Anruf anruf = database.AddEventToAnruf(callevent.Uuid, callevent.Caller, new CallEvents(callevent));
                if (callevent.Event.Contains("connect"))
                {
                    LiveData livedata = await inoplaClient.GetLiveDataAsync(true);
                    database.AddStats(anruf, Stats.FromLiveData(livedata));
                }
                else if(callevent.Event.Contains("hangup"))
                {
                    inoplaClient.StartRenewAsync();
                }
                Console.WriteLine(JsonSerializer.Serialize<Anruf>(anruf));
            }
            else
            {
                Console.WriteLine("ModelState NotValid:");
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                Console.WriteLine(JsonSerializer.Serialize(errors));
            }
            return Ok();
        }

    }
}
