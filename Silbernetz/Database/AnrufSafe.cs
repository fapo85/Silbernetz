using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Silbernetz.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Database
{
    public class AnrufSafe
    {
        private static ConcurrentDictionary<Guid, Anruf> Save = new ConcurrentDictionary<Guid, Anruf>();
        public Anruf AnrufGet(Guid id, string TelNummer)
        {
            return Save.GetOrAdd(id, new Anruf()
            {
                Uuid = id,
                TelNummer = TelNummer,
                TimeStamp = DateTime.Now
            });
        }
        public Anruf AnrufAddEvent(Guid id, string TelNummer, CallEvents callEvents)
        {
            var Anruf = AnrufGet(id, TelNummer);
            Anruf.Events.Add(callEvents);
            return AnrufUpdate(Anruf);
        }

        private Anruf AnrufUpdate(Anruf anruf)
        {
            return anruf;
        }

        public Anruf AddStatsToAnruf(Anruf anruf, Stats stats)
        {
            anruf.Angemeldet = stats.Angemeldet;
            anruf.AmTelefon = stats.AmTelefon;
            anruf.Benutzer = stats.Benutzer;
            anruf.TimeStamp = stats.TimeStamp;
            return AnrufUpdate(anruf);
        }
        public IEnumerable<Anrufer> AnrufFromToday()
        {
            Dictionary<string, Anrufer> ret = new Dictionary<string, Anrufer>();
            foreach (Anruf itm in Save.Values.Where(a => a.TelNummer != string.Empty))
            {
                Anrufer anrufer;
                if (!ret.TryGetValue(itm.TelNummer, out anrufer))
                {
                    anrufer = new Anrufer();
                    anrufer.TelNummer = itm.TelNummer;
                    ret.Add(itm.TelNummer, anrufer);
                }
                anrufer.Anrufe.Add(itm.Uuid, itm.Events);
            }
            return ret.Values;
        }
        public void AnrufCleanUp()
        {
            AnrufMakeAnonymous();
            AnrufDelOld();
        }
        private void AnrufMakeAnonymous()
        {
            foreach (Anruf itm in Save.Values.Where(a => a.TelNummer != string.Empty && a.TimeStamp < DateTime.Today))
            {
                itm.TelNummer = string.Empty;
                AnrufUpdate(itm);
            }
        }
        private void AnrufDelOld()
        {
            foreach (Anruf itm in Save.Values.Where(a => a.TimeStamp < DateTime.Today.AddDays(-30)))
            {
                Save.Remove(itm.Uuid, out var papierkorb);
            }
        }
    }
}
