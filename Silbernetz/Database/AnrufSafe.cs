using Microsoft.AspNetCore.SignalR;
using Silbernetz.Actions;
using Silbernetz.Controllers.SignalHub;
using Silbernetz.Models;
using Silbernetz.Models.Api;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Silbernetz.Database {
    public class AnrufSafe {
        private SaveLock saveLock;
        private readonly string[] ErlaubteServices = { "0800-4708090", };
        private readonly SortedSet<Anruf> Save = new SortedSet<Anruf>(new AnrufComparer());
        private readonly SortedSet<WaitTimeProp> WaitTimeSave = new SortedSet<WaitTimeProp>(new WaitTimeComparer());
        private readonly InoplaClient inoplaClient;
        private readonly IHubContext<SignalRHub> hubContext;
        private readonly GroupSave groupSave;
        public LiveData AktuelleStats { get; private set; }
        private const int TAGEHOLEN = 7;
        private static readonly TimeSpan WaitTimeAbstand = TimeSpan.FromHours(2);
        private Updater updater;
        private readonly BlackListSave blsave;
        public bool InitCompleted { get; private set; } = false;
        public AnrufSafe(InoplaClient inoplaClient, IHubContext<SignalRHub> hubContext, BlackListSave blsave, GroupSave groupSave) {
            this.inoplaClient = inoplaClient;
            this.hubContext = hubContext;
            this.blsave = blsave;
            this.groupSave = groupSave;
        }
        public void Init() {
            Task t = new Task(async () => {
                AktuelleStats = groupSave.Filter(await inoplaClient.GetLiveDataAsync());
                long skip = 0;
                bool weiter = true;
                DateTime last = DateTime.MinValue;
                while (weiter) {
                    Evn evn = await inoplaClient.GetEVNDataAsync(skip);
                    foreach (var revn in evn.Response.Data) {
                        if (InErlaubtenServies(revn.Service)) {
                            Anruf anruf = new Anruf();
                            anruf.DataFromEvn(revn);
                            Save.Add(anruf);
                            last = anruf.TimeStamp;
                        }
                    }
                    skip += evn.Response.Count;
                    //Hole 7 Tage;
                    weiter = last > DateTime.Today.AddDays(-1 * TAGEHOLEN);
                }
                updater = new Updater(inoplaClient, this, blsave, groupSave);
                saveLock = new SaveLock();
            });
            t.Start();
        }

        private bool InErlaubtenServies(string service) {
            foreach (string str in ErlaubteServices) {
                if (service.StartsWith(str)) {
                    return true;
                }
            }
            return false;
        }

        internal void UpdateBlacklist() {
            updater.UpdateBlacklist();
        }

        internal void UpdateStatistik() {
            if (saveLock == null) {
                return;
            }
            using (saveLock.Write()) {
                DateTime last = WaitTimeSave.LastOrDefault() == null ? NextHour(Save.First().TimeStamp) : NextHour(WaitTimeSave.Last().TimeStamp);
                ulong Wartezeit = 0;
                while (last.Add(WaitTimeAbstand) < DateTime.Now) {
                    //var dt = DateTime.Today.AddDays(-4).AddHours(7);
                    ulong WarteSekunden = 0;
                    ulong anzahl = 0;
                    foreach (var itms in Save.Where(it =>
                    it.TimeStamp <= last.Add(WaitTimeAbstand) && it.TimeStamp.AddSeconds(it.OutBound) >= last.Subtract(WaitTimeAbstand)
                    )) {
                        if (itms.OutBound > 0) {
                            WarteSekunden += itms.Wait;
                            anzahl++;
                        }
                    }
                    if (anzahl > 0) {
                        Wartezeit = (ulong)(WarteSekunden / anzahl);
                    }
                    WaitTimeSave.Add(new WaitTimeProp() {
                        TimeStamp = last,
                        WaitTime = Wartezeit
                    });
                    last = last.Add(WaitTimeAbstand);
                }
                if (InitCompleted == false) {
                    InitCompleted = true;
                    Console.WriteLine("Alle Daten wurden Geladen. Tage im Speicher: " + TAGEHOLEN);
                }
            }
        }


        public WaitTimeProp WaitTimeNow() {
            if (saveLock == null) {
                return new WaitTimeProp() {
                    TimeStamp = DateTime.Now,
                    WaitTime = 0
                };
            }
            using (saveLock.Read()) {
                ulong WarteSekunden = 0;
                ulong anzahl = 0;
                foreach (var itms in Save.Where(it => it.TimeStamp.AddSeconds(it.OutBound) >= DateTime.Now.Subtract(WaitTimeAbstand))) {
                    if (itms.OutBound > 0) {
                        WarteSekunden += itms.Wait;
                        anzahl++;
                    }
                }
                return new WaitTimeProp() {
                    TimeStamp = DateTime.Now,
                    WaitTime = anzahl == 0 ? 0 : (ulong)(WarteSekunden / anzahl)
                };
            }
        }
        private DateTime NextHour(DateTime dt) {
            var timeOfDay = dt.TimeOfDay;
            var nextFullHour = TimeSpan.FromHours(Math.Ceiling(timeOfDay.TotalHours));
            return dt.AddSeconds((nextFullHour - timeOfDay).TotalSeconds);
        }
        public IEnumerable<Anrufer> AnrufFromToday() {
            Dictionary<string, Anrufer> ret = new Dictionary<string, Anrufer>();
            if (saveLock != null) {
                using (saveLock.Read()) {
                    foreach (Anruf itm in Save.Where(a => a.TimeStamp > DateTime.Today.AddDays(-2))) {
                        Anrufer anrufer;
                        if (!ret.TryGetValue(itm.TelNummer, out anrufer)) {
                            anrufer = new Anrufer();
                            anrufer.TelNummer = itm.TelNummer;
                            ret.Add(itm.TelNummer, anrufer);
                        }
                        anrufer.Anrufe.Add(itm);
                    }
                }
            }
            return ret.Values.Where(ae => ae.GesamtDauer > 0);
        }

        internal Task NewDataToAdd(LiveData liveData, Evn evn, LiveCalls liveCalls, DateTime letzteFehlerfreieAktualisierung) {
            if (saveLock == null) {
                return Task.CompletedTask;
            }
            //Aktualisiere Live Data
            LiveData oldValues = AktuelleStats;
            bool forcerenew = false;
            if (liveData != null) {
                AktuelleStats = liveData;
            }
            //EVN Check
            if (evn?.Response?.Data == null) {
                //Fehler im EVN Request, Noch Statistik versenden vor der Exception
                if (liveData != null) {
                    CheckForUpdate(oldValues, liveData, false, letzteFehlerfreieAktualisierung).Wait();
                }
                throw new Exception("EVN sind null");
            }
            using (var lockob = saveLock.ReadThanWrite()) {
                var SaveList = Save.Where(s => s.lauftnoch == false).ToList();
                //List<Datum> EvnToAdd = new List<Datum>(evn.Response.Data.Where(evn => SaveList.Where(sl => sl.TelNummer.Replace("X", "").StartsWith(evn.Caller.Replace("X", ""))).FirstOrDefault() == null));
                //Neu Hinzugefügte aus env eintragen
                List<Datum> EvnToAdd = new List<Datum>(evn.Response.Data.Where(evn => InErlaubtenServies(evn.Service) && SaveList.FirstOrDefault(sl => sl.id == evn.Id || (sl.lauftnoch == true && sl.TelNummer.StartsWith(evn.Caller.Replace("X", "")))) == null));
                //Ab Hier in Save Schreiben
                lockob.UpgradeToWriterLock();
                foreach (Datum itm in EvnToAdd) {
                    Anruf anruf = Save.FirstOrDefault(sl => sl.id == itm.Id || (sl.lauftnoch == true && sl.TelNummer.StartsWith(itm.Caller.Replace("X", ""))));
                    if (anruf == null) {
                        anruf = new Anruf();
                        Save.Add(anruf);
                    }
                    anruf.DataFromEvn(itm);
                }
                if (liveCalls?.Response?.Data != null) {
                    foreach (CallDatum itm in liveCalls.Response.Data.Where(c => InErlaubtenServies(c.Service))) {

                        Anruf anruf = Save.FirstOrDefault(so => so.id == itm.Id);
                        if (anruf == null) {
                            anruf = new Anruf();
                            Save.Add(anruf);
                        }
                        anruf.DataFromLC(itm);
                    }
                }
            }

            //LiveData Check
            if (liveData == null) {
                throw new Exception("LiveData sind null keine Statistik versenden");
            }
            //Update
            return CheckForUpdate(oldValues, liveData, forcerenew, letzteFehlerfreieAktualisierung);
        }

        public IEnumerable<WaitTimeProp> AnrufStatisitk(DateTime after) {
            List<WaitTimeProp> ret = new List<WaitTimeProp>();
            if (saveLock != null) {
                Task<WaitTimeProp> TaskToNow = new Task<WaitTimeProp>(() => WaitTimeNow());
                TaskToNow.Start();
                using (saveLock.Read()) {
                    ret.AddRange(WaitTimeSave);
                }
                ret.Add(TaskToNow.Result);
            }
            return ret;

        }
        public Stats GetStatsNow() {
            if (AktuelleStats == null)
            {
                return new Stats();
            }
            return Stats.FromLiveData(AktuelleStats, WaitTimeNow().WaitTime, AktuelleStats.TimeStamp);
        }
        public void AnrufCleanUp() {
            AnrufMakeAnonymous();
            AnrufDelOld();
            WaitTimeSaveDelold();
        }
        private void AnrufMakeAnonymous() {
            if (saveLock != null) {
                using (saveLock.Read()) {
                    foreach (Anruf itm in Save.Where(a =>
                        a.TimeStamp < DateTime.Today && a.TelNummer != null && a.TelNummer.Length > 3 &&
                        !a.TelNummer.EndsWith("XXX"))) {
                        itm.TelNummer = itm.TelNummer.Remove(itm.TelNummer.Length - 3, 3) + "XXX";
                    }
                }
            }
        }
        private void AnrufDelOld() {
            if (saveLock != null) {
                using (saveLock.Write()) {
                    Save.RemoveWhere(a => a.TimeStamp < DateTime.Today.AddDays(-1 * TAGEHOLEN));
                }
            }
        }
        private void WaitTimeSaveDelold() {
            if (saveLock != null) {
                using (saveLock.Write()) {
                    WaitTimeSave.RemoveWhere(a => a.TimeStamp < DateTime.Today.AddDays(-1 * TAGEHOLEN));
                }
            }
        }
        private Task CheckForUpdate(LiveData oldData, LiveData newData, bool forcerenew, DateTime zeitpunkt) {
            Stats stats = Stats.FromLiveData(newData, WaitTimeNow().WaitTime, zeitpunkt);
            stats.Changes = forcerenew || oldData == null || oldData.AmTelefon != stats.AmTelefon || oldData.Angemeldet != stats.Angemeldet || oldData.Benutzer != stats.Benutzer;
            return hubContext.Clients.All.SendAsync("stats", stats);
        }
    }
}
