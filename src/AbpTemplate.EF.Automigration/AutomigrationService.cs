using AbpTemplate.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AbpTemplate.EF.Automigration
{
    public static class AutomigrationService
    {
        public static void Migrate(string connectionString)
        {
            using var context = CreateDbContext(connectionString);
            context.Database.Migrate();
        }

        private static TemplateDbContext CreateDbContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<TemplateDbContext>();
            builder.UseNpgsql(connectionString, o => o.UseProjectMigrations());

            return new TemplateDbContext(builder.Options);
        }
    }
}
