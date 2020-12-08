using AbpTemplate.App.EntitiesUpdate;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using AbpTemplate.App.Utils;

namespace AbpTemplate.App.Base
{
    public abstract class BaseAppService : BaseRepoService
    {
        #region Mapper

        protected TDest Map<TDest>(object source)
        {
            return ObjectMapper.Map<TDest>(source);
        }

        #endregion

        #region Claims

        private int? _myClaim;
        protected int MyClaim
        {
            get => _myClaim ?? GetIntClaim("MyClaim", out _myClaim);
        }

        private int GetIntClaim(string claimName, out int? valRef)
        {
            var claimVal = GetClaimValue(claimName);
            valRef = int.Parse(claimVal);
            return valRef.Value;
        }

        private string GetClaimValue(string claimName)
        {
            var claim = HttpContext.User.FindFirst(claimName);

            if (claim is null)
            {
                throw new UserFriendlyException($"Claim \"{claimName}\" not found");
            }

            return claim.Value;
        }

        #endregion

        #region HttpContext

        private IHttpContextAccessor _httpContext;
        protected HttpContext HttpContext
        {
            get => _httpContext?.HttpContext ?? GetService(out _httpContext).HttpContext;
        }

        #endregion

        #region Exceptions

        protected void IsNotNull(object val, string failMessage)
        {
            if (val is null)
            {
                throw new UserFriendlyException(failMessage);
            }
        }

        protected void IsTrue(bool val, string failMessage)
        {
            if (!val)
            {
                throw new UserFriendlyException(failMessage);
            }
        }

        #endregion

        #region EntitiesUpdateService

        private EntitiesUpdateService _entitiesUpdate;
        protected EntitiesUpdateService EntitiesUpdate
        {
            get => _entitiesUpdate ?? GetService(out _entitiesUpdate);
        }

        #endregion
    }
}
