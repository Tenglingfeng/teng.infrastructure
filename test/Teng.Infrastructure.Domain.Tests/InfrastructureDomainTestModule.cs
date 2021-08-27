using Teng.Infrastructure.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Teng.Infrastructure
{
    [DependsOn(
        typeof(InfrastructureEntityFrameworkCoreTestModule)
        )]
    public class InfrastructureDomainTestModule : AbpModule
    {

    }
}