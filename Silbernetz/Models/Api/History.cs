using System.Text.Json.Serialization;

namespace Silbernetz.Models.Api
{

    public partial class History
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }
}
