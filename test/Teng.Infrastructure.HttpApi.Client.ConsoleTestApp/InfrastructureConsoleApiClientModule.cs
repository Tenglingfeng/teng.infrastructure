using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Teng.Infrastructure.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(InfrastructureHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class InfrastructureConsoleApiClientModule : AbpModule
    {
        
    }
}
