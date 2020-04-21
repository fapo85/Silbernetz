using System;
using System.Text.Json.Serialization;

namespace Silbernetz.Models
{
    public class WaitTimeProp
    {

        /// <summary>
        /// Dauer der Warteschlange
        /// </summary>
        [JsonPropertyName("waittime")]
        public ulong WaitTime { get; set; }

        /// <summary>
        /// Zeitpunkt der Datenaufnahme
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
