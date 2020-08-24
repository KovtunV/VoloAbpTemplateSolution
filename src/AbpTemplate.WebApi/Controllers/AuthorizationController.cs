using System.Threading.Tasks;
using AbpTemplate.App.Services.Authorization;
using AbpTemplate.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbpTemplate.WebApi.Controllers
{
    public class AuthorizationController : BaseController
    {
        private readonly AuthService _authService;

        public AuthorizationController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/api/Auth/login")]
        public async Task<ObjectResult> Login()
        {
            var res = await _authService.CreateJwtAsync();
            return Ok(res);
        }

        [HttpPost("/api/Auth/test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ObjectResult Test()
        {
            var res = MyСlaim;
            return Ok(res);
        }
    }
}
