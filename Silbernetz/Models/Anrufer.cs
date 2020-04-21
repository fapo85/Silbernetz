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
        [JsonPropertyName("anzahl")]
        public int Anzahl {
            get {
                return Anrufe.Count;
            }
        }
        [JsonPropertyName("gesamtdauer")]
        public ulong GesamtDauer {
            get {
                ulong dauer = 0;
                foreach(Anruf ae in Anrufe)
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
