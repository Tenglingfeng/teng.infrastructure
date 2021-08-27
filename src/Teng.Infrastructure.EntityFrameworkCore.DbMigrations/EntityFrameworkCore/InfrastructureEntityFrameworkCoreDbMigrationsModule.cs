using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Teng.Infrastructure.EntityFrameworkCore
{
    [DependsOn(
        typeof(InfrastructureEntityFrameworkCoreModule)
        )]
    public class InfrastructureEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<InfrastructureMigrationsDbContext>();
        }
    }
}
