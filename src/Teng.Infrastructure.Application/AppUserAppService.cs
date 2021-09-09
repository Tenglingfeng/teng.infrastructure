using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Teng.Infrastructure.Users;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Users;

namespace Teng.Infrastructure
{
    public class AppUserAppService : IdentityUserAppService, IAppUserAppService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IdentitySecurityLogManager _identitySecurityLogManager;

        public AppUserAppService(IdentityUserManager userManager, 
            IIdentityUserRepository userRepository, 
            IIdentityRoleRepository roleRepository,
            SignInManager<AppUser> signInManager, 
            IdentitySecurityLogManager identitySecurityLogManager) : base(userManager, userRepository, roleRepository)
        {
            _signInManager = signInManager;
            _identitySecurityLogManager = identitySecurityLogManager;
        }

        public async Task<AppUserDto> Login(string userName, string passWord)
        {
            Check.NotNullOrWhiteSpace(userName, nameof(userName));
            Check.NotNullOrWhiteSpace(passWord, nameof(passWord));

            var signInResult = await _signInManager.PasswordSignInAsync(userName, passWord, true, true);

            await _identitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = signInResult.ToIdentitySecurityLogAction(),
                UserName = userName
            });

            return new AppUserDto();

            //var user = await UserManager.FindByNameAsync(userName);

            //var b = await UserManager.CheckPasswordAsync(user, passWord);

            //if (b)
            //{
            //    return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, AppUserDto>(user);
            //}
            //throw new UserFriendlyException("用户名或密码错误");
        }
    }
}