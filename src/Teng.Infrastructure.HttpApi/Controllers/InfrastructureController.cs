using Teng.Infrastructure.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Teng.Infrastructure.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class InfrastructureController : AbpController
    {
        protected InfrastructureController()
        {
            LocalizationResource = typeof(InfrastructureResource);
        }
    }
}