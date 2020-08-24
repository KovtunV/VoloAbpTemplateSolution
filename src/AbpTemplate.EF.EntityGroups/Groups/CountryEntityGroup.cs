using System;
using System.Linq;
using AbpTemplate.Domain.Entities;
using AbpTemplate.EF.EntityGroups.Groups.Base;

namespace AbpTemplate.EF.EntityGroups.Groups
{
    public class CountryEntityGroup : BaseEntityGroup
    {
        public CountryEntityGroup(IServiceProvider services) : base(services)
        {

        }

        public IQueryable<CountryEntity> GetCountries()
        {
            return GetAll<CountryEntity>();
        }
    }
}
