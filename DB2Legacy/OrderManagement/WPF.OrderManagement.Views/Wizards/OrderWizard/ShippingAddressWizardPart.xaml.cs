using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared.Properties;
using BOS.ShippingAddressDataEntity;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using WPF.OrderManagement.Shared;
using WPF.ShippingAddress;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for ShippingAddressWizardPart.xaml
    /// </summary>
    public partial class ShippingAddressWizardPart : AB_WizardPartBase
    {
        //// Due to a bug in the CLR this type will not resolve in the XAML at runtime unless we make a reference to it in the code behind
        private A4DN.Core.WPF.Base.BoolToVisibilityConverter coverter;
        private OrderWizardObject _sharedObject;
        private ShippingAddressDetail _ShippingAddressDetail;
        private ShippingAddressDetail _ShippingAddressDetailReadOnly;
        public const string Step_ShippingAddress = "WPF.Wizards.OrderWizard.ShippingAddressWizardPart";

        public ShippingAddressWizardPart() : this(null)
        {

        }

        public ShippingAddressWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _sharedObject = (OrderWizardObject)ap_SharedWizardObject;
            _ShippingAddressDetail = new ShippingAddressDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(5), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _ShippingAddressDetailReadOnly = new ShippingAddressDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(5), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _ShippingAddressDetailReadOnly.ShippingAddressDetailLayout.SetBinding(DataContextProperty, new Binding("ap_CurrentSelectedEntity")
            {
                Source = ddShipAddress,
                Mode = BindingMode.OneWay
            });

            addShipAddress.Content = _ShippingAddressDetail.ShippingAddressDetailLayout;
            readOnlyDetail.Content = _ShippingAddressDetailReadOnly.ShippingAddressDetailLayout;

            layoutRoot.DataContext = _sharedObject;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_ShippingAddress, DescriptionResource.SHIPPINGADDRESS, DescriptionResource.ENTERSHIPPINGADDRESSINFORMATION, "ShippingAddress.png"));
        }

        public override void am_BeforeStepShown(AB_WizardStep Step)
        {
            base.am_BeforeStepShown(Step);

            if (!_sharedObject.IsSpecifyingExistingShippingAddress)
            {
                rbNewShipAddress.IsChecked = true;

                return;
            }
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _ShippingAddressDetail.am_InitializeDetailForNewModeButDoNotShow();
        }

        private void rbSpecifyExisting_Checked(object sender, RoutedEventArgs e)
        {
            ap_MessageConsole.am_ClearMessages();
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if ((bool)rbSpecifyExisting.IsChecked)
            {
                if (ddShipAddress.ap_CurrentSelectedEntity == null)
                {
                    ap_MessageConsole.am_AddMessage(new AB_Message(DescriptionResource.PLEASEPICKSHIPPINGADDRESS), false);
                    Args.Cancel = true;

                    return;
                }

                _sharedObject.CurrentShippingAddressEntity = (ShippingAddressEntity)ddShipAddress.ap_CurrentSelectedEntity;
            }
            else
            {
                if (!((ShippingAddressEntity)_ShippingAddressDetail.ap_CurrentEntity).am_ValidateEntity(out ObservableCollection<AB_Message> messages))
                {
                    ap_MessageConsole.am_AddMessages(messages, false);
                    Args.Cancel = true;

                    return;
                }

                _sharedObject.CurrentShippingAddressEntity = (ShippingAddressEntity)_ShippingAddressDetail.ap_CurrentEntity;
            }

            _sharedObject.IsSpecifyingExistingShippingAddress = (bool)rbSpecifyExisting.IsChecked;

            WizardSummaryController.ShippingAddressSummaryItem.Header = DescriptionResource.SHIPPINGADDRESS;
            WizardSummaryController.ShippingAddressSummaryItem.Info = _sharedObject.CurrentShippingAddressEntity.ShippingAddressLine;

            am_MarkCurrentStepComplete();
        }
    }
}
