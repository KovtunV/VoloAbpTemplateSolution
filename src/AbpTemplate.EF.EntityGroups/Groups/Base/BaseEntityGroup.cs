using System;
using System.Collections.Concurrent;
using System.Linq;
using AbpTemplate.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace AbpTemplate.EF.EntityGroups.Groups.Base
{
    public abstract class BaseEntityGroup : ITransientDependency
    {
        private readonly ConcurrentDictionary<Type, IRepository> _repositories;
        protected readonly IServiceProvider _services;

        protected BaseEntityGroup(IServiceProvider services)
        {
            _repositories = new ConcurrentDictionary<Type, IRepository>();
            _services = services;
        }

        #region Repo

        protected IQueryable<TEntity> GetAllNotTracking<TEntity>()
            where TEntity : BaseEntity
        {
            return GetRepo<TEntity>().AsNoTracking();
        }

        protected IQueryable<TEntity> GetAll<TEntity>()
            where TEntity : BaseEntity
        {
            return GetRepo<TEntity>().AsTracking();
        }

        protected IRepository<TEntity> GetRepo<TEntity>()
            where TEntity : BaseEntity
        {
            var entityType = typeof(TEntity);

            if (!_repositories.TryGetValue(entityType, out var repo))
            {
                var repoType = typeof(IRepository<TEntity>);
                repo = (IRepository)_services.GetRequiredService(repoType);

                // If it isn't possible to add, then try again,
                // several threads can simultaneously try to add a different repo to the dictionary
                if (!_repositories.TryAdd(entityType, repo))
                {
                    return GetRepo<TEntity>();
                }
            }

            return (IRepository<TEntity>)repo;
        }

        #endregion
    }
}
