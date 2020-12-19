using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models.Api {
    public class SipGroups {
        [JsonPropertyName("version")]
        public Version version { get; set; }
        [JsonPropertyName("response")]
        public ResponseGroup Response { get; set; }
    }
    public class GroupDatum {
        [JsonPropertyName("user")]
        public GroupUser User { get; set; }
        [JsonPropertyName("priority")]
        public int Priority { get; set; }
        [JsonPropertyName("last")]
        public LastCall Last { get; set; }
    }
    public class LastCall {
        [JsonPropertyName("call")]
        public String Call { get; set; }
    }
    public class GroupUser {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }
}
