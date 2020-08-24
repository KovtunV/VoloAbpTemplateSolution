using AbpTemplate.Domain.Entities;
using AbpTemplate.EF.Seed;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpTemplate.EF
{
    [ConnectionStringName("DefaultConnection")]
    public class TemplateDbContext : AbpDbContext<TemplateDbContext>
    {
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<CityEntity> Cities { get; set; }

        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var countries = modelBuilder.AddCountries();
            modelBuilder.AddCities(countries);
        }
    }
}
