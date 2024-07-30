using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderItemDataEntity;
using BOS.OrderManagement.Shared;
using BOS.OrderManagement.Shared.Properties;
using System;
using System.Windows;
using System.Windows.Data;
using WPF.OrderItem;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for OrderItemWizardPart.xaml
    /// </summary>
    public partial class OrderItemWizardPart : AB_WizardPartBase
    {
        public const string Step_OrderItem = "OrderItem";

        private OrderItemDetail _OrderItemDetail;
        private OrderWizardSharedObject _SharedObject => ap_SharedWizardObject as OrderWizardSharedObject;

        private OrderItemEntity CurrentOrderItemEntity
        {
            get => (OrderItemEntity)GetValue(CurrentOrderItemEntityProperty);
            set => SetValue(CurrentOrderItemEntityProperty, value);
        }
        private static readonly DependencyProperty CurrentOrderItemEntityProperty = DependencyProperty.Register(nameof(CurrentOrderItemEntity), typeof(OrderItemEntity), typeof(OrderItemWizardPart), new PropertyMetadata(null));


        public OrderItemWizardPart() : this(null)
        {

        }

        public OrderItemWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            CurrentOrderItemEntity = new OrderItemEntity() { ap_RecordMode = AB_RecordMode.New };
            _OrderItemDetail = new OrderItemDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(Constants.MODULE_OrderItem), ap_MessageConsole, true));
            _OrderItemDetail.OrderItemDetailWizardPanel.SetBinding(DataContextProperty, new Binding(nameof(CurrentOrderItemEntity))
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });
            _OrderItemDetail.Field_PurchaseOrderNumber.Visibility = Visibility.Collapsed;
            itemsDetail.Content = _OrderItemDetail.OrderItemDetailWizardPanel;
            dgOrderItems.ItemsSource = _SharedObject.OrderItems;

            _OrderItemDetail.Field_Memo.ae_ValueChanged += MemoCharacterCountChanged;
            _SharedObject.NewOrderStarted += OnNewOrderStarted;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_OrderItem, DescriptionResource.ITEMS, DescriptionResource.ENTERORDERITEMINFORMATION, "OrderItem"));
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _OrderItemDetail.am_InitializeDetailForNewModeButDoNotShow();
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
            am_MarkStepIncomplete(Step_OrderItem);

            if (_SharedObject.OrderItems.Count == 0 && _SharedObject.OrdersCreated > 0)
            {
                dgOrderItems.ItemsSource = _SharedObject.OrderItems;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_OrderItemDetail.EntityInTheDropDown == null)
            {
                ap_MessageConsole.am_AddMessage(new AB_Message(DescriptionResource.NOORDERITEMS), true, true);
                return;
            }

            int.TryParse(_OrderItemDetail.Field_Quantity.ap_Value, out int quantity);

            if (quantity == 0)
            {
                ap_MessageConsole.am_AddMessage(new AB_Message("Quantity is required."), true, true);
                return;
            }

            ap_MessageConsole.am_ClearMessages();

            CurrentOrderItemEntity.ProductCode = _OrderItemDetail.EntityInTheDropDown.Code;
            CurrentOrderItemEntity.ProductName = _OrderItemDetail.EntityInTheDropDown.Name;
            var currentEntity = CurrentOrderItemEntity.am_DataContractDeepClone();
            _SharedObject.OrderItems.Add(currentEntity);

            CurrentOrderItemEntity = new OrderItemEntity() { ap_RecordMode = AB_RecordMode.New };
            _OrderItemDetail.EntityInTheDropDown = null;
            _OrderItemDetail.Field_PurchaseOrderNumber.Visibility = Visibility.Collapsed;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrderItems.SelectedItem == null)
            {
                return;
            }

            _SharedObject.OrderItems.Remove(dgOrderItems.SelectedItem as OrderItemEntity);
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if (_SharedObject.OrderItems.Count == 0)
            {
                ap_MessageConsole.am_AddMessage(new AB_Message(DescriptionResource.NOORDERITEMS), true, true);
                Args.Cancel = true;

                return;
            }

            am_MarkCurrentStepComplete();
        }

        private void MemoCharacterCountChanged(object sender, EventArgs e)
        {
            var charCount = (_OrderItemDetail.Field_Memo.DataContext as OrderItemEntity).Memo?.Length ?? 0;
            CurrentOrderItemEntity.MemoCharacterCount = charCount;
        }
        private void OnNewOrderStarted(object sender, EventArgs e)
        {
            am_MarkStepIncomplete(Step_OrderItem);
        }
    }
}
