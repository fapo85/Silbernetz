using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models.Api
{
    public partial class BlackListItems
    {
        [JsonPropertyName("version")]
        public Version Version { get; set; }

        [JsonPropertyName("response")]
        public BLIResponse Response { get; set; }
    }

    public partial class BLIResponse
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public List<BLIDatum> Data { get; set; }
    }

    public partial class BLIDatum
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("caller")]
        public string Caller { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("own_id")]
        public object OwnId { get; set; }

        [JsonPropertyName("block")]
        public bool Block { get; set; }

        [JsonPropertyName("locked_up")]
        public string LockedUp { get; set; }

        [JsonPropertyName("count_try")]
        public long CountTry { get; set; }

        [JsonPropertyName("modified")]
        public string Modified { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }
    }
}
