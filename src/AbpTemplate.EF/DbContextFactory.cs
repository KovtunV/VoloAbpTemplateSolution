using System.IO;
using AbpTemplate.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpTemplate.EF
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TemplateDbContext>
    {
        public TemplateDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<TemplateDbContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), o => o.UseProjectMigrations());
            
            return new TemplateDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false);

            return builder.Build();
        }
    }
}
