using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Silbernetz.Models.Api
{
    public class BLAction
    {
        private const int MAXDAYS = 7;
        /// <summary>
        /// Rufnummer
        /// </summary>
        [JsonPropertyName("caller")]
        string Caller { get; set; }
        /// <summary>
        /// Einzeilige Text-Eingabe

        /// </summary>
        [JsonPropertyName("description")]
        string Description { get; set; }
        /// <summary>
        /// Blocken
        /// </summary>
        [JsonPropertyName("block")]
        bool Block { get; set; }
        /// <summary>
        /// Rufnummer
        /// </summary>
        [JsonPropertyName("locked_up")]
        string Locked_Up { get; set; }
        public BLAction(string Caller, string Description, int tage = 1)
        {
            if(tage < 1 || tage > (1+ MAXDAYS))
            {
                throw new Exception($"Es sind Maximal {MAXDAYS} Tage erlaubt, gesendet {tage}");
            }
            this.Caller = Caller;
            this.Description = Description;
            this.Block = true;
            DateTime zp = DateTime.Today.AddDays(tage).AddHours(7);
            this.Locked_Up = zp.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
