using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderDataEntity;
using BOS.OrderItemDataEntity;
using BOS.OrderManagement.Shared.Properties;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using WPF.OrderItem;
using WPF.OrderManagement.Shared;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for OrderItemWizardPart.xaml
    /// </summary>
    public partial class OrderItemWizardPart : AB_WizardPartBase
    {
        private OrderItemDetail _ItemDetail;
        private OrderWizardObject _sharedObject;
        public const string Step_OrderItem = "WPF.Wizards.OrderWizard.OrderItemWizardPart";

        private OrderItemEntity _CurrentEntity;
        public OrderItemEntity CurrentEntity
        {
            get { return _CurrentEntity; }
            set { _CurrentEntity = value; }
        }

        public OrderItemWizardPart() : this(null)
        {

        }

        public OrderItemWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _sharedObject = (OrderWizardObject)ap_SharedWizardObject;
            _ItemDetail = new OrderItemDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(2), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _ItemDetail.OrderItemDetailLayout.SetBinding(DataContextProperty, new Binding("CurrentEntity")
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });
            _ItemDetail.Field_PurchaseOrderNumber.Visibility = Visibility.Collapsed;
            itemsDetail.Content = _ItemDetail.OrderItemDetailLayout;

            _CurrentEntity = new OrderItemEntity() { ap_RecordMode = AB_RecordMode.New };
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_OrderItem, DescriptionResource.ORDERITEM, DescriptionResource.ENTERORDERITEMINFORMATION, "OrderItem.png"));
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _ItemDetail.am_InitializeDetailForNewModeButDoNotShow();
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!_CurrentEntity.am_ValidateEntity(out ObservableCollection<AB_Message> messages))
            {
                ap_MessageConsole.am_ClearMessages();
                ap_MessageConsole.am_AddMessages(messages, false);
                ap_MessageConsole.am_AddMessage(DescriptionResource.ENTERORDERITEMINFORMATION, false);

                return;
            }
            _CurrentEntity.ProductCode = _ItemDetail.EntityInTheDropDown.Code;
            _CurrentEntity.ProductName = _ItemDetail.EntityInTheDropDown.Name;
            _sharedObject.OrderItems.Add(_CurrentEntity);
            _CurrentEntity = new OrderItemEntity() { ap_RecordMode = AB_RecordMode.New };
            _ItemDetail.OrderItemDetailLayout.SetBinding(DataContextProperty, new Binding("CurrentEntity")
            {
                Source = this,
                Mode = BindingMode.OneWay
            });
        }

        private void bRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrderItems.SelectedItem == null)
            {
                return;
            }

            _sharedObject.OrderItems.Remove(dgOrderItems.SelectedItem as OrderItemEntity);
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if (_sharedObject.OrderItems.Count == 0)
            {
                ap_MessageConsole.am_AddMessage(new AB_Message(DescriptionResource.NOORDERITEMS), false);
                Args.Cancel = true;

                return;
            }

            WizardSummaryController.OrderItemSummaryItem.Header = DescriptionResource.UNITPRICE;
            
            // calculate total
            decimal total = 0;
            foreach (OrderItemEntity entity in _sharedObject.OrderItems)
            {
                total += (entity.UnitPrice ?? 0) * (entity.Quantity ?? 0);
            }

            WizardSummaryController.OrderItemSummaryItem.Info = total.ToString("C");

            am_MarkCurrentStepComplete();
        }
    }
}
