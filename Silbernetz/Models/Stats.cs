using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models
{
    public class Stats
    {
        /// <summary>
        /// Personen die Derzeit Ihr Telefon Eingeschaltet haben.
        /// </summary>
        [JsonPropertyName("angemeldet")]
        public int Angemeldet { get; set; }
        /// <summary>
        /// Personen die derzeit am Telefonieren sind
        /// </summary>
        [JsonPropertyName("amtelefon")]
        public int AmTelefon { get; set; }
        /// <summary>
        /// Benutzer, welche derzeit einen Account haben.
        /// </summary>
        [JsonPropertyName("benutzer")]
        public int Benutzer { get; set; }
        /// <summary>
        /// Zeitpunkt der Datenaufnahme
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }

        public static Stats FromLiveData(LiveData livedata)
        {
            return new Stats()
            {
                Angemeldet = livedata.Angemeldet,
                AmTelefon = livedata.AmTelefon,
                Benutzer = livedata.Benutzer,
                TimeStamp = livedata.TimeStamp
            };
        }
    }
}
