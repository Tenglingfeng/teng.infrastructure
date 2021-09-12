using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Teng.Infrastructure.Users;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Teng.Infrastructure
{
    public class AppUserAppService : IdentityUserAppService, IAppUserAppService
    {
        private readonly IdentitySecurityLogManager _identitySecurityLogManager;

        private readonly IOptions<TokenClientOptions> _clientOptions;

        public AppUserAppService(IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IdentitySecurityLogManager identitySecurityLogManager, IOptions<TokenClientOptions> clientOptions) : base(userManager, userRepository, roleRepository)
        {
            _identitySecurityLogManager = identitySecurityLogManager;
            _clientOptions = clientOptions;
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
                throw new UserFriendlyException("认证中心异常");
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