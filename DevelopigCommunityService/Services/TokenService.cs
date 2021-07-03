using DevelopigCommunityService.DTOs.Bassal;
using DevelopigCommunityService.Interfaces;
using DevelopigCommunityService.Models.AbstractClasses.Bassal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevelopigCommunityService.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser appUser)
        {
            bool IsAdmin=false;
            if (appUser.GetType().Name == "Admin") IsAdmin = true;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Iss,IsAdmin==true?"Admin":"AppUser"),
                new Claim(JwtRegisteredClaimNames.Typ,appUser.GetType().Name)
                
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var TokenHandler = new JwtSecurityTokenHandler();

            var Token = TokenHandler.CreateToken(TokenDescriptor);


            return TokenHandler.WriteToken(Token);
        }


        public AuthDTOs GetJWTClams(String encodedJWT)
        {

            if (encodedJWT == null) return null;

            encodedJWT = encodedJWT.Substring(7);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(encodedJWT);

            var Token = jsonToken as JwtSecurityToken;

            var jwtClaim = Token.Claims.FirstOrDefault();


           
            return new AuthDTOs
            {
                Id=int.Parse(Token?.Claims?.FirstOrDefault().Value),
                NameId = Token?.Claims?.Skip(1).FirstOrDefault().Value,
                IsAdmin = Token?.Claims?.Skip(2).FirstOrDefault().Value == "Admin" ? true:false,
                Type=Token?.Claims.Skip(3).FirstOrDefault().Value
            };


            //jwtClaim.Value;

          

        }
    }
}
