using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerDataEntity;
using BOS.OrderItemDataEntity;
using BOS.ShippingAddressDataEntity;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BOS.OrderDataEntity
{
    public class OrderWizardObject : AB_WizardSharedObjectBase, INotifyPropertyChanged
    {
        public OrderWizardObject()
        {
            OrderItems = new ObservableCollection<OrderItemEntity>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}
