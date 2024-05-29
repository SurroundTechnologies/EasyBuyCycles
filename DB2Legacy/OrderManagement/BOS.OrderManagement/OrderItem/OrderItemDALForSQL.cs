//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DALForSQL.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DALBaseForSQL;
using BOS.OrderItemDataEntity;
using BOS.OrderItemDataMaps;


namespace BOS.OrderItemDataAccessLayer
{
	/// <summary>
	/// Contains operations for data access and IO.
	/// </summary>
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class OrderItemDALForSQL : AB_BusinessObjectDALBaseForSQL<OrderItemEntity>, BOS.OrderItemDataAccessLayer.IOrderItemDALServiceContract
	{

		/// <summary>
		/// Used to pass static data for the instance constructor
		/// </summary>
		protected static AB_DataMapContainer DataMapContainer;
		
		/// <summary>
		/// Static Declaration of each map we need to use
		/// </summary>
		protected static OrderItemMaps OrderItemMaps = new OrderItemMaps();
	   
		/// <summary>
		/// Initializes static variables
		/// </summary>
		private static void RG_StaticInitialize()
		{           
			 // Load Data Maps 
			 DataMapContainer = am_MergeMaps(OrderItemMaps, null);            
		}

		/// <summary>
		/// Generated method that sets properties of the parent class before it is instantiated.
		/// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
		/// </summary>
		private void RG_SetParentProperties()
		{	         
			// Set the DataMaps instance Property to Static DataMap Collection
			ap_DataMapContainer = DataMapContainer;
			
			// Set properties to change the parent initialization.
		}        
		

		/// <summary>
		/// Type initializer / static constructor
		/// </summary>
		static OrderItemDALForSQL()
		{
			RG_StaticInitialize();
		}  

		/// <summary>
		/// This method is called during the parent's constructor. Set properties to change the parent initialization.
		/// </summary>
		protected override void am_SetParentProperties()
		{
			RG_SetParentProperties();
		}        
	   
		/// <summary>
		/// Sets parameters for connection to override app config.
		/// </summary>
		protected override AB_ConnectionParms am_SetDatabaseParms()
		{
			AB_ConnectionParms overrideParms = base.am_SetDatabaseParms();
			// Do Overrides here:
			//overrideParms.Collection = "DifferentCollection";
			return overrideParms;
		}

		#region Standard Operations

		///// <summary>
		///// Override for default to insert data.
		///// </summary>
		//public override AB_InsertReturnArgs am_Insert(AB_InsertInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Insert(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Override for default select data.
		///// </summary>
		//public override AB_SelectReturnArgs am_Select(AB_SelectInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Select(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Override for default to fetch data.
		///// </summary>
		//public override AB_FetchReturnArgs am_Fetch(AB_FetchInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Fetch(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Override for default to update data.
		///// </summary>
		//public override AB_UpdateReturnArgs am_Update(AB_UpdateInputArgs inputArgs)
		//{
		//	var retArgs = base.am_Update(inputArgs);
		//	return retArgs;
		//}

		///// <summary>
		///// Override for default to delete data.
		///// </summary>
		//public override AB_PermDeleteReturnArgs am_PermDeleteEntity(AB_PermDeleteInputArgs inputArgs)
		//{
		//	return base.am_PermDeleteEntity(inputArgs);
		//}

		#endregion Standard Operations
	  
		//// A4DN_Tag: Add Custom Method Example - Step 2: Implement Method as defined in the Interface  IOrderItemDALServiceContract
		//// My Custom Method 
		////=============================================================
		//public string MyCustomMethod(string myString)
		//{ 
		//// Do Something and return 

		//    return myString;                       

		//}

	}

}