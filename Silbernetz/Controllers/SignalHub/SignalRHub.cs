using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Silbernetz.Actions;
using Silbernetz.Database;
using Silbernetz.Models;
using Silbernetz.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Controllers.SignalHub
{
    public partial class SignalRHub : Hub
    {
        private readonly AnrufSafe database;
        private readonly BlackListSave blsave;
        public SignalRHub(AnrufSafe database, BlackListSave blsave)
        {
            this.database = database;
            this.blsave = blsave;
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
        [Authorize]
        public IEnumerable<BLIDatum> GetBlackList()
        {
            return blsave.GetList();
        }
        [Authorize]
        public void AddToBlackList(string nr, string description)
        {
            if (string.IsNullOrEmpty(nr))
            {
                throw new Exception("Telefonnummer darf nicht null sein");
            }
            if (string.IsNullOrEmpty(description))
            {
                throw new Exception("description darf nicht null sein");
            }
            Console.WriteLine("Setze Blacklist " + nr.Remove(nr.Length - 3, 3) + "XXX");
            blsave.Add(nr, description);
            database.UpdateBlacklist();
        }
    }
}
