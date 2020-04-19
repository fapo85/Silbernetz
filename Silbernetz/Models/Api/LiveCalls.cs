using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models.Api
{
    public partial class LiveCalls
    {
        [JsonPropertyName("version")]
        public Version Version { get; set; }

        [JsonPropertyName("response")]
        public Response Response { get; set; }
    }

    public partial class Response
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public List<CallDatum> Data { get; set; }
    }

    public partial class CallDatum
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }

        [JsonPropertyName("cdr")]
        public Cdr Cdr { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("ddi")]
        public object Ddi { get; set; }

        [JsonPropertyName("caller")]
        public string Caller { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("status")]
        public Modul Status { get; set; }

        [JsonPropertyName("dtmf")]
        public object Dtmf { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("modul")]
        public Modul Modul { get; set; }

        [JsonPropertyName("free")]
        public List<Free> Free { get; set; }

        [JsonPropertyName("link")]
        public List<Link> Link { get; set; }

        [JsonPropertyName("successfully")]
        public bool Successfully { get; set; }

        [JsonPropertyName("recording")]
        public Recording Recording { get; set; }

        [JsonPropertyName("streaming")]
        public bool Streaming { get; set; }

        [JsonPropertyName("history")]
        public History History { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }
    }

    public partial class Cdr
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public partial class Free
    {
        [JsonPropertyName("name")]
        public object Name { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }

    public partial class Link
    {
        [JsonPropertyName("name")]
        public object Name { get; set; }

        [JsonPropertyName("href")]
        public object Href { get; set; }
    }

    public partial class Modul
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public partial class Recording
    {
        [JsonPropertyName("state")]
        public bool State { get; set; }

        [JsonPropertyName("allow")]
        public bool Allow { get; set; }
    }

}
