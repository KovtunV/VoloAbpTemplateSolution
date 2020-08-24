using AbpTemplate.App.EntitiesUpdate;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Abp.ObjectMapping;

namespace AbpTemplate.App.Base
{
    public abstract class BaseAppService : BaseRepoService
    {
        #region Mapper

        protected MapHelper<TSource> Map<TSource>(TSource source)
        {
            // I've done it, bacause don't want to write <TypeFrom, TypeTo>
            return new MapHelper<TSource>(ObjectMapper, source);
        }

        // I have no idea why IObjectMapper doesn't contain a Map<TDest> method
        protected class MapHelper<TSource>
        {
            private IObjectMapper _mapper;
            private TSource _source;

            public MapHelper(IObjectMapper mapper, TSource source)
            {
                _mapper = mapper;
                _source = source;
            }

            public TDest To<TDest>()
            {
                return _mapper.Map<TSource, TDest>(_source);
            }
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
