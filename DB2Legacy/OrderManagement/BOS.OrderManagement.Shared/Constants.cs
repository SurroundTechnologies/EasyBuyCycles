using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOS.OrderManagement.Shared
{
    public static class Constants
    {
        public const string CMD_OpenInMaps = "OPENINMAPS";
        public const string CMD_CopyAddressLine = "COPYADDRESSLINE";
        public const string CMD_CopyAddressBlock = "COPYADDRESSBLOCK";
        public const string CMD_CallCustomer = "CALLCUSTOMER";
        public const string CMD_EmailCustomer = "EMAILCUSTOMER";
        public const string CMD_ORDERWIZARD = "ORDERWIZARD";

        public const int MODULE_Customer = 1;
        public const int MODULE_OrderItem = 2;
        public const int MODULE_Order = 3;
        public const int MODULE_Product = 4;
        public const int MODULE_ShippingAddress = 5;
        public const int MODULE_Dashboard = 6;
    }
}
