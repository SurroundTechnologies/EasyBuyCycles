using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerBusinessProcess;
using BOS.CustomerDataEntity;
using BOS.OrderBusinessProcess;
using BOS.OrderDataEntity;
using BOS.OrderItemBusinessProcess;
using BOS.OrderItemDataEntity;
using BOS.OrderManagement.Shared.Properties;
using BOS.ShippingAddressBusinessProcess;
using BOS.ShippingAddressDataEntity;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BOS.OrderManagement.Wizard.Shared
{
    public class WizardSharedObject : AB_WizardSharedObjectBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WizardSharedObject()
        {
            OrderItems = new ObservableCollection<OrderItemEntity>();
        }

        #region Properties

        private CustomerEntity _CurrentCustomerEntity;
        public CustomerEntity CurrentCustomerEntity
        {
            get { return _CurrentCustomerEntity; }
            set
            {
                if (value != _CurrentCustomerEntity)
                {
                    _CurrentCustomerEntity = value;
                    NotifyPropertyChanged("CurrentCustomerEntity");
                }
            }
        }

        private OrderEntity _CurrentOrderEntity;
        public OrderEntity CurrentOrderEntity
        {
            get { return _CurrentOrderEntity; }
            set
            {
                if (value != _CurrentOrderEntity)
                {
                    _CurrentOrderEntity = value;
                    NotifyPropertyChanged("CurrentOrderEntity");
                }
            }
        }

        private ShippingAddressEntity _CurrentShippingAddressEntity;
        public ShippingAddressEntity CurrentShippingAddressEntity
        {
            get { return _CurrentShippingAddressEntity; }
            set { _CurrentShippingAddressEntity = value; }
        }

        private ObservableCollection<OrderItemEntity> _OrderItems;
        public ObservableCollection<OrderItemEntity> OrderItems
        {
            get { return _OrderItems; }
            set { _OrderItems = value; }
        }

        private bool _IsSpecifyingExistingCustomer;
        public bool IsSpecifyingExistingCustomer
        {
            get { return _IsSpecifyingExistingCustomer; }
            set
            {
                if (value != _IsSpecifyingExistingCustomer)
                {
                    _IsSpecifyingExistingCustomer = value;
                    NotifyPropertyChanged("IsSpecifyingExistingCustomer");
                }
            }
        }

        private bool _IsSpecifyingExistingShippingAddress;
        public bool IsSpecifyingExistingShippingAddress
        {
            get { return _IsSpecifyingExistingShippingAddress; }
            set
            {
                if (value != _IsSpecifyingExistingShippingAddress)
                {
                    _IsSpecifyingExistingShippingAddress = value;
                    NotifyPropertyChanged("IsSpecifyingExistingShippingAddress");
                }
            }
        }

        private string _ProgressMessage;
        public string ProgressMessage
        {
            get { return _ProgressMessage; }
            set
            {
                if (value != _ProgressMessage)
                {
                    _ProgressMessage = value;
                    NotifyPropertyChanged("ProgressMessage");
                }
                _ProgressMessage = value;
            }
        }

        private int _ProgressPercentage;
        public int ProgressPercentage
        {
            get { return _ProgressPercentage; }
            set
            {
                if (value != _ProgressPercentage)
                {
                    _ProgressPercentage = value;
                    NotifyPropertyChanged("ProgressPercentage");
                }
            }
        }

        #endregion

        public AB_InsertReturnArgs AsyncProcessWizard()
        {
            ProgressMessage = DescriptionResource.PROCESSINGSTARTED;
            ProgressPercentage = 0;

            //Customer insertion if needed
            if (!IsSpecifyingExistingCustomer)
            {
                ProgressMessage = DescriptionResource.CREATINGNEWCUSTOMER;

                using (var _CustomerBP = new CustomerBP())
                {
                    var custInsertInArgs = new AB_InsertInputArgs<CustomerEntity>(CurrentCustomerEntity);
                    var custInsertRetArgs = (AB_InsertReturnArgs<CustomerEntity>)_CustomerBP.am_Insert(custInsertInArgs);
                    if (custInsertRetArgs.ap_ReturnCode != AB_ReturnCodes.OK.ToString())
                    {
                        ProgressMessage = DescriptionResource.ERRORCREATINGNEWCUSTOMER;
                        ProgressPercentage = 100;

                        return custInsertRetArgs;
                    }
                    CurrentCustomerEntity = (CustomerEntity)custInsertRetArgs.ap_OutputEntity;
                    ProgressPercentage = 25;
                }
            }

            //shipping address insertion
            if (!_IsSpecifyingExistingShippingAddress)
            {
                ProgressMessage = DescriptionResource.CREATINGNEWSHIPPINGADDRESS;
                CurrentShippingAddressEntity.CustomerInternalID = CurrentCustomerEntity.CustomerInternalID;

                using (var _ShippingAddressBP = new ShippingAddressBP())
                {
                    var shipAddrInsertInArgs = new AB_InsertInputArgs<ShippingAddressEntity>
                    (CurrentShippingAddressEntity);
                    var shipAddrInsertRetArgs = (AB_InsertReturnArgs<ShippingAddressEntity>)_ShippingAddressBP.am_Insert(shipAddrInsertInArgs);
                    
                    if (shipAddrInsertRetArgs.ap_ReturnCode != AB_ReturnCodes.OK.ToString())
                    {
                        ProgressMessage = DescriptionResource.ERRORCREATINGNEWSHIPPINGADDRESS;
                        ProgressPercentage = 100;

                        return shipAddrInsertRetArgs;
                    }

                    CurrentShippingAddressEntity = (ShippingAddressEntity)shipAddrInsertRetArgs.ap_OutputEntity;
                    ProgressPercentage = 50;
                }
            }

            //Order insertion
            ProgressMessage = DescriptionResource.CREATINGNEWORDER;
            using (var _OrderBP = new OrderBP())
            {
                CurrentOrderEntity.OrderTime = DateTime.Now.TimeOfDay;
                CurrentOrderEntity.OrderDate = DateTime.Today;
                CurrentOrderEntity.ShippingAddressInternalID = CurrentShippingAddressEntity.ShippingAddressInternalID;
                CurrentOrderEntity.CustomerInternalID = CurrentCustomerEntity.CustomerInternalID;

                var orderInsertInArgs = new AB_InsertInputArgs<OrderEntity>(CurrentOrderEntity);
                var orderInsertRetArgs = (AB_InsertReturnArgs<OrderEntity>)_OrderBP.am_Insert(orderInsertInArgs);

                if (orderInsertRetArgs.ap_ReturnCode != AB_ReturnCodes.OK.ToString())
                {
                    ProgressMessage = DescriptionResource.ERRORCREATINGNEWORDER;
                    ProgressPercentage = 100;

                    return orderInsertRetArgs;
                }

                CurrentOrderEntity = (OrderEntity)orderInsertRetArgs.ap_OutputEntity;
                ProgressPercentage = 75;
            }

            //Order Items insertion
            ProgressMessage = DescriptionResource.CREATINGNEWORDERITEMS;
            using (var _OrderItemBP = new OrderItemBP())
            {

                foreach (OrderItemEntity orderItem in _OrderItems)
                {
                    orderItem.OrderInternalID = CurrentOrderEntity.OrderInternalID;
                    var orderItemInsertInArgs = new AB_InsertInputArgs<OrderItemEntity>(orderItem);
                    var orderItemInsertRetArgs = (AB_InsertReturnArgs<OrderItemEntity>)_OrderItemBP.am_Insert(orderItemInsertInArgs);

                    if (orderItemInsertRetArgs.ap_ReturnCode != AB_ReturnCodes.OK.ToString())
                    {
                        ProgressMessage = DescriptionResource.ERRORCREATINGNEWORDERITEMS;
                        ProgressPercentage = 100;

                        return orderItemInsertRetArgs;
                    }
                }

                ProgressPercentage = 100;
                ProgressMessage = DescriptionResource.PROCESSINGCOMPLETE;
            }

            return new AB_InsertReturnArgs { ap_ReturnCode = AB_ReturnCodes.OK.ToString() };
        }
    }
}
