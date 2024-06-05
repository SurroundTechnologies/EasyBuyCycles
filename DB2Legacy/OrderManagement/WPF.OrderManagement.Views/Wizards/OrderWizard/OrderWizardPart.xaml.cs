using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderViewModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using WPF.Order;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for OrderWizardPart.xaml
    /// </summary>
    public partial class OrderWizardPart : AB_WizardPartBase
    {
        private OrderWizardObject _sharedObject;
        private OrderDetail _OrderDetail;
        public const string Step_Order = "WPF.Wizards.OrderWizard.OrderWizardPart";

        public OrderWizardPart() : this(null)
        {

        }

        public OrderWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _sharedObject = (OrderWizardObject)ap_SharedWizardObject;
            _OrderDetail = new OrderDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(3), ap_SharedWizardObject.ap_WizardMessageConsole, true));
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
                    using (var vm = new OrderVM())
                    {
                        var inArgs = new AB_ProcessRequestInputArgs<OrderEntity>("ASYNCPROCESSORDERWIZARD", _sharedObject.CurrentOrderEntity).am_WithCustomInputArgs(_sharedObject);
                        return vm.am_ProcessRequest(inArgs);

                        //return vm.AsyncProcessOrderWizard(_sharedObject);
                    }
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
