using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace AbpTemplate.EF.Extensions
{
    public static class MigrationAssemblyExtensions
    {
        public static NpgsqlDbContextOptionsBuilder UseProjectMigrations(this NpgsqlDbContextOptionsBuilder builder)
        {
            return builder.MigrationsAssembly("AbpTemplate.EF.Migrations");
        }
    }
}
