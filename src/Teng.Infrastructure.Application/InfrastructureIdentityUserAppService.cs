using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Teng.Infrastructure.Features;
using Teng.Infrastructure.Localization;
using Teng.Infrastructure.Users;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Teng.Infrastructure
{
    [RemoteService(isEnabled: false)]
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IdentityUserAppService), typeof(IIdentityUserAppService))]
    public class InfrastructureIdentityUserAppService : IdentityUserAppService, IInfrastructureIdentityUserAppService
    {
        private readonly StringLocalizer<InfrastructureResource> _stringLocalizer;

        public InfrastructureIdentityUserAppService(IdentityUserManager userManager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, StringLocalizer<InfrastructureResource> stringLocalizer) : base(userManager, userRepository, roleRepository)
        {
            _stringLocalizer = stringLocalizer;
        }

        public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var userCount = (await FeatureChecker.GetOrNullAsync(InfrastructureFeatures.UserCount)).To<int>();
            var currentUserCount = await UserRepository.GetCountAsync();
            if (currentUserCount >= userCount)
            {
                throw new UserFriendlyException(_stringLocalizer["Feature:UserCount.Maximum", userCount]);
            }
            return await base.CreateAsync(input);
        }
    }
}