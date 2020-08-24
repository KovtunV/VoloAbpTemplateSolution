using AbpTemplate.Synchronization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpTemplate.WebApi.Controllers.Base
{
    public abstract class BaseController : AbpController
    {
        #region Services

        private SynchronizationService _locker;
        protected SynchronizationService Locker
        {
            get => _locker ?? GetService(out _locker);
        }

        private T GetService<T>(out T service)
        {
            service = ServiceProvider.GetRequiredService<T>();
            return service;
        }

        #endregion

        #region Claims
        
        protected int MyСlaim
        {
            get => GetIntClaim("MyClaim");
        }

        private int GetIntClaim(string claimName)
        {
            var claimVal = GetClaimValue(claimName);
            return int.Parse(claimVal);
        }

        private string GetClaimValue(string claimName)
        {
            var claim = User.FindFirst(claimName);

            if (claim is null)
            {
                throw new UserFriendlyException($"Claim \"{claimName}\" not found");
            }

            return claim.Value;
        }

        #endregion
    }
}
