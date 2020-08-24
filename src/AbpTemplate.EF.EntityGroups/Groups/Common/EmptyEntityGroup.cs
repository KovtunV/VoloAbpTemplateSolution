using System;
using System.Linq;
using AbpTemplate.Domain.Entities.Base;
using AbpTemplate.EF.EntityGroups.Groups.Base;
using Volo.Abp.Domain.Repositories;

namespace AbpTemplate.EF.EntityGroups.Groups.Common
{
    public sealed class EmptyEntityGroup : BaseEntityGroup
    {
        public EmptyEntityGroup(IServiceProvider services)
            : base(services)
        {

        }

        // I wanna have these methods opened only here

        public IQueryable<TEntity> GetNotTracking<TEntity>()
            where TEntity : BaseEntity
        {
            return GetAllNotTracking<TEntity>();
        }

        public IQueryable<TEntity> Get<TEntity>()
            where TEntity : BaseEntity
        {
            return GetAll<TEntity>();
        }

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity
        {
            return GetRepo<TEntity>();
        }
    }
}
