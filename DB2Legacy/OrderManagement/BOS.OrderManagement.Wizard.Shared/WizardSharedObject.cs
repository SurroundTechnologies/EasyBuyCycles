using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerDataEntity;
using BOS.OrderDataEntity;
using BOS.OrderItemDataEntity;
using BOS.ShippingAddressDataEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion
    }
}
