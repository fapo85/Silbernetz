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
                foreach(List<CallEvents> ev in Anrufe.Values)
                {
                    CallEvents Beginn = ev.Where(e => e.Event == EventType.connect).FirstOrDefault();
                    CallEvents Ende = ev.Where(e => e.Event == EventType.hangup).FirstOrDefault();
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
        public Dictionary<Guid, List<CallEvents>> Anrufe { get; set; } = new Dictionary<Guid, List<CallEvents>>();
    }
}
