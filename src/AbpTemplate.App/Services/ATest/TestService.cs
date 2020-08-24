using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpTemplate.App.Base;
using AbpTemplate.App.Services.ATest.Dto;
using Microsoft.EntityFrameworkCore;

namespace AbpTemplate.App.Services.ATest
{
    public class TestService : BaseAppService
    {
        public async Task<IList<CityDto>> TestAsync()
        {
            var cities = await City.GetCities().ToListAsync();
            var countries = await Country.GetCountries().ToListAsync();

            IsTrue(false, "df");
            var dtos = cities.Select(s => Map(s).To<CityDto>()).ToList();
            return dtos;
        }
    }
}
