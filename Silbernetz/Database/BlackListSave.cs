using Microsoft.AspNetCore.SignalR;
using Silbernetz.Actions;
using Silbernetz.Controllers.SignalHub;
using Silbernetz.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Database
{
    public class BlackListSave
    {
        private readonly IHubContext<SignalRHub> hubContext;
        private List<BLIDatum> Save = new List<BLIDatum>();
        private readonly SaveLock saveLock = new SaveLock();
        private readonly InoplaClient inoplaClient;
        public BlackListSave(IHubContext<SignalRHub> hubContext, InoplaClient inoplaClient)
        {
            this.hubContext = hubContext;
            this.inoplaClient = inoplaClient;
            //new BLAction("344254363546", "test");
        }
        internal void RenewBlackList(BlackListItems result)
        {
            if (result?.Response?.Data != null)
            {
                using (saveLock.Write())
                {
                    this.Save = result.Response.Data;
                }
            }
        }
        public IEnumerable<BLIDatum> GetList()
        {
            using (saveLock.Read())
            {
                return Save.ToList();
            }
        }

        internal void Add(string nr, string description)
        {

            inoplaClient.AddToBlackList(new BLAction(nr, description));
        }
    }
}
