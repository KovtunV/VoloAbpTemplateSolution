using AbpTemplate.EF.EntityGroups;
using AbpTemplate.EF.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace AbpTemplate.EF
{
    [DependsOn(
        typeof(AbpEntityFrameworkCorePostgreSqlModule), 
        typeof(EFEntityGroupsModule))]
    public class EFModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;

            ConfigureDbContext(services);
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            services.AddAbpDbContext<TemplateDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true); 
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(o => o.UseProjectMigrations());
            });
        }
    }
}
