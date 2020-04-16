using System;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Silbernetz.Models.Identity
{
    public class JwtManager
    {
        private readonly SymmetricSecurityKey symmetricKey;
        public const Int32 ExpireMinutes = 30;
        public JwtManager(IOptions<SecretConf> options)
        {
            symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.JWTSecret));
        }
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>

        public string GenerateToken(IdentityUserToken<Guid> tobject, DateTime now)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, tobject.Name)
                        }),
                Expires = now.AddMinutes(ExpireMinutes),

                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
