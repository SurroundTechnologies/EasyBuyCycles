using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkBusinessProcess;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerDataEntity;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderManagement.Wizard.Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using WPF.Customer;
using WPF.OrderManagement.Shared;

namespace WPF.OrderManagement.Wizards
{
    /// <summary>
    /// Interaction logic for CustomerWizardPart.xaml
    /// </summary>
    public partial class CustomerWizardPart : AB_WizardPartBase
    {
        //// Due to a bug in the CLR this type will not resolve in the XAML at runtime unless we make a reference to it in the code behind
        private Shared.BoolToVisibilityConverter coverter;
        private WizardSharedObject _sharedObject;
        private CustomerDetail _CustomerDetail;
        private CustomerDetail _CustomerDetailReadOnly;
        public const string Step_Customer = "WPF.OrderManagement.Wizards.CustomerWizardPart";

        public CustomerWizardPart() : this(null)
        {

        }

        public CustomerWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _sharedObject = (WizardSharedObject)ap_SharedWizardObject;
            _CustomerDetail = new CustomerDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(1), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _CustomerDetailReadOnly = new CustomerDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(1), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _CustomerDetailReadOnly.WizardMainInfoPanel.SetBinding(DataContextProperty, new Binding("ap_CurrentSelectedEntity")
            {
                Source = ddCustomer,
                Mode = BindingMode.OneWay
            });

            addCustomer.Content = _CustomerDetail.WizardMainInfoPanel;
            readOnlyDetail.Content = _CustomerDetailReadOnly.WizardMainInfoPanel;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Customer, DescriptionResource.CUSTOMER, DescriptionResource.ENTERCUSTOMERINFORMATION, "Customers.png"));
        }

        private void rbSpecifyExisting_Checked(object sender, RoutedEventArgs e)
        {
            ap_MessageConsole.am_ClearMessages();
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _CustomerDetail.am_InitializeDetailForNewModeButDoNotShow();
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if ((bool)rbSpecifyExisting.IsChecked)
            {
                if (ddCustomer.ap_CurrentSelectedEntity == null)
                {
                    ap_MessageConsole.am_AddMessage(new AB_Message(DescriptionResource.PLEASEPICKCUSTOMER), false);
                    Args.Cancel = true;
                    return;
                }

                _sharedObject.CurrentCustomerEntity = (CustomerEntity)ddCustomer.ap_CurrentSelectedEntity;
            }
            else
            {
                if (!((CustomerEntity)_CustomerDetail.ap_CurrentEntity).am_ValidateEntity(out ObservableCollection<AB_Message> messages))
                {
                    ap_MessageConsole.am_AddMessages(messages, false);
                    Args.Cancel = true;
                    return;
                }

                _sharedObject.CurrentCustomerEntity = (CustomerEntity)_CustomerDetail.ap_CurrentEntity;
            }

            _sharedObject.IsSpecifyingExistingCustomer = (bool)rbSpecifyExisting.IsChecked;

            WizardSummaryController.CustomerSummaryItem.Header = DescriptionResource.CUSTOMERNAME;
            WizardSummaryController.CustomerSummaryItem.Info = _sharedObject.CurrentCustomerEntity.ap_Title;

            am_MarkCurrentStepComplete();
        }
    }
}
