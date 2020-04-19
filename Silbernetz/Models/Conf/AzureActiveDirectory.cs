using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Models.Conf
{
    public class AzureActiveDirectory
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string Issuer { get; set; }
        public string ClientKey { get; set; }
    }
}
