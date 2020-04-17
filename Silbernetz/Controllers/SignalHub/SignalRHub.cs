using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Silbernetz.Actions;
using Silbernetz.Database;
using Silbernetz.Models;
using Silbernetz.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Controllers.SignalHub
{
    public partial class SignalRHub : Hub
    {
        private readonly InoplaClient inoplaClient;
        private readonly SignInManager<ApplicationUser> siginmanager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtManager jwtManager;
        private readonly AnrufSafe database = new AnrufSafe();
        public SignalRHub(InoplaClient inoplaClient, SignInManager<ApplicationUser> siginmanager, UserManager<ApplicationUser> userManager, JwtManager jwtManager)
        {
            this.inoplaClient = inoplaClient;
            this.siginmanager = siginmanager;
            this.userManager = userManager;
            this.jwtManager = jwtManager;
        }
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("stats", Stats.FromLiveData(inoplaClient.GetLiveDataAsync().Result));
            Clients.Caller.SendAsync("manystats", database.AnrufStatisitk(DateTime.Today.AddDays(-7)));
            return base.OnConnectedAsync();
        }

        public IEnumerable<Stats> GetStatistikFuerTage(int tage)
        {
            return database.AnrufStatisitk(DateTime.Now.AddDays(-1 * tage));
        }
    }
}
