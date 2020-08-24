using AbpTemplate.Domain.Entities.Base;

namespace AbpTemplate.Domain.Entities
{
    public class CityEntity : BaseEntity
    {
        public string Name { get; set; }

        public CountryEntity Country { get; set; }
        public int? CountryId { get; set; }

        public CityEntity()
        {
            
        }

        public CityEntity(int id) : base(id)
        {
            
        }
    }
}
