using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silbernetz.Models.Identity
{
    public class TokenWith<T>
    {

        public T Data { get; set; }
        public string Token { get; set; }
        public TokenWith() { }
        public TokenWith(T Data, string Token)
        {
            this.Data = Data;
            this.Token = Token;
        }
    }
}
