using System.Linq;
using System.Threading.Tasks;
using AbpTemplate.Domain.Entities.Base;

namespace AbpTemplate.App.EntitiesUpdate.Builders.Base
{
    public abstract class BaseUpdateBuilder<TEntity>
        where TEntity: BaseEntity
    {
        protected readonly EntitiesUpdateService _updateService;

        // Mandatory
        internal IQueryable<TEntity> DbValuesQuery { get; }

        protected BaseUpdateBuilder(EntitiesUpdateService updateService, IQueryable<TEntity> dbValuesQuery)
        {
            _updateService = updateService;
            DbValuesQuery = dbValuesQuery;
        }

        public abstract Task UpdateAsync();
    }
}
