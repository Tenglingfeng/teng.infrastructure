using Volo.Abp.Modularity;

namespace Teng.Infrastructure
{
    [DependsOn(
        typeof(InfrastructureApplicationModule),
        typeof(InfrastructureDomainTestModule)
        )]
    public class InfrastructureApplicationTestModule : AbpModule
    {

    }
}