using System.Collections.Generic;
using AbpTemplate.Domain.Entities.Base;

namespace AbpTemplate.Domain.Entities
{
    public class CountryEntity : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<CityEntity> Cities { get; set; }

        public CountryEntity()
        {
            Cities = new HashSet<CityEntity>();
        }

        public CountryEntity(int id) : base(id)
        {
            Cities = new HashSet<CityEntity>();
        }
    }
}
