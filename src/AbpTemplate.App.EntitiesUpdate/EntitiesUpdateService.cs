using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpTemplate.App.EntitiesUpdate.Builders;
using AbpTemplate.Domain.Entities.Base;
using AbpTemplate.EF.Bulk;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace AbpTemplate.App.EntitiesUpdate
{
    public class EntitiesUpdateService : ITransientDependency
    {
        private readonly BulkService _bulkService;

        public EntitiesUpdateService(BulkService bulkService)
        {
            _bulkService = bulkService;
        }

        #region EntityToEntity

        public EntityToEntityUpdateBuilder<TEntity> Configure<TEntity>(
            IQueryable<TEntity> dbValuesQuery,
            IEnumerable<TEntity> newValues,
            Func<TEntity, TEntity, bool> equalsFunc,
            Action<TEntity, TEntity> updatePropertiesAct) where TEntity : BaseEntity
        {
            return new EntityToEntityUpdateBuilder<TEntity>(this, dbValuesQuery, newValues, equalsFunc, updatePropertiesAct);
        }

        internal async Task UpdateAsync<TEntity>(EntityToEntityUpdateBuilder<TEntity> config)
            where TEntity : BaseEntity
        {
            var dbValues = await config.DbValuesQuery.AsNoTracking().ToListAsync();

            var insert = new List<TEntity>();
            var update = new List<TEntity>();
            var delete = new List<TEntity>();

            foreach (var value in config.NewValues.WhereIf(config.Filter != null, config.Filter))
            {
                var dbValue = dbValues.Find(dbVal => config.EqualsFunc(dbVal, value));
                if (dbValue != null)
                {
                    config.UpdatePropertiesAct(dbValue, value);
                    update.Add(dbValue);
                }
                else
                {
                    insert.Add(value);
                }
            }

            if (config.UseDelete)
            {
                delete.AddRange(dbValues.Where(dbValue => !config.NewValues.Any(val => config.EqualsFunc(dbValue, val))));
            }

            await _bulkService.UpdateAsync(update);
            await _bulkService.InsertAsync(insert);
            await _bulkService.DeleteAsync(delete);
        }

        #endregion

        #region DtoToEntity

        public DtoToEntityUpdateBuilder<TDto, TEntity> Configure<TDto, TEntity>(
            IQueryable<TEntity> dbValuesQuery,
            IEnumerable<TDto> newValues,
            Func<TDto, TEntity> createFunc,
            Func<TEntity, TDto, bool> equalsFunc,
            Action<TEntity, TDto> updatePropertiesAct) where TEntity : BaseEntity
        {
            return new DtoToEntityUpdateBuilder<TDto, TEntity>(this, dbValuesQuery, newValues, createFunc, equalsFunc, updatePropertiesAct);
        }

        internal async Task UpdateAsync<TDto, TEntity>(DtoToEntityUpdateBuilder<TDto, TEntity> config)
            where TEntity : BaseEntity
        {
            var dbValues = await config.DbValuesQuery.AsNoTracking().ToListAsync();

            var insert = new List<TEntity>();
            var update = new List<TEntity>();
            var delete = new List<TEntity>();

            foreach (var value in config.NewValues.WhereIf(config.Filter != null, config.Filter))
            {
                var dbValue = dbValues.Find(dbVal => config.EqualsFunc(dbVal, value));
                if (dbValue != null)
                {
                    if (config.UseDelete && config.CanDeleteFunc(value))
                    {
                        delete.Add(dbValue);
                    }
                    else
                    {
                        config.UpdatePropertiesAct(dbValue, value);
                        update.Add(dbValue);
                    }
                }
                else
                {
                    var entity = config.CreateFunc(value);
                    insert.Add(entity);
                }
            }

            await _bulkService.UpdateAsync(update);
            await _bulkService.InsertAsync(insert);
            await _bulkService.DeleteAsync(delete);
        }

        #endregion
    }
}
