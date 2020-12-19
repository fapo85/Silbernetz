using Silbernetz.Actions;
using Silbernetz.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Database {
    public class GroupSave {
        private readonly InoplaClient inoplaClient;
        private Dictionary<int, SipGroups> Save = null;
        private readonly SaveLock saveLock = new SaveLock();
        public GroupSave(InoplaClient inoplaClient) {
            this.inoplaClient = inoplaClient;
            Update();
        }
        internal bool Renew(Dictionary<int, SipGroups> result) {
            if (result != null) {
                using (saveLock.Write()) {
                    this.Save = result;
                }
                return false;
            }
            return true;
        }
        public Dictionary<int, SipGroups> GetList() {
            using (saveLock.Read()) {
                return Save;
            }
        }
        public bool Update() {
            try {
                var res = inoplaClient.GetGroupMembersAsync().Result;
                return Renew(res);
            } catch (Exception e) {
                return true;
                Console.WriteLine("Updater: Fehler Beim Holen der Group Daten: " + e.Message);
            }
        }

        internal LiveData Filter(LiveData liveData) {
            liveData.Response.Data = liveData.Response.Data.Where(r => GroupContains(r.Id)).ToArray();
            return liveData;
        }

        private bool GroupContains(ulong id) {
            foreach(SipGroups sg in GetList().Values) {
                foreach(var user in sg.Response.Data) {
                    if(user.User.Id == id) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
