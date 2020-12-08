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
            var cities = await City.GetCities().Include(q => q.Country).ToListAsync();
            var countries = await Country.GetCountries().Include(q => q.Cities).ToListAsync();

            var dtos = cities.Select(Map<CityDto>).ToList();
            return dtos;
        }
    }
}
