using AbpTemplate.App.EntitiesUpdate;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace AbpTemplate.App
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpEmailingModule),
        typeof(EntitiesUpdateModule),
        typeof(AbpAspNetCoreSignalRModule))]
    public class TemplateAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TemplateAppModule>();
            });
        }
    }
}
