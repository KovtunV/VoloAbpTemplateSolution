using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpTemplate.App.EntitiesUpdate.Builders.Base;
using AbpTemplate.Domain.Entities.Base;

namespace AbpTemplate.App.EntitiesUpdate.Builders
{
    public class DtoToEntityUpdateBuilder<TDto, TEntity> : BaseUpdateBuilder<TEntity>
        where TEntity : BaseEntity
    {
        // Mandatory
        internal IEnumerable<TDto> NewValues { get; }
        internal Func<TEntity, TDto, bool> EqualsFunc { get; }
        internal Func<TDto, TEntity> CreateFunc { get; }
        internal Action<TEntity, TDto> UpdatePropertiesAct { get; }

        // Manual
        internal Func<TDto, bool> Filter { get; private set; }
        internal Func<TDto, bool> CanDeleteFunc { get; private set; }
        internal bool UseDelete => CanDeleteFunc != null;


        public DtoToEntityUpdateBuilder(
            EntitiesUpdateService updateService,
            IQueryable<TEntity> dbValuesQuery,
            IEnumerable<TDto> newValues,
            Func<TDto, TEntity> createFunc,
            Func<TEntity, TDto, bool> equalsFunc,
            Action<TEntity, TDto> updatePropertiesAct) : base(updateService, dbValuesQuery)
        {
            NewValues = newValues;
            CreateFunc = createFunc;
            EqualsFunc = equalsFunc;
            UpdatePropertiesAct = updatePropertiesAct;
        }

        public DtoToEntityUpdateBuilder<TDto, TEntity> WithDelete(Func<TDto, bool> canDeleteFunc)
        {
            CanDeleteFunc = canDeleteFunc;
            return this;
        }

        public DtoToEntityUpdateBuilder<TDto, TEntity> WithFilter(Func<TDto, bool> filter)
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
