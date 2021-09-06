using System;
using System.Collections.Generic;
using System.Text;
using Teng.Infrastructure.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Teng.Infrastructure.Features
{
    public class InfrastructureFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.AddGroup(InfrastructureFeatures.GroupName);
            group.AddFeature(InfrastructureFeatures.SocialLogins, "true", L("Feature: SocialLogins")
                , valueType: new ToggleStringValueType());

            group.AddFeature(InfrastructureFeatures.UserCount, "10", L("Feature:UserCount")
                , valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<InfrastructureResource>(name);
        }
    }
}