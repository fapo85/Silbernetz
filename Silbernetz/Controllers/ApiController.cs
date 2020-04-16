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

        [HttpGet("/Api/PushCall")]
        public async Task<IActionResult> PushCall(CallConnectEvent callevent)
        {
            Console.WriteLine(JsonSerializer.Serialize<CallConnectEvent>(callevent));
            //if(callevent.Event == EventType.connect)
            //{

            //}
            return Ok();
        }

    }
}
