using Teng.Infrastructure.Localization;
using Volo.Abp.Application.Services;

namespace Teng.Infrastructure
{
    /* Inherit your application services from this class.
     */
    public abstract class InfrastructureAppService : ApplicationService
    {
        protected InfrastructureAppService()
        {
            LocalizationResource = typeof(InfrastructureResource);
        }
    }
}
