using Teng.Infrastructure.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Teng.Infrastructure.Permissions
{
    public class InfrastructurePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(InfrastructurePermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(InfrastructurePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<InfrastructureResource>(name);
        }
    }
}
