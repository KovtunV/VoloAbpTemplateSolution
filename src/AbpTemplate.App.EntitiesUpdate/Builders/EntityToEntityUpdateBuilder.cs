using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpTemplate.App.EntitiesUpdate.Builders.Base;
using AbpTemplate.Domain.Entities.Base;

namespace AbpTemplate.App.EntitiesUpdate.Builders
{
    public class EntityToEntityUpdateBuilder<TEntity> : BaseUpdateBuilder<TEntity>
        where TEntity : BaseEntity
    {
        // Mandatory
        internal IEnumerable<TEntity> NewValues { get; }
        internal Func<TEntity, TEntity, bool> EqualsFunc { get; }
        internal Action<TEntity, TEntity> UpdatePropertiesAct { get; }

        // Manual
        internal Func<TEntity, bool> Filter { get; private set; }
        internal bool UseDelete { get; private set; }

        public EntityToEntityUpdateBuilder(
            EntitiesUpdateService updateService,
            IQueryable<TEntity> dbValuesQuery,
            IEnumerable<TEntity> newValues,
            Func<TEntity, TEntity, bool> equalsFunc,
            Action<TEntity, TEntity> updatePropertiesAct) : base(updateService, dbValuesQuery)
        {
            NewValues = newValues;
            EqualsFunc = equalsFunc;
            UpdatePropertiesAct = updatePropertiesAct;
        }

        public EntityToEntityUpdateBuilder<TEntity> WithDelete()
        {
            UseDelete = true;
            return this;
        }

        public EntityToEntityUpdateBuilder<TEntity> WithFilter(Func<TEntity, bool> filter)
        {
            Filter = filter;
            return this;
        }

        public override Task UpdateAsync()
        {
            return _updateService.UpdateAsync(this);
        }
    }
}
