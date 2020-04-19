using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silbernetz.Models.Api
{
    public partial class Evn
    {
        [JsonPropertyName("version")]
        public Version Version { get; set; }

        [JsonPropertyName("response")]
        public ResponseEvn Response { get; set; }
    }


    public partial class Datum
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("duration")]
        public Duration Duration { get; set; }

        [JsonPropertyName("caller")]
        public string Caller { get; set; }

        [JsonPropertyName("successfully")]
        public bool Successfully { get; set; }

        [JsonPropertyName("history")]
        public History History { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }
    }

    public partial class Duration
    {
        [JsonPropertyName("inbound")]
        public uint Inbound { get; set; }

        [JsonPropertyName("outbound")]
        public uint Outbound { get; set; }
    }

}
