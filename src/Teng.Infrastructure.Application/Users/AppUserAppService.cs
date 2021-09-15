using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Teng.Infrastructure.ApplicationExtension;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Teng.Infrastructure.Users
{
    public class AppUserAppService : IdentityUserAppService, IAppUserAppService
    {
        private readonly IdentitySecurityLogManager _identitySecurityLogManager;

        private readonly IAppUserRepository _appUserRepository;

        private readonly IOptions<TokenClientOptions> _clientOptions;

        public AppUserAppService(IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IdentitySecurityLogManager identitySecurityLogManager, IOptions<TokenClientOptions> clientOptions, IAppUserRepository appUserRepository) : base(userManager, userRepository, roleRepository)
        {
            _identitySecurityLogManager = identitySecurityLogManager;
            _clientOptions = clientOptions;
            _appUserRepository = appUserRepository;
        }

        /// <summary>
        /// 用户创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateAsync(UserCreateDto input)
        {
            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

            input.MapExtraPropertiesTo(user);
            var identityResult = (await UserManager.CreateAsync(user, input.Password));
            identityResult.CheckErrors();

            await UpdateUserByInput(user, input);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AppUserDto>> GetListAsync(GetUserPagedAndSortedResultInput input)
        {
            var count = await _appUserRepository.GetCountAsync(input.Filter);
            var entities =
                await _appUserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                    input.Filter);

            var dtos = entities.Select(x => x.ToDto<AppUser, AppUserDto>()).ToList();

            return new PagedResultDto<AppUserDto>(count, dtos);
        }

        public async Task<LoginResultDto> Login(LoginInputDto input)
        {
            Check.NotNullOrWhiteSpace(input.UserName, nameof(input.UserName));
            Check.NotNullOrWhiteSpace(input.PassWord, nameof(input.PassWord));

            await _identitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = nameof(Login),
                UserName = input.UserName
            });

            var user = await UserManager.FindByNameAsync(input.UserName);

            var b = await UserManager.CheckPasswordAsync(user, input.PassWord);
            if (!b)
            {
                throw new UserFriendlyException("用户名或密码错误");
            }

            var client = new HttpClient();
            var doc = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _clientOptions.Value.Address
            });

            if (doc.IsError)
            {
                throw new UserFriendlyException("系统错误");
            }

            var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = doc.TokenEndpoint,
                ClientId = _clientOptions.Value.ClientId,
                ClientSecret = _clientOptions.Value.ClientSecret,
                UserName = input.UserName,
                Password = input.PassWord,
                GrantType = OidcConstants.GrantTypes.Password
            });

            if (token.IsError)
            {
                throw new UserFriendlyException("认证失败" + token.HttpErrorReason);
            }
            return new LoginResultDto()
            {
                AccessToken = token.AccessToken,
                IdentityToken = token.IdentityToken,
                RefreshToken = token.RefreshToken,
                TokenType = token.TokenType,
                ExpiresIn = token.ExpiresIn,
                ErrorDescription = token.ErrorDescription
            };
        }
    }
}