using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Models
{
    public class SecretConf
    {
        public string InoplaApiId { get; set; }
        public string InoplaApiKey { get; set; }
        public string JWTSecret { get; set; }
        public string DBConnection { get; set; }
        public string DbName { get; set; }
    }
}
