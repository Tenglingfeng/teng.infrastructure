using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace Teng.Infrastructure.IdentityServer
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IConfiguration _configuration;

        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await CreateApiResourcesAsync();
            await CreateClientsAsync();
        }

        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[]
            {
                "name",
                "role"
            };

            await CreateApiResourceAsync("web.admin", commonApiUserClaims);
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task CreateClientsAsync()
        {
            var commonScopes = new[]
            {
                "openid",
                "profile",
                "role",
                "web.admin"
            };

            var configurationSection = _configuration.GetSection("IdentityServer:Clients");

            //Web Client
            //var webClientId = configurationSection["Infrastructure_Web:ClientId"];
            //if (!webClientId.IsNullOrWhiteSpace())
            //{
            //    var webClientRootUrl = configurationSection["Infrastructure_Web:RootUrl"].EnsureEndsWith('/');

            //    /* Infrastructure_Web client is only needed if you created a tiered
            //     * solution. Otherwise, you can delete this client. */

            //    await CreateClientAsync(
            //        name: webClientId,
            //        scopes: commonScopes,
            //        grantTypes: new[] {"hybrid"},
            //        secret: (configurationSection["Infrastructure_Web:ClientSecret"] ?? "1q2w3e*").Sha256(),
            //        redirectUri: $"{webClientRootUrl}signin-oidc",
            //        postLogoutRedirectUri: $"{webClientRootUrl}signout-callback-oidc",
            //        frontChannelLogoutUri: $"{webClientRootUrl}Account/FrontChannelLogout"
            //    );
            //}

            //Console Test / Angular Client
            var webClientRootUrl = configurationSection["Infrastructure_App:RootUrl"]?.TrimEnd('/');

            await CreateClientAsync(
                name: "client.web.admin",
                scopes: commonScopes,
                grantTypes: new[] { "password", "client_credentials", "authorization_code" },
                secret: "1q2w3e*".Sha256(),
                requireClientSecret: false,
                redirectUri: $"{webClientRootUrl}signin-oidc",
                postLogoutRedirectUri: $"{webClientRootUrl}Account/FrontChannelLogout"
            );

            // Blazor Client
            //var blazorClientId = configurationSection["Infrastructure_Blazor:ClientId"];
            //if (!blazorClientId.IsNullOrWhiteSpace())
            //{
            //    var blazorRootUrl = configurationSection["Infrastructure_Blazor:RootUrl"].TrimEnd('/');

            //    await CreateClientAsync(
            //        name: blazorClientId,
            //        scopes: commonScopes,
            //        grantTypes: new[] { "authorization_code" },
            //        secret: configurationSection["Infrastructure_Blazor:ClientSecret"]?.Sha256(),
            //        requireClientSecret: false,
            //        requirePkce: true,
            //        redirectUri: $"{blazorRootUrl}/authentication/login-callback",
            //        postLogoutRedirectUri: $"{blazorRootUrl}/authentication/logout-callback"
            //    );
            //}
        }

        private async Task<Client> CreateClientAsync(
            string name,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret = null,
            string redirectUri = null,
            string postLogoutRedirectUri = null,
            string frontChannelLogoutUri = null,
            bool requireClientSecret = true,
            bool requirePkce = false,
            IEnumerable<string> permissions = null)
        {
            var client = await _clientRepository.FindByCliendIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(
                        _guidGenerator.Create(),
                        name
                    )
                    {
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = 31536000, //365 days
                        AccessTokenLifetime = 31536000, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false,
                        FrontChannelLogoutUri = frontChannelLogoutUri,
                        RequireClientSecret = true,
                        RequirePkce = requirePkce
                    },
                    autoSave: true
                );
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!secret.IsNullOrEmpty())
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            if (redirectUri != null)
            {
                if (client.FindRedirectUri(redirectUri) == null)
                {
                    client.AddRedirectUri(redirectUri);
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions
                );
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}
