using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Silbernetz.Actions;
using Silbernetz.Database;
using Silbernetz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Controllers.SignalHub
{
    public partial class SignalRHub : Hub
    {
        private readonly AnrufSafe database;
        public SignalRHub(AnrufSafe database)
        {
            this.database = database;
        }
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("stats", database.GetStatsNow()).Wait();
            Clients.Caller.SendAsync("manystats", database.AnrufStatisitk(DateTime.Today.AddDays(-7))).Wait();
            return base.OnConnectedAsync();
        }
        [Authorize]
        public IEnumerable<Anrufer> AnrufFromToday()
        {
            return database.AnrufFromToday();
        }
        public IEnumerable<WaitTimeProp> GetStatistikFuerTage(int tage)
        {
            return database.AnrufStatisitk(DateTime.Now.AddDays(-1 * tage));
        }
    }
}
