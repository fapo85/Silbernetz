using System;
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
    }
}
