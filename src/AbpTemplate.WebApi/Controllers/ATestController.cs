using System.Threading.Tasks;
using AbpTemplate.App.Services.ATest;
using AbpTemplate.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace AbpTemplate.WebApi.Controllers
{
    public class ATestController : BaseController
    {
        private readonly TestService _testService;

        public ATestController(TestService testService)
        {
            _testService = testService;
        }

        [HttpPost("/api/ATest/test")]
        public async Task<ObjectResult> Test()
        {
            var scopeId = 1;
            var res = await Locker.LockAsync(scopeId, () => _testService.TestAsync());
            return Ok(res);
        }

    }
}
