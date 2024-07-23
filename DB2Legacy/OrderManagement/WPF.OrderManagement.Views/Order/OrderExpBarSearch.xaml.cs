//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.ExplorerBar.Search.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using BOS.OrderDataEntity;
using WPF.OrderManagement.Shared;

namespace WPF.Order
{
	/// <summary>
	/// Interaction logic for the Search Explorer Bar
	/// </summary>
	public partial class OrderExpBarSearch : AB_ExplorerBarBase
	{
		/// <summary>
		/// Type initializer / static constructor
		/// </summary>
		static OrderExpBarSearch()
		{
			RG_StaticInit();
		}

		/// <summary>
		/// Initializes static members
		/// </summary>
		private static void RG_StaticInit()
		{
			// Enroll Culture Resource
			AB_SystemController.ap_SystemPropertyMethods.am_EnrollResourceCultureInfo(typeof(OrderExpBarSearch), WPF.OrderManagement.Views.Properties.Resources.Culture);
			
		}

		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// </summary>
		private void RG_SetParentProperties()
		{
			// Call Initialize Component im order to access XAML objects in Code Behind
			InitializeComponent();
						// Set Entity Type
			ap_SearchEntityType = typeof(OrderEntity);
		}

				
		/// <summary>
		/// Alters UI based on selected view. Runs when view is switched.
		/// </summary>
		protected override void am_ViewSwitched(string currentView)
		{
		}
		
		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor. 
		/// </summary>
		protected override void am_SetParentProperties()
		{
			RG_SetParentProperties();
		}

		/// <summary>
		/// This method is called after the Loaded Event. At this point the XAML objects can be accessed.
		/// </summary>
		protected override void am_OnInitialized()
		{
			base.am_OnInitialized();
			
		}

		/// <summary>
		/// This method is called when the clear button event is fired. 
		/// </summary>
		protected override void am_ResetSearchEntity()
		{
			base.am_ResetSearchEntity();
		}
		
		/// <summary>
		/// This is called after the parent keys have been loaded into ap_SearchEntity. 
		/// </summary>
		protected override void am_AfterSubBrowserKeysLoaded()
		{
			// A4DN_Tag: Set any additional search criteria here.
			// Example:
			//((OrderEntity)ap_SearchEntity).MyProperty = "foo";
			
		}

		protected override void am_OnClearButtonClicked()
		{
			base.am_OnClearButtonClicked();

			Utilities.ClearDateField(Field_OrderDateStart, Field_OrderDateEnd);
			Utilities.ClearDateTimeField(Field_CreateDateTimeStart, Field_CreateDateTimeEnd);
			Utilities.ClearDateTimeField(Field_LastChangeDateTimeStart, Field_LastChangeDateTimeEnd);
		}

		/// <summary>
		/// Override for query object 
		/// </summary>
		protected override AB_Query am_BuildFilter()
		{
			var whereFilter = base.ap_SearchEntity.am_BuildDefaultQuery();

			Utilities.AddDateRangeFilter(whereFilter, Field_OrderDateStart.ap_Value, Field_OrderDateEnd.ap_Value, OrderEntity.OrderDateProperty);
			Utilities.AddDateTimeRangeFilter(whereFilter, Field_CreateDateTimeStart.ap_Value, Field_CreateDateTimeEnd.ap_Value, OrderEntity.CreateDateProperty, OrderEntity.CreateTimeProperty);
			Utilities.AddDateTimeRangeFilter(whereFilter, Field_LastChangeDateTimeStart.ap_Value, Field_LastChangeDateTimeEnd.ap_Value, OrderEntity.LastChangeDateProperty, OrderEntity.LastChangeTimeProperty);

			return whereFilter;
		}

		private void DisabledMouseWheelInput(object sender, MouseWheelEventArgs e)
		{
			var scrollViewer = AB_WPFHelperMethods.am_FindVisualParent<ScrollViewer>((UIElement)sender);
			if (scrollViewer != null)
			{
				// Pass the mouse wheel event to the ScrollViewer
				var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
				e2.RoutedEvent = UIElement.MouseWheelEvent;
				scrollViewer.RaiseEvent(e2);

				// Mark the event as handled to prevent it from being processed by the ListBox
				e.Handled = true;
			}
		}
	}
}