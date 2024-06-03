using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderManagement.Wizard.Shared;
using WPF.OrderManagement.Wizards;

namespace WPF.OrderManagement.Wizard
{
    public class WizardWindow : AB_WizardBase
    {
        protected override void am_SetParentProperties()
        {
            base.am_SetParentProperties();

            Width = 1005;
            Title = DescriptionResource.WIZARDSPLASH;
            ap_SplashWindowType = typeof(WizardSplash);
            ap_TreeViewIconsStyle = AB_TreeViewIconsStyles.SmallIcons;
            ap_SharedObjectType = typeof(WizardSharedObject);
        }

        protected override void am_EnrollWizardParts()
        {
            base.am_EnrollWizardParts();

            am_AddWizardPart(typeof(CustomerWizardPart));
            am_AddWizardPart(typeof(OrderItemWizardPart));
        }
    }
}
