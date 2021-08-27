using Teng.Infrastructure.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Teng.Infrastructure.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(InfrastructureEntityFrameworkCoreDbMigrationsModule),
        typeof(InfrastructureApplicationContractsModule)
        )]
    public class InfrastructureDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
