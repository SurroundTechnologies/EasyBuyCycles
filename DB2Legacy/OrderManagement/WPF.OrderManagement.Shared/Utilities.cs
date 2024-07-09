using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF.OrderManagement.Shared
{
    public static class Utilities
    {
        public static AB_MessageConsole MessageConsole { get; set; }

        public static void CopyToClipboard(string stringToCopy, AB_MessageConsole messageConsole = null)
        {
            if (messageConsole == null)
            {
                messageConsole = MessageConsole;
            }

            if (!string.IsNullOrEmpty(stringToCopy))
            {
                Clipboard.SetText(stringToCopy);
                messageConsole.am_AddToastNotificationMessage("Copied to clipboard.", AB_ToastMessageMode.Success);
            }
            else
            {
                messageConsole.am_AddToastNotificationMessage("Nothing was copied.", AB_ToastMessageMode.Error);
            }
        }

        public static void OpenWithGoogleMaps(string address, AB_MessageConsole messageConsole = null)
        {
            if (messageConsole == null)
            {
                messageConsole = MessageConsole;
            }

            if (!string.IsNullOrEmpty(address) || address.Trim() != "0")
            {
                var url = "https://www.google.com/maps/search/" + Uri.EscapeDataString(address);
                Process.Start(url);
                messageConsole.am_AddToastNotificationMessage("Opening Google Maps.", AB_ToastMessageMode.Success);
            }
            else
            {
                messageConsole.am_AddToastNotificationMessage("Invalid addreess.", AB_ToastMessageMode.Error);
            }
        }

        public static void CallCustomer(string fullPhoneNumber, AB_MessageConsole messageConsole = null)
        {
            if (messageConsole == null)
            {
                messageConsole = MessageConsole;
            }

            if (!string.IsNullOrEmpty(fullPhoneNumber) && fullPhoneNumber.Length == 10)
            {
                Process.Start("tel:" + fullPhoneNumber);
                messageConsole.am_AddToastNotificationMessage("Opening phone app.", AB_ToastMessageMode.Success);
            }
            else
            {
                messageConsole.am_AddToastNotificationMessage("Invalid phone number.", AB_ToastMessageMode.Error);
            }
        }

        public static void EmailCustomer(string email, AB_MessageConsole messageConsole = null)
        {
            if (messageConsole == null)
            {
                messageConsole = MessageConsole;
            }

            try
            {
                var mail = new MailAddress(email);
                Process.Start("mailto:" + email);
                messageConsole.am_AddToastNotificationMessage("Opening email app.", AB_ToastMessageMode.Success);
            }
            catch (Exception e)
            {
                messageConsole.am_AddToastNotificationMessage("Invalid email." + Environment.NewLine + e.Message, AB_ToastMessageMode.Error);
            }
        }

		public static void AddDateRangeFilter(AB_Query whereFilter, DateTime? startDate, DateTime? endDate, AB_IPropertyMetadata propertyName)
		{
			if (startDate != null)
			{
				whereFilter.am_AddWhereClause(propertyName, ">=", startDate.Value);
			}
			if (endDate != null)
			{
				whereFilter.am_AddWhereClause(propertyName, "<", endDate.Value);
			}
		}

		public static void ClearDateField(AB_DatePicker startDateField, AB_DatePicker endDateField)
		{
			(startDateField.Content as DatePicker).SelectedDate = DateTime.Today;
			startDateField.ap_Value = null;

			(endDateField.Content as DatePicker).SelectedDate = DateTime.Today;
			endDateField.ap_Value = null;
		}
	}
}
