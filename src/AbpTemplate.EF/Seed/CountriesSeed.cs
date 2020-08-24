using System.Collections.Generic;
using AbpTemplate.Domain.Entities;
using AbpTemplate.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AbpTemplate.EF.Seed
{
    public static class CountriesSeed
    {
        public static IEnumerable<CountryEntity> AddCountries(this ModelBuilder modelBuilder)
        {
            var data = new[]
            {
                Create(1, "Russia"),
                Create(2, "Canada"),
                Create(3, "Australia")
            };

            modelBuilder.Entity<CountryEntity>(m => m.HasData(data));
            modelBuilder.IdStartAt<CountryEntity>(4);

            return data;
        }

        private static CountryEntity Create(int id,  string name)
        {
            return new CountryEntity(id)
            {
                Name = name
            };
        }
    }
}
