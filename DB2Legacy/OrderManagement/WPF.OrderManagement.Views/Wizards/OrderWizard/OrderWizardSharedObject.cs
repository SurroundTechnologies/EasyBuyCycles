using A4DN.Core.WPF.Base.WizardBase;
using BOS.CustomerDataEntity;
using BOS.OrderDataEntity;
using BOS.OrderItemDataEntity;
using BOS.ShippingAddressDataEntity;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WPF.Wizards.OrderWizard
{
    public class OrderWizardSharedObject : AB_WizardSharedObjectBase
    {
        #region Part Entities

        public CustomerEntity Customer { get; set; }
        public ObservableCollection<OrderItemEntity> OrderItems { get; set; } = new ObservableCollection<OrderItemEntity>();
        public ShippingAddressEntity ShippingAddress { get; set; }

        private OrderEntity _Order;
        public OrderEntity Order
        {
            get => _Order;
            set
            {
                if (_Order != value)
                {
                    _Order = value;
                    OnRecordsProcessed();
                }

                if (_Order == null && OrdersCreated > 0)
                {
                    OnNewOrderStarted();
                }
            }
        }

        #endregion

        #region Summary Item Text Blocks

        public TextBlock CurrentOrderSummaryHeaderTextBlock { get; set; } = new TextBlock { Margin = new Thickness(5, 5, 5, 0) };
        public TextBlock CustomerSummaryItemTextBlock { get; set; } = new TextBlock { Margin = new Thickness(5, 0, 5, 0) };
        public TextBlock OrderItemsSummaryItemTextBlock { get; set; } = new TextBlock { Margin = new Thickness(5, 0, 5, 0) };
        public TextBlock ShippingAddressSummaryItemTextBlock { get; set; } = new TextBlock { Margin = new Thickness(5, 0, 5, 0) };
        public TextBlock WizardSessionSummaryHeaderTextBlock { get; set; } = new TextBlock { Margin = new Thickness(5, 0, 5, 0) };
        public TextBlock WizardSessionSummaryContentsTextBlock { get; set; } = new TextBlock { Margin = new Thickness(5, 0, 5, 5) };

        #endregion

        #region Wizard Session Summary Properties

        public int OrdersCreated { get; set; }
        public decimal OrdersTotal { get; set; }
        public decimal OrdersDiscountedTotal { get; set; }

        #endregion

        #region Event Handlers

        public event EventHandler RecordsProcessed;
        protected void OnRecordsProcessed()
        {
            RecordsProcessed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler NewOrderStarted;
        protected void OnNewOrderStarted()
        {
            NewOrderStarted?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        public void ResetPropertiesForNextLoop()
        {
            Order = null;
            Customer = null;
            ShippingAddress = null;
            OrderItems = new ObservableCollection<OrderItemEntity>();
            CustomerSummaryItemTextBlock.Text = string.Empty;
            OrderItemsSummaryItemTextBlock.Text = string.Empty;
            ShippingAddressSummaryItemTextBlock.Text = string.Empty;
        }

        #endregion
    }
}
