using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerDataEntity;
using BOS.OrderManagement.Shared;
using BOS.OrderManagement.Shared.Properties;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPF.Customer;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for CustomerWizardPart.xaml
    /// </summary>
    public partial class CustomerWizardPart : AB_WizardPartBase
    {
        public const string Step_Customer = "Customer";

        private OrderWizardSharedObject _SharedObject => ap_SharedWizardObject as OrderWizardSharedObject;
        private CustomerDetail _CustomerDetail;
        private CustomerDetail _CustomerDetailReadOnly;

        public CustomerWizardPart() : this(null)
        {

        }

        public CustomerWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _CustomerDetail = new CustomerDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(Constants.MODULE_Customer), ap_MessageConsole, true));
            _CustomerDetailReadOnly = new CustomerDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(Constants.MODULE_Customer), ap_MessageConsole, true));
            _CustomerDetailReadOnly.CustomerDetailWizardPanel.SetBinding(DataContextProperty, new Binding("ap_CurrentSelectedEntity")
            {
                Source = ddCustomer,
                Mode = BindingMode.OneWay
            });

            addCustomer.Content = _CustomerDetail.CustomerDetailWizardPanel;
            readOnlyDetail.Content = _CustomerDetailReadOnly.CustomerDetailWizardPanel;

            _SharedObject.NewOrderStarted += OnNewOrderStarted;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Customer, DescriptionResource.CUSTOMER, DescriptionResource.ENTERCUSTOMERINFORMATION, "Customer_small.png"));
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _CustomerDetail.am_InitializeDetailForNewModeButDoNotShow();
        }

        public override void am_SetWizardButtonStates()
        {
            ap_BtnNextVisibility = Visibility.Visible;
            ap_BtnBackVisibility = Visibility.Visible;
            ap_BtnResetVisibility = Visibility.Collapsed;
            ap_BtnSkipVisibility = Visibility.Collapsed;
            ap_BtnFinishVisibility = Visibility.Collapsed;
        }

        public override void am_BeforeStepShown(AB_WizardStep Step)
        {
            am_MarkStepIncomplete(Step_Customer);

            if (_SharedObject.Customer == null && _SharedObject.OrdersCreated > 0)
            {
                ddCustomer.CustomerInternalID = _SharedObject.Customer?.CustomerInternalID;
                SetExistingDetailToReadOnly();
            }
            else if (_SharedObject.Customer != null && _SharedObject.Customer.CustomerInternalID == null)
            {
                var customer = _SharedObject.Customer;

                _CustomerDetail.Expander_OrdersInformation.IsExpanded = false;

                _CustomerDetail.Field_Name.ap_Value = customer.Name;
                _CustomerDetail.Field_LegalName.ap_Value= customer.LegalName;
                _CustomerDetail.Field_ContactFirstName.ap_Value = customer.ContactFirstName;
                _CustomerDetail.Field_ContactMiddleName.ap_Value = customer.ContactMiddleName;
                _CustomerDetail.Field_ContactLastName.ap_Value = customer.ContactLastName;
                _CustomerDetail.Field_ContactNickName.ap_Value = customer.ContactNickName;
                _CustomerDetail.Field_BillingAddress1.ap_Value = customer.BillingAddress1;
                _CustomerDetail.Field_BillingAddress2.ap_Value = customer.BillingAddress2;
                _CustomerDetail.Field_BillingAddress3.ap_Value = customer.BillingAddress3;
                _CustomerDetail.Field_BillingPostalCode.ap_Value = customer.BillingPostalCode;
                _CustomerDetail.Field_BillingCountry.ap_Value = customer.BillingCountry;
                _CustomerDetail.Field_Telephone.ap_Value = customer.Telephone;
                _CustomerDetail.Field_Email.ap_Value = customer.Email;
                _CustomerDetail.Field_Memo.ap_Value = customer.Memo;

                if (customer.PurchasePoints.HasValue)
                {
                    _CustomerDetail.Field_PurchasePoints.ap_Value = customer.PurchasePoints.ToString();
                }

                if (customer.ParentInternalID.HasValue)
                {
                    _CustomerDetail.Field_ParentInternalID.ap_Value = customer.ParentInternalID.ToString();
                    _CustomerDetail.Field_ParentRelationship.ap_Value = customer.ParentRelationship;
                }
            }
        }

        private void rbNewShipAddress_Checked(object sender, RoutedEventArgs e)
        {
            ddCustomer.am_ClearDropDown();
            _SharedObject.ap_WizardMessageConsole.am_ClearMessages();

            _CustomerDetail.Expander_OrdersInformation.IsExpanded = false;
        }

        private void rbSpecifyExisting_Checked(object sender, RoutedEventArgs e)
        {
            _SharedObject.ap_WizardMessageConsole.am_ClearMessages();

            SetExistingDetailToReadOnly();
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if ((bool)rbSpecifyExisting.IsChecked)
            {
                if (ddCustomer.ap_CurrentSelectedEntity == null)
                {
                    _SharedObject.ap_WizardMessageConsole.am_AddMessage(new AB_Message(DescriptionResource.PLEASEPICKCUSTOMER), true, true);
                    Args.Cancel = true;
                    return;
                }

                _SharedObject.Customer = (CustomerEntity)ddCustomer.ap_CurrentSelectedEntity;
            }
            else
            {
                if (!((CustomerEntity)_CustomerDetail.ap_CurrentEntity).am_ValidateEntity(out ObservableCollection<AB_Message> messages))
                {
                    _SharedObject.ap_WizardMessageConsole.am_AddMessages(messages, true, true);
                    Args.Cancel = true;
                    return;
                }

                _SharedObject.Customer = (CustomerEntity)_CustomerDetail.ap_CurrentEntity;
            }

            am_MarkCurrentStepComplete();
        }

        private void OnNewOrderStarted(object sender, EventArgs e)
        {
            am_MarkStepIncomplete(Step_Customer);
        }

        private void SetExistingDetailToReadOnly()
        {
            if (_CustomerDetailReadOnly != null)
            {
                _CustomerDetailReadOnly.Field_Name.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_LegalName.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_ParentInternalID.IsReadOnly = true;
                _CustomerDetailReadOnly.dd_ParentInternalID.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_ParentRelationship.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_ContactFirstName.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_ContactMiddleName.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_ContactLastName.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_ContactNickName.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_BillingAddress1.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_BillingAddress2.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_BillingAddress3.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_BillingPostalCode.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_BillingCountry.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_Telephone.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_Email.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_Memo.IsReadOnly = true;
                _CustomerDetailReadOnly.Field_PurchasePoints.IsReadOnly = true;

                foreach(var child in LogicalTreeHelper.GetChildren(_CustomerDetailReadOnly.Expander_OrdersInformation))
                {
                    if (child is Grid grid)
                    {
                        foreach(var gridElement in grid.Children)
                        {
                            if (gridElement is StackPanel stackPanel)
                            {
                                foreach (var element in stackPanel.Children)
                                {
                                    if (element is AB_FieldWithLabel field)
                                    {
                                        field.IsReadOnly = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
