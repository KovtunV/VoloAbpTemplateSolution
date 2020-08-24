using System;
using System.Collections.Generic;
using System.Linq;
using AbpTemplate.Domain.Entities;
using AbpTemplate.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AbpTemplate.EF.Seed
{

    public static class CitiesSeed
    {
        public static void AddCities(this ModelBuilder modelBuilder, IEnumerable<CountryEntity> countries)
        {
            var russiaId = countries.First(f => f.Name.Equals("Russia", StringComparison.InvariantCulture)).Id;

            var data = new[]
            {
                Create(1, russiaId, "Moscow"),
                Create(2, russiaId, "Perm"),
            };

            modelBuilder.Entity<CityEntity>(m => m.HasData(data));
            modelBuilder.IdStartAt<CityEntity>(3);
        }

        private static CityEntity Create(int id, int countryId, string name)
        {
            return new CityEntity(id)
            {
                CountryId = countryId,
                Name = name
            };
        }
    }
}
