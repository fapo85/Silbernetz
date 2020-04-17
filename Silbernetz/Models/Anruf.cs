using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silbernetz.Models
{
    public class Anruf : Stats
    {
        /// <summary>
        /// Eindeutige Call-ID
        /// </summary>
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }
        public string TelNummer { get; set; }
        public List<CallEvents> Events { get; set; } = new List<CallEvents>();
    }
    public class CallEvents
    {
        public DateTime TimeStamp { get; set; }
        public EventType Event { get; set; }
        public CallEvents() { }
        public CallEvents(CallConnectEvent eventSource)
        {
            TimeStamp = DateTime.Now;
            Event = (EventType)Enum.Parse(typeof(EventType), eventSource.Event);
        }
    }
}
