using System;
using System.Linq;
using AbpTemplate.Domain.Entities;
using AbpTemplate.EF.EntityGroups.Groups.Base;

namespace AbpTemplate.EF.EntityGroups.Groups
{
    public class CityEntityGroup : BaseEntityGroup
    {
        public CityEntityGroup(IServiceProvider services) : base(services)
        {

        }

        public IQueryable<CityEntity> GetCities()
        {
            return GetAll<CityEntity>();
        }

        [Obsolete("EntitiesUpdateService usage example")]
        public bool EqualsToUpdate(CityEntity dbEntity, CityEntity newEntity)
        {
            return string.Equals(dbEntity.Name, newEntity.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        [Obsolete("EntitiesUpdateService usage example")]
        public void UpdateProperties(CityEntity dbEntity, CityEntity newEntity)
        {
            dbEntity.Name = newEntity.Name;
            // Other properties to update
        }
    }
}
