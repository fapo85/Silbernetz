using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models.Api
{

    public partial class LiveData
    {
        [JsonPropertyName("version")]
        public Version Version { get; set; }

        [JsonPropertyName("response")]
        public ResponseLive Response { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public int Abgemeldet => Response.Data.Where(r => r.Registrations.Count == 0).Count();
        public int Angemeldet => Response.Data.Where(r => r.Registrations.Count > 0).Count();
        public int Benutzer => Response.Data.Length;
        public int AmTelefon => Response.Data.Where(r => r.Live.Count > 0 && r.Registrations.Count > 0).Count();
        public int Verfügbar => Response.Data.Where(r => r.Live.Count == 0 && r.Registrations.Count > 0).Count();

    }



    public partial class ResponseDatum
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("displayname")]
        public string Displayname { get; set; }

        [JsonPropertyName("registrations")]
        public Registrations Registrations { get; set; }

        [JsonPropertyName("live")]
        public Live Live { get; set; }
    }

    public partial class Live
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public LiveDatum[] Data { get; set; }
    }

    public partial class LiveDatum
    {
        [JsonPropertyName("direction")]
        public string Direction { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }
    }

    public partial class Status
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public partial class Registrations
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("data")]
        public RegistrationsDatum[] Data { get; set; }
    }

    public partial class RegistrationsDatum
    {
        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("network")]
        public Network Network { get; set; }
    }

    public partial class Network
    {
        [JsonPropertyName("internal")]
        public Ternal Internal { get; set; }

        [JsonPropertyName("external")]
        public Ternal External { get; set; }
    }

    public partial class Ternal
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("port")]
        public string Port { get; set; }
    }

}
