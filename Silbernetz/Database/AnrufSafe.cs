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
        public Anruf GetAnruf(Guid id, string TelNummer)
        {
            return Save.GetOrAdd(id, new Anruf()
            {
                Uuid = id,
                TelNummer = TelNummer,
                TimeStamp = DateTime.Now
            });
        }
        public Anruf AddEventToAnruf(Guid id, string TelNummer, CallEvents callEvents){
            var Anruf = GetAnruf(id, TelNummer);
            Anruf.Events.Add(callEvents);
            return UpdateAnruf(Anruf);
        }

        private Anruf UpdateAnruf(Anruf anruf)
        {
            return anruf;
        }

        public Anruf AddStats(Anruf anruf, Stats stats)
        {
            anruf.Angemeldet = stats.Angemeldet;
            anruf.AmTelefon = stats.AmTelefon;
            anruf.Benutzer = stats.Benutzer;
            anruf.TimeStamp = stats.TimeStamp;
            return UpdateAnruf(anruf);
        }
    }
}
