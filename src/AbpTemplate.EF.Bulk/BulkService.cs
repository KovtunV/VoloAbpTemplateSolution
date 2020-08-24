using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AbpTemplate.Domain.Entities.Base;
using AbpTemplate.EF.Bulk.Extensions;
using AbpTemplate.EF.EntityGroups.Groups.Common;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace AbpTemplate.EF.Bulk
{
    public class BulkService : ITransientDependency
    {
        private IDbContextProvider<TemplateDbContext> _dbContextProvider;
        private EmptyEntityGroup _entities;

        public BulkService(IDbContextProvider<TemplateDbContext> dbContextProvider, EmptyEntityGroup entities)
        {
            _dbContextProvider = dbContextProvider;
            _entities = entities;
        }

        #region Insert

        public async Task<int> InsertAsync<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            await _entities.GetRepository<TEntity>().InsertAsync(entity, autoSave: true);
            return entity.Id;
        }

        public Task InsertAsync<T>(params T[] entities)
            where T : BaseEntity
        {
            return InsertAsync(entities?.AsEnumerable());
        }

        public async Task InsertAsync<T>(IEnumerable<T> entities)
            where T : BaseEntity
        {
            if (entities is null || !entities.Any())
            {
                return;
            }

            // Manual set due to it isn't AbpFramework
            SetCreationTime(entities);
            await BulkExtensions.BulkInsertAsync(_dbContextProvider.GetDbContext(), entities);
        }

        #endregion

        #region Update

        public Task UpdateAsync<TEntity>(params TEntity[] entities)
            where TEntity : BaseEntity
        {
            return UpdateAsync(entities?.AsEnumerable());
        }

        public async Task UpdateAsync<T>(IEnumerable<T> entities)
            where T : BaseEntity
        {
            if (entities is null || !entities.Any())
            {
                return;
            }

            // Manual set due to it isn't AbpFramework
            SetLastModificationTime(entities);
            await BulkExtensions.BulkUpdateAsync(_dbContextProvider.GetDbContext(), entities);
        }

        #endregion

        #region Delete

        public Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : BaseEntity
        {
            return _entities.Get<TEntity>().Where(predicate).DeleteAsync();
        }

        public Task<int> DeleteAsync<TEntity>(IQueryable<TEntity> query)
            where TEntity : BaseEntity
        {
            return query.DeleteAsync();
        }

        public Task<int> DeleteAsync<TEntity>(params TEntity[] entities)
            where TEntity : BaseEntity
        {
            return DeleteAsync(entities?.AsEnumerable());
        }

        public Task<int> DeleteAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            return _entities.Get<TEntity>().DeleteAsync(entities);
        }

        #endregion

        private void SetCreationTime<T>(IEnumerable<T> entities)
            where T : BaseEntity
        {
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.CreationTime = now;
            }
        }

        private void SetLastModificationTime<T>(IEnumerable<T> entities)
            where T : BaseEntity
        {
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.LastModificationTime = now;
            }
        }
    }
}
