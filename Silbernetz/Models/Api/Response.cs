using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silbernetz.Models.Api
{
    public partial class ResponseLive
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public ResponseDatum[] Data { get; set; }

    }

    public partial class ResponseEvn {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public Datum[] Data { get; set; }
    }
    public partial class ResponseGroup {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public GroupDatum[] Data { get; set; }
    }
}
