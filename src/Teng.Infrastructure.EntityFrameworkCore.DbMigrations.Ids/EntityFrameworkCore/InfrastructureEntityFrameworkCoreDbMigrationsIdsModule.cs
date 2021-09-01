using Microsoft.Extensions.DependencyInjection;
using Teng.Infrastructure.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Teng.Infrastructure.Ids.EntityFrameworkCore
{
    [DependsOn(typeof(InfrastructureEntityFrameworkCoreModule))]
    public class InfrastructureEntityFrameworkCoreDbMigrationsIdsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDbContext<InfrastructureMigrationIdsDbContext>();
            base.ConfigureServices(context);
        }
    }
}