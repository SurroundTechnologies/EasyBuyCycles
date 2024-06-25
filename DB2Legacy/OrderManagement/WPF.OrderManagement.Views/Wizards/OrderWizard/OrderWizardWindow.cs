using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared.Properties;
using System.ComponentModel;
using WPF.OrderManagement.Shared;

namespace WPF.Wizards.OrderWizard
{
    public class OrderWizardWindow : AB_WizardBase
    {
        protected override void am_SetParentProperties()
        {
            base.am_SetParentProperties();

            Width = 1005;
            Title = DescriptionResource.ORDERWIZARDSPLASH;
            //ap_SplashWindowType = typeof(OrderWizardSplash);
            ap_TreeViewIconsStyle = AB_TreeViewIconsStyles.SmallIcons;
            ap_SharedObjectType = typeof(OrderWizardObject);
        }

        protected override void am_EnrollWizardParts()
        {
            base.am_EnrollWizardParts();

            am_AddWizardPart(typeof(StartWizardPart));
            am_AddWizardPart(typeof(CustomerWizardPart));
            am_AddWizardPart(typeof(OrderItemWizardPart));
            am_AddWizardPart(typeof(ShippingAddressWizardPart));
            am_AddWizardPart(typeof(OrderWizardPart));

            InitializeSummaryItems();
        }

        private void InitializeSummaryItems()
        {
            ap_SummaryStackPanel.Children.Add(WizardSummaryController.CustomerSummaryItem);
            ap_SummaryStackPanel.Children.Add(WizardSummaryController.OrderItemSummaryItem);
            ap_SummaryStackPanel.Children.Add(WizardSummaryController.OrderSummaryItem);
            ap_SummaryStackPanel.Children.Add(WizardSummaryController.ShippingAddressSummaryItem);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            ap_SummaryStackPanel.Children.Clear();
        }
    }
}
