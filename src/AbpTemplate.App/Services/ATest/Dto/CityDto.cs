using AbpTemplate.Domain.Entities;
using AutoMapper;

namespace AbpTemplate.App.Services.ATest.Dto
{
    [AutoMap(typeof(CityEntity))]
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
