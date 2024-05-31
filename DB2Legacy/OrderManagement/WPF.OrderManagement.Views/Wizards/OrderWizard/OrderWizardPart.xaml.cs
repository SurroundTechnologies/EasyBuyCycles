using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerDataEntity;
using BOS.CustomerViewModel;
using BOS.OrderDataEntity;
using BOS.OrderItemDataEntity;
using BOS.OrderItemViewModel;
using BOS.OrderManagement.Shared.Properties;
using BOS.OrderViewModel;
using BOS.ShippingAddressDataEntity;
using BOS.ShippingAddressViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using WPF.Order;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for OrderWizardPart.xaml
    /// </summary>
    public partial class OrderWizardPart : AB_WizardPartBase
    {
        public const string Step_Order = "Order";

        private OrderWizardSharedObject _SharedObject => ap_SharedWizardObject as OrderWizardSharedObject;
        private OrderDetail _OrderDetail;
        private OrderEntity CurrentOrderEntity
        {
            get => (OrderEntity)GetValue(CurrentOrderEntityProperty);
            set => SetValue(CurrentOrderEntityProperty, value);
        }
        private static readonly DependencyProperty CurrentOrderEntityProperty = DependencyProperty.Register(nameof(CurrentOrderEntity), typeof(OrderEntity), typeof(OrderWizardPart), new PropertyMetadata(null));


        public OrderWizardPart() : this(null)
        {

        }

        public OrderWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();

            _OrderDetail = new OrderDetail(new AB_DetailInitializationArgs(AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(3), ap_MessageConsole, true));
            _OrderDetail.NonWizardControlsVisibility = Visibility.Collapsed;
            _OrderDetail.OrderDetailLayout.SetBinding(DataContextProperty, new Binding(nameof(CurrentOrderEntity))
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });

            presenter.Content = _OrderDetail.OrderDetailLayout;
            CurrentOrderEntity = new OrderEntity() { ap_RecordMode = AB_RecordMode.New };

            _SharedObject.NewOrderStarted += OnNewOrderStarted;
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Order, DescriptionResource.ORDERCONFIRMATION, DescriptionResource.ENTERORDERINFORMATION, "Order.png"));
        }

        public override void am_InitializeOrResetWizardPart()
        {
            _OrderDetail.am_InitializeDetailForNewModeButDoNotShow();
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
            am_MarkStepIncomplete(Step_Order);

            _OrderDetail.Field_CustomerInternalID.IsReadOnly = true;
            _OrderDetail.Field_ShippingAddressInternalID.IsReadOnly = true;

            if (_SharedObject.Customer.CustomerInternalID != null)
            {
                CurrentOrderEntity.CustomerInternalID = _SharedObject.Customer.CustomerInternalID;
                _OrderDetail.Field_CustomerInternalID.Visibility = Visibility.Visible;
                Field_CustomerName.ap_Value = string.Empty;
                Field_CustomerName.Visibility = Visibility.Collapsed;
            }
            else
            {
                _OrderDetail.Field_CustomerInternalID.Visibility = Visibility.Collapsed;
                Field_CustomerName.ap_Value = _SharedObject.Customer.Name;
                Field_CustomerName.Visibility = Visibility.Visible;
            }

            if (_SharedObject.ShippingAddress.ShippingAddressInternalID != null)
            {
                CurrentOrderEntity.ShippingAddressInternalID = _SharedObject.ShippingAddress.ShippingAddressInternalID;
                _OrderDetail.Field_ShippingAddressInternalID.Visibility = Visibility.Visible;
                Field_ShippingAddressName.ap_Value = string.Empty;
                Field_ShippingAddressName.Visibility = Visibility.Collapsed;
            }
            else
            {
                _OrderDetail.Field_ShippingAddressInternalID.Visibility = Visibility.Collapsed;
                Field_ShippingAddressName.ap_Value = _SharedObject.ShippingAddress.Name;
                Field_ShippingAddressName.Visibility = Visibility.Visible;
            }
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            if (!CurrentOrderEntity.am_ValidateEntity(out ObservableCollection<AB_Message> messages, OrderEntity.WarehouseNameProperty, OrderEntity.PurchaseOrderNumberIDProperty, OrderEntity.SalesPersonNameProperty))
            {
                ap_MessageConsole.am_ClearMessages();
                ap_MessageConsole.am_AddMessages(messages, true, true);

                Args.Cancel = true;

                return;
            }

            _SharedObject.Order = CurrentOrderEntity;

            if (string.IsNullOrEmpty(_SharedObject.WizardSessionSummaryHeaderTextBlock.Text))
            {
                _SharedObject.WizardSessionSummaryHeaderTextBlock.Inlines.Add(new Underline(new Run("Wizard Session\n")));
            }

            ProcessRecords();

            am_MarkCurrentStepComplete();
        }

        private void ProcessRecords()
        {
            if (_SharedObject.Customer != null)
            {
                if (!_SharedObject.Customer.ap_IsFromDB)
                {
                    InsertNewCustomerRecord();
                }
                else if (_SharedObject.Customer.ap_IsFromDB && _SharedObject.Customer.ap_IsModified)
                {
                    UpdateExistingCustomerRecord();
                }
                else
                {
                    ProcessShippingAddress();
                }
            }
        }

        private void InsertNewCustomerRecord()
        {
            AB_WPFHelperMethods.am_CallInBackground(
                inBackground: () =>
                {
                    using (var vm = new CustomerVM())
                    {
                        var inArgs = new AB_InsertInputArgs<CustomerEntity>(_SharedObject.Customer);
                        return vm.am_Insert(inArgs);
                    }
                },
                onSuccess: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessage("Customer has successfully been created.", true);

                    var outputEntityId = (retArgs.ap_OutputEntity as CustomerEntity).CustomerInternalID;

                    if (_SharedObject.Customer.CustomerInternalID == null)
                    {
                        _SharedObject.Customer.CustomerInternalID = outputEntityId;
                    }

                    if (_SharedObject.ShippingAddress.CustomerInternalID == null)
                    {
                        _SharedObject.ShippingAddress.CustomerInternalID = outputEntityId;
                    }

                    ProcessShippingAddress();
                },
                onError: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true, true);
                }
            );
        }

        private void UpdateExistingCustomerRecord()
        {
            AB_WPFHelperMethods.am_CallInBackground(
                inBackground: () =>
                {
                    using (var vm = new CustomerVM())
                    {
                        var inArgs = new AB_UpdateInputArgs<CustomerEntity>(_SharedObject.Customer);
                        return vm.am_Update(inArgs);
                    }
                },
                onSuccess: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessage("Customer has successfully been updated.", true);
                    ProcessShippingAddress();
                },
                onError: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true, true);
                }
            );
        }

        private void ProcessShippingAddress()
        {
            if (_SharedObject.ShippingAddress != null)
            {
                if (!_SharedObject.ShippingAddress.ap_IsFromDB)
                {
                    InsertNewShippingAddressRecord();
                }
                else if (_SharedObject.ShippingAddress.ap_IsFromDB && _SharedObject.ShippingAddress.ap_IsModified)
                {
                    UpdateExistingShippingAddressRecord();
                }
                else
                {
                    InsertOrder();
                }
            }
        }

        private void InsertNewShippingAddressRecord()
        {
            if (_SharedObject.ShippingAddress != null && _SharedObject.ShippingAddress.ap_IsFromDB && !_SharedObject.ShippingAddress.ap_IsModified) return;

            AB_WPFHelperMethods.am_CallInBackground(
                inBackground: () =>
                {
                    using (var vm = new ShippingAddressVM())
                    {
                        var inArgs = new AB_InsertInputArgs<ShippingAddressEntity>(_SharedObject.ShippingAddress);
                        return vm.am_Insert(inArgs);
                    }
                },
                onSuccess: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessage("Shipping Address has successfully been created.", true);

                    if (_SharedObject.ShippingAddress.ShippingAddressInternalID == null)
                    {
                        _SharedObject.ShippingAddress.ShippingAddressInternalID = (retArgs.ap_OutputEntity as ShippingAddressEntity).ShippingAddressInternalID;
                    }

                    InsertOrder();
                },
                onError: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true, true);
                }
            );
        }

        private void UpdateExistingShippingAddressRecord()
        {
            if (_SharedObject.ShippingAddress != null && _SharedObject.ShippingAddress.ap_IsFromDB && !_SharedObject.ShippingAddress.ap_IsModified) return;

            AB_WPFHelperMethods.am_CallInBackground(
                inBackground: () =>
                {
                    using (var vm = new ShippingAddressVM())
                    {
                        var inArgs = new AB_InsertInputArgs<ShippingAddressEntity>(_SharedObject.ShippingAddress);
                        return vm.am_Insert(inArgs);
                    }
                },
                onSuccess: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessage("Shipping Address has successfully been updated.", true);
                    InsertOrder();
                },
                onError: (retArgs) =>
                {
                    ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true, true);
                }
            );
        }

        private void InsertOrder()
        {
            if (_SharedObject.Order != null)
            {
                AB_WPFHelperMethods.am_CallInBackground(
                    inBackground: () =>
                    {
                        using (var vm = new OrderVM())
                        {
                            if (_SharedObject.Order.CustomerInternalID == null)
                            {
                                _SharedObject.Order.CustomerInternalID = _SharedObject.Customer.CustomerInternalID;
                            }

                            if (_SharedObject.Order.ShippingAddressInternalID == null)
                            {
                                _SharedObject.Order.ShippingAddressInternalID = _SharedObject.ShippingAddress.ShippingAddressInternalID;
                            }

                            var inArgs = new AB_InsertInputArgs<OrderEntity>(_SharedObject.Order);
                            return vm.am_Insert(inArgs);
                        }
                    },
                    onSuccess: (retArgs) =>
                    {
                        ap_MessageConsole?.am_AddMessage("Orders have successfully been created.", true);
                        var outputEntity = (retArgs.ap_OutputEntity as OrderEntity);

                        _SharedObject.Order = outputEntity;

                        if (outputEntity != null)
                        {
                            InsertOrderItems(outputEntity);
                        }
                    },
                    onError: (retArgs) =>
                    {
                        ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true, true);
                    }
                );
            }
        }

        private void InsertOrderItems(OrderEntity parentOrder)
        {
            if (parentOrder != null)
            {
                AB_WPFHelperMethods.am_CallInBackground(
                    beforeBackground: () =>
                    {
                        foreach (var orderItem in _SharedObject.OrderItems)
                        {
                            orderItem.OrderInternalID = parentOrder.OrderInternalID;
                            orderItem.PurchaseOrderNumberID = parentOrder.PurchaseOrderNumberID;

                            if (!orderItem.am_ValidateEntity(out ObservableCollection<AB_Message> messages))
                            {
                                ap_MessageConsole.am_ClearMessages();
                                ap_MessageConsole.am_AddMessages(messages, true, true);

                                return;
                            }
                        }
                    },
                    inBackground: () =>
                    {
                        using (var vm = new OrderItemVM())
                        {
                            var inArgs = new AB_BatchInsertInputArgs<OrderItemEntity>(_SharedObject.OrderItems);
                            return vm.am_BatchInsert(inArgs);
                        }
                    },
                    onSuccess: (retArgs) =>
                    {
                        ap_MessageConsole?.am_AddMessage("Order Items have successfully been created.", true);

                        var orderTotal = _SharedObject.OrderItems.Sum(x => x.OrderTotal + x.OrderDiscount) ?? 0;
                        var orderDiscount = _SharedObject.OrderItems.Sum(x => x.OrderDiscount) ?? 0;

                        // update wizard session summary info
                        _SharedObject.OrdersCreated += 1;
                        _SharedObject.OrdersTotal += orderTotal;
                        _SharedObject.OrdersDiscountedTotal += orderTotal - orderDiscount;

                        // update wizard session summary
                        var wizardSessionSummary = $"Orders Created: {_SharedObject.OrdersCreated}\nOrders Total: {_SharedObject.OrdersTotal:C}\nOrders Discounted Total: {_SharedObject.OrdersDiscountedTotal:C}";
                        _SharedObject.WizardSessionSummaryContentsTextBlock.Text = wizardSessionSummary;

                        CurrentOrderEntity = new OrderEntity() { ap_RecordMode = AB_RecordMode.New };

                        ap_MessageConsole?.am_AddMessage(new AB_Message { Text = "Wizard has successfully completed.", ReturnCode = AB_ReturnCodes.OK.ToString() }, true, true);
                    },
                    onError: (retArgs) =>
                    {
                        ap_MessageConsole?.am_AddMessages(retArgs.ap_Messages, true, true);
                    }
                );
            }
        }

        private void OnNewOrderStarted(object sender, EventArgs e)
        {
            am_MarkStepIncomplete(Step_Order);
        }

    }
}
