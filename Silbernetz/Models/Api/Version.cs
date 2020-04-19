using System.Text.Json.Serialization;

namespace Silbernetz.Models.Api
{

    public partial class Version
    {
        [JsonPropertyName("used")]
        public string Used { get; set; }

        [JsonPropertyName("expire")]
        public string Expire { get; set; }

        [JsonPropertyName("new_available")]
        public object NewAvailable { get; set; }
    }
}
