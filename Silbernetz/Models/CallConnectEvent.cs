using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models
{
    public class CallConnectEvent
    {
        /// <summary>
        /// Eindeutige Call-ID
        /// </summary>
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }
        /// <summary>
        /// Event Typo
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("kid")]
        public string Kid { get; set; }

        //[JsonPropertyName("cdr_id")]
        //public string CdrId { get; set; }
        /// <summary>
        /// Aktuelle Routing ID
        /// </summary>
        [JsonPropertyName("routing_id")]
        public string RoutingId { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }
        /// <summary>
        /// Angewählte Rufnummer
        /// </summary>
        [JsonPropertyName("service")]
        public string Service { get; set; }
        /// <summary>
        /// Skill Gruppen ID
        /// </summary>
        [JsonPropertyName("skill_id")]
        public string SkillId { get; set; }
        /// <summary>
        /// Agenten ID
        /// </summary>
        [JsonPropertyName("agenten_id")]
        public string AgentenId { get; set; }
        /// <summary>
        /// Eigene Agenten ID
        /// </summary>
        [JsonPropertyName("externe_agenten_id")]
        public string ExterneAgentenID { get; set; }
        /// <summary>
        /// Angewählte DDI/Durchwahl
        /// </summary>
        [JsonPropertyName("ddi")]
        public string Ddi { get; set; }
        /// <summary>
        /// DTMF Übergabe
        /// </summary>
        [JsonPropertyName("dtmf")]
        public string Dtmf { get; set; }
        /// <summary>
        /// Rufnummer des Anrufers
        /// </summary>
        [JsonPropertyName("caller")]
        public string Caller { get; set; }
        /// <summary>
        /// Zielrufnummer
        /// </summary>
        [JsonPropertyName("destination_number")]
        public string DestinationNumber { get; set; }

        [JsonPropertyName("hangup_cause")]
        public string HangupCause { get; set; }
        /// <summary>
        /// Gesprächsdauer eingehend
        /// </summary>
        [JsonPropertyName("duration_in")]
        public string DurationIn { get; set; }
        /// <summary>
        /// Gesprächsdauer ausgehend
        /// </summary>
        [JsonPropertyName("duration_out")]
        public string DurationOut { get; set; }

        [JsonPropertyName("successfully")]
        public string Successfully { get; set; }
        /// <summary>
        /// Datum und Uhrzeit des Anrufes
        /// </summary>
        [JsonPropertyName("date_time")]
        public string DateTime { get; set; }

        [JsonPropertyName("session_id")]
        public object SessionId { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
    }
    public enum EventType
    {
        ringing, ringing_outbound, connect, connect_outbound, hangup, heartbeat, start_rp, start_skill
    }
}
