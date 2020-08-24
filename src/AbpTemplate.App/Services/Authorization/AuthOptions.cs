using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AbpTemplate.App.Services.Authorization
{
    public class AuthOptions
    {
        /// <summary>
        /// Token lifetime, minutes
        /// </summary>
        public const int LIFETIME = 1440;

        /// <summary>
        /// Issuer
        /// </summary>
        public const string ISSUER = "MyCompany";

        /// <summary>
        /// Audience
        /// </summary>
        public const string AUDIENCE = "MyUser";

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string jwtSecret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        }
    }
}
