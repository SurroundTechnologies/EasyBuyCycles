using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.OrderManagement.Shared
{
    public static class WizardSummaryController
    {
        static WizardSummary _CustomerSummaryItem = new WizardSummary();
        public static WizardSummary CustomerSummaryItem
        {
            get { return _CustomerSummaryItem; }
            set { _CustomerSummaryItem = value; }
        }

        static WizardSummary _ShippingAddressSummaryItem = new WizardSummary();
        public static WizardSummary ShippingAddressSummaryItem
        {
            get { return _ShippingAddressSummaryItem; }
            set { _ShippingAddressSummaryItem = value; }
        }

        static WizardSummary _OrderSummaryItem = new WizardSummary();
        public static WizardSummary OrderSummaryItem
        {
            get { return _OrderSummaryItem; }
            set { _OrderSummaryItem = value; }
        }

        static WizardSummary _OrderItemSummaryItem = new WizardSummary();
        public static WizardSummary OrderItemSummaryItem
        {
            get { return _OrderItemSummaryItem; }
            set { _OrderItemSummaryItem = value; }
        }
    }
}
