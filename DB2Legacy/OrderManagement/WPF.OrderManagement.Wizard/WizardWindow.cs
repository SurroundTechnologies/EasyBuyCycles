using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderManagement.Wizard.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
