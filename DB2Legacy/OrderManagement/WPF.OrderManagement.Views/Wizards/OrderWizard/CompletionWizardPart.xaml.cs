using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderItemDataEntity;
using BOS.OrderManagement.Shared.Properties;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for CompletionWizardPart.xaml
    /// </summary>
    public partial class CompletionWizardPart : AB_WizardPartBase
    {
        public const string Step_Completion = "Completion";

        private OrderWizardSharedObject _SharedObject => ap_SharedWizardObject as OrderWizardSharedObject;

        public CompletionWizardPart() : this(null)
        {

        }

        public CompletionWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _SharedObject.RecordsProcessed += OnRecordsProcessed;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Completion, DescriptionResource.COMPLETION, DescriptionResource.ORDERSUCCESSFULLYCREATED, "ShoppingCartBlueOk_large.png"));
        }

        public override void am_SetWizardButtonStates()
        {
            ap_BtnNextVisibility = Visibility.Visible;
            ap_BtnBackVisibility = Visibility.Collapsed;
            ap_BtnResetVisibility = Visibility.Collapsed;
            ap_BtnSkipVisibility = Visibility.Collapsed;
            ap_BtnFinishVisibility = Visibility.Collapsed;

            ap_BtnNextIsEnabled = false;
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            _SharedObject.ResetPropertiesForNextLoop();

            ap_MessageConsole.am_ClearMessages();

            am_MarkStepIncomplete(Step_Completion);
        }

        public override void am_BeforeStepShown(AB_WizardStep Step)
        {
            am_MarkStepIncomplete(Step_Completion);

            ddOrder.OrderInternalID = _SharedObject.Order?.OrderInternalID;
            Field_CustomerName.ap_Value = _SharedObject.Customer.Name;
            Field_ShippingAddress.ap_Value = _SharedObject.ShippingAddress.Name;
        }

        private void OnRecordsProcessed (object sender, EventArgs e)
        {
            if (_SharedObject.Order != null)
            {
                ddOrder.OrderInternalID = _SharedObject.Order.OrderInternalID;
                am_MarkCurrentStepComplete();
                ap_BtnNextIsEnabled = true;
            }

            if (Field_CustomerName.ap_Value == null)
            {
                Field_CustomerName.ap_Value = _SharedObject.Customer.Name;
            }

            if (Field_CustomerName.ap_Value == null)
            {
                Field_ShippingAddress.ap_Value = _SharedObject.ShippingAddress.Name;
            }
        }
    }
}
