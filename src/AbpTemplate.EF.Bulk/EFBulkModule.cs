using Volo.Abp.Modularity;
using Z.EntityFramework.Plus;

namespace AbpTemplate.EF.Bulk
{
    public class EFBulkModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Allow IncludeOptimized(q => q.Collection.Select(s => s.SubEntity.SubEntity))
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;
        }
    }
}
