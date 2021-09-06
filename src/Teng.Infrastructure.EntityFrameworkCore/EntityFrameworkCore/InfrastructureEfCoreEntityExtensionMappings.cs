﻿using Microsoft.EntityFrameworkCore;
using Teng.Infrastructure.Users;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Teng.Infrastructure.EntityFrameworkCore
{
    public static class InfrastructureEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            InfrastructureGlobalFeatureConfigurator.Configure();
            InfrastructureModuleExtensionConfigurator.Configure();

            OneTimeRunner.Run(() =>
            {
                /* You can configure extra properties for the
                 * entities defined in the modules used by your application.
                 *
                 * This class can be used to map these extra properties to table fields in the database.
                 *
                 * USE THIS CLASS ONLY TO CONFIGURE EF CORE RELATED MAPPING.
                 * USE InfrastructureModuleExtensionConfigurator CLASS (in the Domain.Shared project)
                 * FOR A HIGH LEVEL API TO DEFINE EXTRA PROPERTIES TO ENTITIES OF THE USED MODULES
                 *
                 * Example: Map a property to a table field:

                     ObjectExtensionManager.Instance
                         .MapEfCoreProperty<IdentityUser, string>(
                             "MyProperty",
                             (entityBuilder, propertyBuilder) =>
                             {
                                 propertyBuilder.HasMaxLength(128);
                             }
                         );

                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */

                ObjectExtensionManager.Instance
                    .MapEfCoreProperty<IdentityUser, string>(
                        nameof(AppUser.HeadPortrait),
                        ((builder, propertyBuilder) =>
                        {
                            propertyBuilder.HasMaxLength(256);
                        }));

                ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.Avatar),
                     (builder, property) =>
                     {
                         property.HasMaxLength(256);
                     });

                ObjectExtensionManager.Instance
                    .MapEfCoreProperty<IdentityUser, string>(
                        nameof(AppUser.Introduction),
                        (builder, property) =>
                        {
                            property.HasMaxLength(256);
                        });
            });
        }
    }
}