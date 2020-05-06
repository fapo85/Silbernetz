using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models
{
    public class Anrufer
    {
        [JsonPropertyName("telnummer")]
        public string TelNummer { get; set; }
        [JsonPropertyName("anzahldays")]
        public int Anzahl3Days {
            get {
                return Anrufe.Count;
            }
        }
        [JsonPropertyName("anzahl")]
        public int Anzahl {
            get {
                return Anrufe.Where(ae => ae.TimeStamp > DateTime.Today).Count();
            }
        }
        [JsonPropertyName("gesamtdauer")]
        public ulong GesamtDauer {
            get {
                ulong dauer = 0;
                foreach (Anruf ae in Anrufe.Where(ae => ae.TimeStamp > DateTime.Today))
                {
                    dauer += ae.OutBound;
                }
                return dauer;
            }
        }
        [JsonPropertyName("gesamtdauerdays")]
        public ulong GesamtDauer3Days {
            get {
                ulong dauer = 0;
                foreach (Anruf ae in Anrufe)
                {
                    dauer += ae.OutBound;
                }
                return dauer;
            }
        }
        [JsonPropertyName("anrufe")]
        public List<Anruf> Anrufe { get; set; } = new List<Anruf>();
    }
}
