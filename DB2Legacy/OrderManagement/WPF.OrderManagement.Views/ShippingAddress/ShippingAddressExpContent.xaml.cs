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
using BOS.ShippingAddressViewModel;
using BOS.ShippingAddressDataEntity;
using BOS.ShippingAddressDataMaps;

namespace WPF.ShippingAddress
{
	/// <summary>
	/// Content Window for the module. Contains areas to handle commands before and after the parent has handled them. Interacts with the ViewModel.
	/// </summary>
	public partial class  ShippingAddressExpContent : AB_ContentWindowBase
	{
		ShippingAddressVM _ViewModel; 

		/// <summary>
		/// Type initializer / static constructor
		/// </summary>
		static ShippingAddressExpContent()
		{
			RG_StaticInit();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ShippingAddressExpContent(AB_VisualComponentInitializationArgs initializationArgs)
			: base(initializationArgs) {  }


		/// <summary>
		/// Initializes static members
		/// </summary>
		private static void RG_StaticInit()
		{
			// Enroll Culture Resource
			AB_SystemController.ap_SystemPropertyMethods.am_EnrollResourceCultureInfo(typeof(ShippingAddressExpContent), WPF.OrderManagement.Views.Properties.Resources.Culture);
		}
		
		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
		/// </summary>
		private void RG_SetParentProperties()
		{
			// Call Initialize Component im order to access XAML objects in Code Behind
			InitializeComponent();
			
			ap_DataMaps = new ShippingAddressMaps().ap_FieldMaps;
			_ViewModel = FindResource("ShippingAddressVM") as ShippingAddressVM;
			ap_ViewModel = _ViewModel;
			ap_MainDetailType = typeof(ShippingAddressDetail);
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
			// Access the Current Selected Entity
		    //var selectedEntity = ap_SelectedEntity as ShippingAddressEntity;
            //if (selectedEntity != null)
            //{
            //    var myVariable = selectedEntity.<Property>;
            //}

			switch (command.ap_CommandID)
			{
				//case "<CommandID>":

                //    Do Something ...

              	//    set e.Handled to true to prevent the higher level from executing its command click logic and to prevent further processing by the Detail.
                //    e.Handled = true;
                
                //    break;

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