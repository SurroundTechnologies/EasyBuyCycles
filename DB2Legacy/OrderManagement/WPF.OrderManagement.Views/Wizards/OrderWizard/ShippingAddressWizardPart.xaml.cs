using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderManagement.Shared;
using BOS.OrderManagement.Shared.Properties;
using BOS.ShippingAddressDataEntity;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using WPF.ShippingAddress;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for ShippingAddressWizardPart.xaml
    /// </summary>
    public partial class ShippingAddressWizardPart : AB_WizardPartBase
    {
        public const string Step_ShippingAddress = "ShippingAddress";

        private OrderWizardSharedObject _SharedObject => ap_SharedWizardObject as OrderWizardSharedObject;
        private ShippingAddressDetail _ShippingAddressDetail;
        private ShippingAddressDetail _ShippingAddressDetailReadOnly;

        public ShippingAddressWizardPart() : this(null)
        {

        }

        public ShippingAddressWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _ShippingAddressDetail = new ShippingAddressDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(Constants.MODULE_ShippingAddress), ap_MessageConsole, true));
            _ShippingAddressDetailReadOnly = new ShippingAddressDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(Constants.MODULE_Customer), ap_MessageConsole, true));
            _ShippingAddressDetailReadOnly.ShippingAddressDetailWizardPanel.SetBinding(DataContextProperty, new Binding("ap_CurrentSelectedEntity")
            {
                Source = ddShipAddress,
                Mode = BindingMode.OneWay
            });

            addShipAddress.Content = _ShippingAddressDetail.ShippingAddressDetailWizardPanel;
            readOnlyDetail.Content = _ShippingAddressDetailReadOnly.ShippingAddressDetailWizardPanel;

            layoutRoot.DataContext = _SharedObject;

            _SharedObject.NewOrderStarted += OnNewOrderStarted;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_ShippingAddress, DescriptionResource.SHIPPING, DescriptionResource.ENTERSHIPPINGADDRESSINFORMATION, "ShippingAddress.png"));
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _ShippingAddressDetail.am_InitializeDetailForNewModeButDoNotShow();
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
            am_MarkStepIncomplete(Step_ShippingAddress);

            if (_SharedObject.ShippingAddress == null)
            {
                ddShipAddress.CustomerInternalID = _SharedObject.Customer.CustomerInternalID ?? 0;

                if (ddShipAddress.CustomerInternalID > 0)
                {
                    rbNewShipAddress.IsChecked = false;
                    rbSpecifyExisting.IsChecked = true;

                    SetExistingDetailToReadOnly();

                    _ShippingAddressDetail.Field_CustomerInternalID.Visibility = Visibility.Visible;
                    Field_CustomerName.ap_Value = string.Empty;
                    Field_CustomerName.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rbNewShipAddress.IsChecked = true;
                    rbSpecifyExisting.IsChecked = false;

                    ddShipAddress.am_ClearDropDown();
                    _ShippingAddressDetail.Field_CustomerInternalID.IsReadOnly = true;
                    _ShippingAddressDetail.Field_CustomerInternalID.CustomerInternalID = _SharedObject.Customer.CustomerInternalID;
                    _ShippingAddressDetail.Field_CustomerInternalID.Visibility = Visibility.Collapsed;
                    Field_CustomerName.ap_Value = _SharedObject.Customer.Name;
                    Field_CustomerName.Visibility = Visibility.Visible;
                }
            }
            else if (_SharedObject.ShippingAddress != null && _SharedObject.ShippingAddress.ShippingAddressInternalID == null)
            {
                var shippingAddress = _SharedObject.ShippingAddress;

                _ShippingAddressDetail.Field_Name.ap_Value = shippingAddress.Name;
                _ShippingAddressDetail.Field_ContactFirstName.ap_Value = shippingAddress.ContactFirstName;
                _ShippingAddressDetail.Field_ContactMiddleName.ap_Value = shippingAddress.ContactMiddleName;
                _ShippingAddressDetail.Field_ContactLastName.ap_Value = shippingAddress.ContactLastName;
                _ShippingAddressDetail.Field_ContactNickName.ap_Value = shippingAddress.ContactNickName;
                _ShippingAddressDetail.Field_Address1.ap_Value = shippingAddress.Address1;
                _ShippingAddressDetail.Field_Address2.ap_Value = shippingAddress.Address2;
                _ShippingAddressDetail.Field_Address3.ap_Value = shippingAddress.Address3;
                _ShippingAddressDetail.Field_PostalCode.ap_Value = shippingAddress.PostalCode;
                _ShippingAddressDetail.Field_Country.ap_Value = shippingAddress.Country;
                _ShippingAddressDetail.Field_Telephone.ap_Value = shippingAddress.Telephone;
                _ShippingAddressDetail.Field_Email.ap_Value = shippingAddress.Email;
                _ShippingAddressDetail.Field_Memo.ap_Value = shippingAddress.Memo;

                if (shippingAddress.PurchasePoints.HasValue)
                {
                    _ShippingAddressDetail.Field_PurchasePoints.ap_Value = shippingAddress.PurchasePoints.ToString();
                }
            }
        }

        private void rbNewShipAddress_Checked(object sender, RoutedEventArgs e)
        {
            ddShipAddress.am_ClearDropDown();
            _ShippingAddressDetail.Field_CustomerInternalID.IsReadOnly = true;
            _ShippingAddressDetail.Field_CustomerInternalID.CustomerInternalID = _SharedObject.Customer.CustomerInternalID;
            _ShippingAddressDetail.Field_CustomerInternalID.Visibility = Visibility.Collapsed;
            Field_CustomerName.ap_Value = _SharedObject.Customer.Name;
            Field_CustomerName.Visibility = Visibility.Visible;
            ap_MessageConsole.am_ClearMessages();
        }

        private void rbSpecifyExisting_Checked(object sender, RoutedEventArgs e)
        {
            ap_MessageConsole.am_ClearMessages();

            _ShippingAddressDetail.Field_CustomerInternalID.Visibility = Visibility.Visible;
            Field_CustomerName.ap_Value = string.Empty;
            Field_CustomerName.Visibility = Visibility.Collapsed;

            SetExistingDetailToReadOnly();
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if ((bool)rbSpecifyExisting.IsChecked)
            {
                if (ddShipAddress.ap_CurrentSelectedEntity == null)
                {
                    ap_MessageConsole.am_AddMessage(new AB_Message(DescriptionResource.PLEASEPICKSHIPPINGADDRESS), true, true);
                    Args.Cancel = true;

                    return;
                }

                _SharedObject.ShippingAddress = ddShipAddress.ap_CurrentSelectedEntity as ShippingAddressEntity;
            }
            else
            {
                var shippingAddress = (ShippingAddressEntity)_ShippingAddressDetail.ap_CurrentEntity;
                shippingAddress.CustomerName = _SharedObject.Customer.Name;

                if (!shippingAddress.am_ValidateEntity(out ObservableCollection<AB_Message> messages))
                {
                    ap_MessageConsole.am_AddMessages(messages, true, true);
                    Args.Cancel = true;

                    return;
                }

                _SharedObject.ShippingAddress = _ShippingAddressDetail.ap_CurrentEntity as ShippingAddressEntity;
            }

            am_MarkCurrentStepComplete();
        }

        private void OnNewOrderStarted(object sender, EventArgs e)
        {
            am_MarkStepIncomplete(Step_ShippingAddress);
        }

        private void SetExistingDetailToReadOnly()
        {
            if (_ShippingAddressDetailReadOnly != null)
            {
                _ShippingAddressDetailReadOnly.Field_CustomerInternalID.CustomerInternalID = _SharedObject.Customer.CustomerInternalID;
                _ShippingAddressDetailReadOnly.Field_Name.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_ContactFirstName.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_ContactMiddleName.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_ContactLastName.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_ContactNickName.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Address1.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Address2.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Address3.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_PostalCode.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Country.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Telephone.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Email.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_Memo.IsReadOnly = true;
                _ShippingAddressDetailReadOnly.Field_PurchasePoints.IsReadOnly = true;
            }
        }
    }
}
