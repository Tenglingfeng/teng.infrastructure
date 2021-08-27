using Volo.Abp.Settings;

namespace Teng.Infrastructure.Settings
{
    public class InfrastructureSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(InfrastructureSettings.MySetting1));
        }
    }
}
