using AbpTemplate.EF;
using AbpTemplate.EF.Bulk;
using AbpTemplate.EF.EntityGroups;
using AbpTemplate.EF.EntityGroups.Groups;
using AbpTemplate.EF.EntityGroups.Groups.Common;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Services;
using Volo.Abp.EntityFrameworkCore;

namespace AbpTemplate.App.Base
{
    public abstract class BaseRepoService : ApplicationService
    {
        #region EntityGroups

        protected EmptyEntityGroup Entities => EntityGroupService.GetEntityGroup<EmptyEntityGroup>();
        protected CityEntityGroup City => EntityGroupService.GetEntityGroup<CityEntityGroup>();
        protected CountryEntityGroup Country => EntityGroupService.GetEntityGroup<CountryEntityGroup>();

        #endregion

        #region EntitiGroupService

        private EntitiGroupService _entityGroupService;
        protected EntitiGroupService EntityGroupService
        {
            get => _entityGroupService ?? GetService(out _entityGroupService);
        }

        #endregion

        #region Bulk

        private BulkService _bulk;
        protected BulkService Bulk
        {
            get => _bulk ?? GetService(out _bulk);
        }

        #endregion

        #region DBConextProvider

        private IDbContextProvider<TemplateDbContext> _dbContextProvider;
        protected TemplateDbContext DbContext
        {
            get => _dbContextProvider?.GetDbContext() ?? GetService(out _dbContextProvider).GetDbContext();
        }

        #endregion

        protected T GetService<T>(out T service)
        {
            service = ServiceProvider.GetRequiredService<T>();
            return service;
        }
    }
}
