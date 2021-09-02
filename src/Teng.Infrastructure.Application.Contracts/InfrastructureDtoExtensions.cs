using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Teng.Infrastructure
{
    public static class InfrastructureDtoExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can add extension properties to DTOs
                 * defined in the depended modules.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */

                ObjectExtensionManager.Instance
                    .AddOrUpdateProperty<string>(new[]
                    {
                        typeof(IdentityUserDto),
                        typeof(IdentityUserUpdateDto),
                        typeof(IdentityUserCreateDto)
                    }, "HeadPortrait", x =>
                     {
                         x.Attributes.Add(new StringLengthAttribute(256));
                     });
            });
        }
    }
}