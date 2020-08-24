using Volo.Abp.Settings;

namespace AbpTemplate.WebApi.Settings
{
    public class ProjectSettingsProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition("JwtSecret"));
        }
    }
}
