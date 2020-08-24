using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AbpTemplate.App.Base;
using Microsoft.IdentityModel.Tokens;

namespace AbpTemplate.App.Services.Authorization
{
    public class AuthService : BaseAppService
    {
        public async Task<string> CreateJwtAsync()
        {
            var jwtSecret = await SettingProvider.GetOrNullAsync("JwtSecret");
            var jwtIdentity = GetJwtIdentity();
            var jwt = GetJwt(jwtIdentity, jwtSecret);

            return jwt;
        }

        private ClaimsIdentity GetJwtIdentity()
        {
            var claims = new List<Claim>
            {
                new Claim("MyClaim", 25.ToString())
               // new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
              //  new Claim("UserId", user.Id.ToString())
            };

       
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private string GetJwt(ClaimsIdentity identity, string jwtSecret)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(jwtSecret), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
