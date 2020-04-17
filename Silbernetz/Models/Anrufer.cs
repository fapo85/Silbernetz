using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Models
{
    public class Anrufer
    {
        public string TelNummer { get; set; }
        public int Anzahl {
            get {
                return Anrufe.Count;
            }
        }
        public int GesamtDauer {
            get {
                long dauer = 0;
                foreach(AnrufExport ae in Anrufe)
                {
                    CallEvents Beginn = ae.events.Where(e => e.Event == EventType.connect).FirstOrDefault();
                    CallEvents Ende = ae.events.Where(e => e.Event == EventType.hangup).FirstOrDefault();
                    if(Beginn != null)
                    {
                        DateTime tsBeginn = Beginn.TimeStamp;
                        DateTime tsEnde = Ende?.TimeStamp ?? DateTime.Now;
                        dauer += (tsEnde.Ticks - tsBeginn.Ticks);
                    }
                }
                return (int)(dauer / TimeSpan.TicksPerSecond);
            }
        }
        public List<AnrufExport> Anrufe { get; set; } = new List<AnrufExport>();
    }
    public class AnrufExport
    {
        public Guid guid { get; set; }
        public List<CallEvents> events { get; set; }
        public AnrufExport(Guid guid, List<CallEvents> events)
        {
            this.guid = guid;
            this.events = events;
        }
    }
}
