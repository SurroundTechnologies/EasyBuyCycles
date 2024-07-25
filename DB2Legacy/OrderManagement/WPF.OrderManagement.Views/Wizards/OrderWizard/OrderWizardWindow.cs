using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderManagement.Shared.Properties;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace WPF.Wizards.OrderWizard
{
    public class OrderWizardWindow : AB_WizardBase
    {
        protected override void am_EnrollWizardParts()
        {
            am_AddWizardPart(typeof(StartWizardPart));
            am_AddWizardPart(typeof(CustomerWizardPart));
            am_AddWizardPart(typeof(OrderItemWizardPart));
            am_AddWizardPart(typeof(ShippingAddressWizardPart));
            am_AddWizardPart(typeof(OrderWizardPart));
            am_AddWizardPart(typeof(CompletionWizardPart));

            SetupSummaryBlocks();
        }

        private void SetupSummaryBlocks()
        {
            var sharedWizardObject = ap_SharedWizardObject as OrderWizardSharedObject;

            ap_SummaryStackPanel.Children.Add(sharedWizardObject.CurrentOrderSummaryHeaderTextBlock);
            ap_SummaryStackPanel.Children.Add(sharedWizardObject.CustomerSummaryItemTextBlock);
            ap_SummaryStackPanel.Children.Add(sharedWizardObject.OrderItemsSummaryItemTextBlock);
            ap_SummaryStackPanel.Children.Add(sharedWizardObject.ShippingAddressSummaryItemTextBlock);
            ap_SummaryStackPanel.Children.Add(sharedWizardObject.WizardSessionSummaryHeaderTextBlock);
            ap_SummaryStackPanel.Children.Add(sharedWizardObject.WizardSessionSummaryContentsTextBlock);
        }

        protected override void am_SetParentProperties()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.CanResize;
            Width = 1200;
            Height = 1040;
            Title = DescriptionResource.ORDERENTRYWIZARD;
            Icon = getWizardIcon();

            base.am_SetParentProperties();

            ap_SharedObjectType = typeof(OrderWizardSharedObject);
            //ap_SplashWindowType = typeof(OrderWizardSplash);
            ap_TreeViewIconsStyle = AB_TreeViewIconsStyles.LargeIcons;
            ap_ShowNavigator = true;
            ap_UseTransitions = false;
            ap_MessageConsoleVisibility = Visibility.Visible;
        }

        BitmapImage getWizardIcon()
        {
			BitmapImage icon = new BitmapImage();
			icon.BeginInit();
			icon.UriSource = new Uri("pack://application:,,,/A4DN.Core.WPF.Base;component/ImagesLineFlat2020/InternationalPackageMagicWand_large.png");
			icon.EndInit();
            return icon;
		}

        public override AB_WizardStep am_GetNextWizardStep(AB_WizardStep step)
        {
            switch (step.ap_StepID)
            {
                case CustomerWizardPart.Step_Customer:
                    return ap_RootSteps.First(x => x.ap_StepID == OrderItemWizardPart.Step_OrderItem);

                case OrderItemWizardPart.Step_OrderItem:
                    return ap_RootSteps.First(x => x.ap_StepID == ShippingAddressWizardPart.Step_ShippingAddress);

                case ShippingAddressWizardPart.Step_ShippingAddress:
                    return ap_RootSteps.First(x => x.ap_StepID == OrderWizardPart.Step_Order);

                case OrderWizardPart.Step_Order:
                    return ap_RootSteps.First(x => x.ap_StepID == CompletionWizardPart.Step_Completion);

                case CompletionWizardPart.Step_Completion:
                    return ap_RootSteps.First(x => x.ap_StepID == CustomerWizardPart.Step_Customer);

                default:
                    return base.am_GetNextWizardStep(step);
            }
        }

        public override AB_WizardStep am_GetPreviousWizardStep(AB_WizardStep step)
        {
            switch (step.ap_StepID)
            {
                case CustomerWizardPart.Step_Customer:
                    return ap_RootSteps.First(x => x.ap_StepID == StartWizardPart.Step_Start);

                case OrderItemWizardPart.Step_OrderItem:
                    return ap_RootSteps.First(x => x.ap_StepID == CustomerWizardPart.Step_Customer);

                case ShippingAddressWizardPart.Step_ShippingAddress:
                    return ap_RootSteps.First(x => x.ap_StepID == OrderItemWizardPart.Step_OrderItem);

                case OrderWizardPart.Step_Order:
                    return ap_RootSteps.First(x => x.ap_StepID == ShippingAddressWizardPart.Step_ShippingAddress);

                default:
                    return base.am_GetNextWizardStep(step);
            }
        }

        protected override void am_ActivateStep(AB_WizardStep StepToActivate)
        {
            base.am_ActivateStep(StepToActivate);

            var sharedWizardObject = ap_SharedWizardObject as OrderWizardSharedObject;

            switch (StepToActivate.ap_StepID)
            {
                case CustomerWizardPart.Step_Customer:

                    if (string.IsNullOrEmpty(sharedWizardObject.CurrentOrderSummaryHeaderTextBlock.Text))
                    {
                        sharedWizardObject.CurrentOrderSummaryHeaderTextBlock.Inlines.Add(new Underline(new Run("Current Order Summary\n")));
                    }

                    break;

                case OrderItemWizardPart.Step_OrderItem:
                    var custIdStr = sharedWizardObject.Customer.CustomerInternalID.HasValue ? string.Format("{0} ", sharedWizardObject.Customer.CustomerInternalID) : string.Empty;
                    var customerSummary = $"Customer\n   {custIdStr + sharedWizardObject.Customer.Name}\n   {sharedWizardObject.Customer.ContactFullName}\n   {sharedWizardObject.Customer.Telephone}\n   {sharedWizardObject.Customer.BillingAddressLine}\n";
                    sharedWizardObject.CustomerSummaryItemTextBlock.Text = customerSummary;

                    break;

                case ShippingAddressWizardPart.Step_ShippingAddress:
                    var orderDiscountTotal = sharedWizardObject.OrderItems.Sum(x => x.OrderTotal);
                    var orderTotal = sharedWizardObject.OrderItems.Sum(x => x.OrderTotal + x.OrderDiscount);
                    var orderItemSummary = $"Items\n   Count: {sharedWizardObject.OrderItems.Count}\n   Total: {orderTotal.Value:C}\n   Discounted Total: {orderDiscountTotal.Value:C}\n";
                    sharedWizardObject.OrderItemsSummaryItemTextBlock.Text = orderItemSummary;
                    
                    break;

                case OrderWizardPart.Step_Order:

                    var shippingAddressSummary = $"Shipping Addrress\n   {sharedWizardObject.ShippingAddress.Name}\n   {sharedWizardObject.ShippingAddress.ContactFullName}\n   {sharedWizardObject.ShippingAddress.Telephone}\n   {sharedWizardObject.ShippingAddress.ShippingAddressLine.Trim()}\n";
                    sharedWizardObject.ShippingAddressSummaryItemTextBlock.Text = shippingAddressSummary;

                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // FIXME: When running the wizard standalone, it crashes when attempting to close.
            // It is expecting type of Browser for some reason.
            base.OnClosing(e);
            ap_SharedWizardObject = null;
        }
    }
}
