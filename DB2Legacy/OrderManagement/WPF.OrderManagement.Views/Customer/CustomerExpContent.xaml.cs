//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.ContentWindow.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using A4DN.Core.WPF.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.ViewModel;
using BOS.CustomerViewModel;
using BOS.CustomerDataEntity;
using BOS.CustomerDataMaps;
using WPF.OrderManagement.Shared;

namespace WPF.Customer
{
	/// <summary>
	/// Content Window for the module. Contains areas to handle commands before and after the parent has handled them. Interacts with the ViewModel.
	/// </summary>
	public partial class  CustomerExpContent : AB_ContentWindowBase
	{
		CustomerVM _ViewModel; 

		/// <summary>
		/// Type initializer / static constructor
		/// </summary>
		static CustomerExpContent()
		{
			RG_StaticInit();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CustomerExpContent(AB_VisualComponentInitializationArgs initializationArgs)
			: base(initializationArgs) {  }


		/// <summary>
		/// Initializes static members
		/// </summary>
		private static void RG_StaticInit()
		{
			// Enroll Culture Resource
			AB_SystemController.ap_SystemPropertyMethods.am_EnrollResourceCultureInfo(typeof(CustomerExpContent), WPF.OrderManagement.Views.Properties.Resources.Culture);
		}
		
		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
		/// </summary>
		private void RG_SetParentProperties()
		{
			// Call Initialize Component im order to access XAML objects in Code Behind
			InitializeComponent();
			
			ap_DataMaps = new CustomerMaps().ap_FieldMaps;
			_ViewModel = FindResource("CustomerVM") as CustomerVM;
			ap_ViewModel = _ViewModel;
			ap_MainDetailType = typeof(CustomerDetail);
		}
		
		/// <summary>
		/// This method is called during the parent's constructor. Set properties to change the parent initialization.
		/// </summary>
		protected override void am_SetParentProperties()
		{
			RG_SetParentProperties();
		}

		/// <summary>
		/// This method is called after the Loaded event. At this point the XAML objects can be accessed.
		/// </summary>
		protected override void am_OnInitialized()
		{
			base.am_OnInitialized();
		   
		}

        /// <summary>
        /// This method is called before the base object has processed the command.
        /// </summary>
        protected override void am_BeforeProcessCommand(AB_Command command, RoutedEventArgs e)
        {
            var selectedEntity = ap_SelectedEntity as CustomerEntity;

            switch (command.ap_CommandID)
            {
                case Constants.CMD_CopyAddressLine:
                    Utilities.CopyToClipboard(selectedEntity.BillingAddressLine);
                    e.Handled = true;
                    break;
                case Constants.CMD_CopyAddressBlock:
                    Utilities.CopyToClipboard(selectedEntity.BillingAddressBlock);
                    e.Handled = true;
                    break;
                case Constants.CMD_OpenInMaps:
                    Utilities.OpenWithGoogleMaps(selectedEntity.BillingAddressLine);
                    e.Handled = true;
                    break;
                case Constants.CMD_CallCustomer:
                    Utilities.CallCustomer(selectedEntity.Telephone);
                    e.Handled = true;
                    break;
                case Constants.CMD_EmailCustomer:
                    Utilities.EmailCustomer(selectedEntity.Email);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This method is called after the base object has processed the command.
        /// </summary>
        protected override void am_AfterProcessCommand(AB_Command command, RoutedEventArgs e)
		{
			
			switch (command.ap_CommandID)
			{
				//case "<CommandID>":

                //    Do Something ...

              	//	  set e.Handled to true to prevent further processing by the Detail.
                //    e.Handled = true;
                
                //    break;

				default:
					break;
			}
			
		}
	}
}