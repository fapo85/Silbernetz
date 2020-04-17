using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Silbernetz.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Database
{
    public class AnrufSafe
    {
        private static readonly ConcurrentDictionary<Guid, Anruf> Save = new ConcurrentDictionary<Guid, Anruf>();
        private static readonly Random random = new Random();
        static AnrufSafe()
        {
            //ADD FAKE DATA
            DateTime Zeitpunkt = DateTime.Today.AddDays(-30);
            int rand = 5;
            while(Zeitpunkt < DateTime.Now)
            {
                rand = NextRandom(rand);
                if(Zeitpunkt < DateTime.Today)
                {
                    AddFakeAnruf(string.Empty, Zeitpunkt.AddSeconds(random.Next(800)), rand);
                }
                else
                {
                    AddFakeAnruf(GetRandomTel(), Zeitpunkt.AddSeconds(200), rand);
                    for (int i = 0; i < rand; i++)
                    {
                        AddFakeAnruf(GetRandomTel(), Zeitpunkt.AddSeconds(3000), rand);
                    }
                }
                Zeitpunkt = Zeitpunkt.AddHours(1);
            }
        }

        private static string GetRandomTel()
        {
            string[] telnr =
            {
                "0147852369",
                "03025478526",
                "017452558944",
                "014588785255",
                "014789899956",
                "0302587415454",
                "014587754225",
                "0145877752369",
                "0145877853669",
                "014785225898",
                "03025877782"
            };
            return telnr[random.Next(telnr.Length)];
        }

        private static int NextRandom(int rand)
        {
            if(random.Next(0, 2) == 0)
            {
                rand = Math.Max(0, rand - 1);
            }
            else
            {
                rand = Math.Min(10, rand + 1);
            }
            return rand;
        }

        private static void AddFakeAnruf(string Telnummer, DateTime Zeitpunkt, int Rand)
        {
            Anruf anruf = new Anruf()
            {
                Uuid = Guid.NewGuid(),
                TelNummer = Telnummer,
                TimeStamp = Zeitpunkt,
                Benutzer = 70,
                Angemeldet = (int)((double)Rand * (3 * random.NextDouble())),
                AmTelefon = (int)((double)Rand * random.NextDouble()),
            };
            if (!string.IsNullOrEmpty(Telnummer)) {
                anruf.Events.Add(new CallEvents()
                {
                    TimeStamp = Zeitpunkt,
                    Event = EventType.connect
                });
                anruf.Events.Add(new CallEvents()
                {
                    TimeStamp = Zeitpunkt.AddSeconds(random.Next(2000)),
                    Event = EventType.hangup
                });
            }
            Save.GetOrAdd(anruf.Uuid, anruf);
        }
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
                anrufer.Anrufe.Add(new AnrufExport(itm.Uuid, itm.Events));
            }
            return ret.Values;
        }
        public IEnumerable<Stats> AnrufStatisitk(DateTime after)
        {
            return Save.Values.Where(a => a.TimeStamp > after).OrderBy(a => a.TimeStamp).Select(a => new Stats()
            {
                Angemeldet = a.Angemeldet,
                AmTelefon = a.AmTelefon,
                Benutzer = a.Benutzer,
                TimeStamp = a.TimeStamp
            });
        }
        public void AnrufCleanUp()
        {
            AnrufMakeAnonymous();
         //   AnrufDelOld();
        }
        private void AnrufMakeAnonymous()
        {
            foreach (Anruf itm in Save.Values.Where(a => a.TelNummer != string.Empty && a.TimeStamp < DateTime.Today))
            {
                itm.TelNummer = string.Empty;
                itm.Events = new List<CallEvents>();
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
