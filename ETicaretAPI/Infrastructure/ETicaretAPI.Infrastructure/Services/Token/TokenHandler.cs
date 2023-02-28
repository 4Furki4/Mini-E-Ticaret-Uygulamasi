using ETicaretAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration configuration;

        public TokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int minute)
        {
            Application.DTOs.Token token = new();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            token.ExpirationDate = DateTime.Now.AddMinutes(minute);

            //Token Configurations
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    audience: configuration["Token:Audience"], // same values used in program.cs, defined in appsettings
                    issuer: configuration["Token:Issuer"],
                    expires: token.ExpirationDate, // defined up there,
                    notBefore: DateTime.Now, // token begins just after creation
                    signingCredentials: credentials // encripted securitykey
                );


            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            token.AccessToken = handler.WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
