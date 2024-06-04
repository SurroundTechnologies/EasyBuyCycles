using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkBusinessProcess;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderItemDataEntity;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderManagement.Wizard.Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using WPF.OrderItem;

namespace WPF.OrderManagement.Wizards
{
    /// <summary>
    /// Interaction logic for OrderItemWizardPart.xaml
    /// </summary>
    public partial class OrderItemWizardPart : AB_WizardPartBase
    {
        private OrderItemDetail _ItemDetail;
        private WizardSharedObject _sharedObject;

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

            _sharedObject = (WizardSharedObject)ap_SharedWizardObject;
            _ItemDetail = new OrderItemDetail(new AB_DetailInitializationArgs(AB_FrameworkDataBP.am_GetModuleEntity(10), ap_SharedWizardObject.ap_WizardMessageConsole, true));
            _ItemDetail.OrderItemDetailLayout.SetBinding(DataContextProperty, new Binding("CurrentEntity")
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });

            itemsDetail.Content = _ItemDetail.OrderItemDetailLayout;

            _CurrentEntity = new OrderItemEntity() { ap_RecordMode = AB_RecordMode.New };
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

            am_MarkCurrentStepComplete();
        }
    }
}
