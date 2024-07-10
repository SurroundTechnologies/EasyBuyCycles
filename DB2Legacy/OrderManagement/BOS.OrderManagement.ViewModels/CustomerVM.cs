//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.ViewModel.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using A4DN.Core.BOS.ViewModel;
using A4DN.Core.BOS.Base;
using BOS.CustomerBusinessProcess;
using BOS.CustomerDataEntity;
using System.Windows;

namespace BOS.CustomerViewModel
{   
	/// <summary>
	/// The ViewModel houses all calls from the UI to the Business Process (BP). It also houses management of the visual state between the view and the data model.
	/// Each call to the BP can be conditioned to call the BP as a web service or directly as an object in a referenced assembly. 
	/// Examples of implementing this can be found within the body of this class.
	/// </summary>
	public class CustomerVM : AB_ViewModel<CustomerEntity>
	{
        public CustomerEntity SelectedParentCustomer
        {
            get => (CustomerEntity)GetValue(SelectedParentCustomerProperty);
            set => SetValue(SelectedParentCustomerProperty, value);
        }
        public static readonly DependencyProperty SelectedParentCustomerProperty = DependencyProperty.Register(nameof(SelectedParentCustomer), typeof(CustomerEntity), typeof(CustomerVM), new PropertyMetadata(null));

        public bool IsMapTabVisible
		{
			get { return (bool)GetValue(IsMapTabVisibleProperty); }
			set { SetValue(IsMapTabVisibleProperty, value); }
		}
		public static readonly DependencyProperty IsMapTabVisibleProperty = DependencyProperty.Register("IsMapTabVisible", typeof(bool), typeof(CustomerVM), new PropertyMetadata(true));

		/// <summary>
		/// Sets properties of the parent class before it is instantiated.
		/// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
		/// </summary>
		private void RG_SetParentProperties()
		{
			// Set WCF Service BP Proxy Class Name
			ap_RemoteBPTypeFullName = "BOS.OrderManagement.ViewModels.Customer.ServiceProxy.CustomerBPServiceContractClient";
			
			// Set Direct Access BP Type Class Name Type
			ap_DirectBPType = typeof(CustomerBP);
			// Set WCF Service Endpoint Name as defined in Service Host
			ap_RemoteBPServiceName = "CustomerBP.svc";
		}

		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// </summary>
		protected override void am_SetParentProperties()
		{
			RG_SetParentProperties();

			// Set properties to change the parent initialization.
		}

		 /// <summary>
		/// This method is called once the BP has been initialized.
		/// </summary>
		protected override void am_OnBPObjectInitialized()
		{

		}

		#region Examples

		// Example - Set Command State - ("CommandID", Visibility, IsChecked, IsEnabled):
		// am_SetCommandState(new AB_CommandState("DELETE", true, false, false));


        // Example - Set Property as IsEnabled, IsInError, IsReadonly, IsRequired, Visibility, etc... :
        // inputArgs.ap_PropertyModelDictionary[CustomerEntity.<Property>.ap_PropertyName].ap_IsReadOnly = true;

		#endregion // Examples

		/// <summary>
		/// This method is called everytime a record selection changes. The CurrentSelectedItems List contains all items Selected.  The FocusedItem contains the current Item that has focus.
		/// </summary>            
		public override void am_SetUpVisualModelForContent(AB_VisualModelInitArgs inputArgs, System.Collections.IList currentSelectedItems, CustomerEntity focusedItem)
		{
			switch (inputArgs.ap_SelectionState)
			{
				case AB_SelectionState.Multiple:
					break;
				case AB_SelectionState.One:
					break;
				case AB_SelectionState.Zero:
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// This method is called before the DataContext is set on the detail
		/// </summary>       
		public override void am_SetUpVisualModelForDetail(AB_VisualModelInitArgs inputArgs, CustomerEntity currentDataContext)
		{
			IsMapTabVisible = true;

            switch (inputArgs.ap_Mode)
			{
				case AB_RecordMode.Display:
					break;
				case AB_RecordMode.New:
					inputArgs.ap_PropertyModelDictionary[CustomerEntity.ParentRelationshipProperty.ap_PropertyName].ap_IsReadOnly = true;
					IsMapTabVisible = false;
					ValidateParentCustomer(inputArgs, currentDataContext);
                    break;
				case AB_RecordMode.Open:
                    ValidateParentCustomer(inputArgs, currentDataContext);
                    break;
				case AB_RecordMode.Preview:
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// This method is called to handle a property change and the updating of property models
		/// </summary>        
		protected override void am_OnCurrentEntityPropertyChanged(AB_VisualModelInitArgs inputArgs, string propertyName, CustomerEntity currentEntity)
		{

		}

		private void ValidateParentCustomer(AB_VisualModelInitArgs inputArgs, CustomerEntity currentDataContext)
		{
            // Put More Code Here
            if (currentDataContext.ParentInternalID == 0)
            {
                inputArgs.ap_PropertyModelDictionary[CustomerEntity.ParentRelationshipProperty.ap_PropertyName].ap_IsReadOnly = true;
            }
		}

		
		#if SILVERLIGHT
		
		#else
		
		#region Standard Operations

		///// <summary>
		///// Calls the Business Process to insert data.
		///// </summary>
		//public override AB_InsertReturnArgs am_Insert(AB_InsertInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Insert(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Calls the Business Process to select data.
		///// </summary>
		//public override AB_SelectReturnArgs am_Select(AB_SelectInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Select(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Calls the Business Process to fetch data.
		///// </summary>
		//public override AB_FetchReturnArgs am_Fetch(AB_FetchInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Fetch(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Calls the Business Process to update data.
		///// </summary>
		//public override AB_UpdateReturnArgs am_Update(AB_UpdateInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Update(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Calls the Business Process to delete data.
		///// </summary>
		//public override AB_PermDeleteReturnArgs am_Delete(AB_PermDeleteInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Delete(inputArgs);
		//	return retArgs;
		//}

		#endregion Standard Operations
		
		#endif
	}
}