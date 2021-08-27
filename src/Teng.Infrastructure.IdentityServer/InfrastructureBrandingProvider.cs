using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace Teng.Infrastructure
{
    [Dependency(ReplaceServices = true)]
    public class InfrastructureBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Infrastructure";
    }
}
