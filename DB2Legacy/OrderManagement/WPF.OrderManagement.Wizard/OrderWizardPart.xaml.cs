using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkBusinessProcess;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderManagement.Wizard.Shared;
using System.Collections.ObjectModel;
using System.Windows.Data;
using WPF.Order;

namespace WPF.OrderManagement.Wizards
{
    /// <summary>
    /// Interaction logic for OrderWizardPart.xaml
    /// </summary>
    public partial class OrderWizardPart : AB_WizardPartBase
    {
        private WizardSharedObject _sharedObject;
        private OrderDetail _OrderDetail;
        public const string Step_Order = "WPF.OrderManagement.Wizards.OrderWizardPart";

        public OrderWizardPart() : this(null)
        {

        }

        public OrderWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _sharedObject = (WizardSharedObject)ap_SharedWizardObject;
            _OrderDetail = new OrderDetail(new AB_DetailInitializationArgs(AB_FrameworkDataBP.am_GetModuleEntity(9), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _OrderDetail.NonWizardControlsVisibility = System.Windows.Visibility.Collapsed;
            _OrderDetail.OrderDetailLayout.SetBinding(DataContextProperty, new Binding("CurrentOrderEntity")
            {
                Source = _sharedObject,
                Mode = BindingMode.OneWay,
            });

            presenter.Content = _OrderDetail.OrderDetailLayout;
            _sharedObject.CurrentOrderEntity = new OrderEntity() { ap_RecordMode = AB_RecordMode.New };
            spProgress.DataContext = _sharedObject;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Order, DescriptionResource.ORDER, DescriptionResource.ENTERORDERINFORMATION, "Order.png"));
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _OrderDetail.am_InitializeDetailForNewModeButDoNotShow();
        }

        public override void am_OnFinishClick(AB_WizardButtonClickEventArgs Args)
        {
            Args.Cancel = true;
            if (!_sharedObject.CurrentOrderEntity.am_ValidateEntity(out ObservableCollection<AB_Message> messages, OrderEntity.WarehouseNameProperty, OrderEntity.StatusProperty, OrderEntity.SalesPersonNameProperty))
            {
                ap_MessageConsole.am_AddMessages(messages, false);

                return;
            }

            spProgress.Visibility = System.Windows.Visibility.Visible;
            ap_BtnFinishIsEnabled = false;
            _OrderDetail.OrderDetailLayout.IsEnabled = false;

            AB_WPFHelperMethods.am_CallInBackground(
                inBackground: () =>
                {
                    return _sharedObject.AsyncProcessWizard();
                },
                onSuccess: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessage("Wizard has successfully completed.", true);
                },
                onError: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true);
                }
            );
        }
    }
}
