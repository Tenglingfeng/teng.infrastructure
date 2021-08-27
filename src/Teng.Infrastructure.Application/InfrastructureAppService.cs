using System;
using System.Collections.Generic;
using System.Text;
using Teng.Infrastructure.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

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
