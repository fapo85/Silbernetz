﻿//using Microsoft.AspNetCore.Mvc;
//using Silbernetz.Actions;
//using Silbernetz.Database;
//using Silbernetz.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Silbernetz.Controllers
//{
//    public class ApiController : Controller
//    {
//        //private readonly InoplaClient inoplaClient;
//        //private readonly AnrufSafe database = new AnrufSafe();
//        //public ApiController(InoplaClient inoplaClient)
//        //{
//        //    this.inoplaClient = inoplaClient;
            
//        //}


//        //[HttpGet("/Api/Stats")]
//        //public async Task<Stats> GetStats()
//        //{
//        //    return Stats.FromLiveData(await inoplaClient.GetLiveDataAsync());
//        //}
//        //[HttpGet("/Api/Anrufe")]
//        //public IEnumerable<Anrufer> AnrufFromToday()
//        //{
//        //    return database.AnrufFromToday();
//        //}
//        //[HttpGet("/Api/Auslastung/")]
//        //public IEnumerable<Stats> Auslastung()
//        //{
//        //    return Auslastung(7);
//        //}
//        //[HttpGet("/Api/Auslastung/{tage}")]
//        //public IEnumerable<Stats> Auslastung(int tage)
//        //{
//        //    return database.AnrufStatisitk(DateTime.Today.AddDays(-1 * tage));
//        //}

//        //[HttpPost("/Api/PushCall")]
//        //public async Task<IActionResult> PushCall([FromBody]CallConnectEvent callevent)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        Anruf anruf = database.AnrufAddEvent(callevent.Uuid, callevent.Caller, new CallEvents(callevent));
//        //        if (callevent.Event.Contains("connect"))
//        //        {
//        //            LiveData livedata = await inoplaClient.GetLiveDataAsync(true);
//        //            database.AddStatsToAnruf(anruf, Stats.FromLiveData(livedata));
//        //        }
//        //        else if(callevent.Event.Contains("hangup"))
//        //        {
//        //            inoplaClient.StartRenewAsync();
//        //        }
//        //        Console.WriteLine(JsonSerializer.Serialize<Anruf>(anruf));
//        //    }
//        //    else
//        //    {
//        //        Console.WriteLine("ModelState NotValid:");
//        //        var errors = ModelState.Values.SelectMany(v => v.Errors);
//        //        Console.WriteLine(JsonSerializer.Serialize(errors));
//        //    }
//        //    return Ok();
//        //}

//    }
//}
