//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DALServiceContract.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using A4DN.Core.BOS.Base;
using BOS.CustomerDataEntity;

namespace BOS.CustomerDataAccessLayer
{
	/// <summary>
	/// This interface defines the service contract used to expose module Data Access Layer (DAL) operations via WCF services.
	/// Any custom methods that are created in the DAL and need to be accesible through WCF must be added to this interface
	/// and marked with the [OperationContract] attribute. This interface by default inherits from AB_IBusinessObjectDALBaseServiceContract<T>
	/// which includes operation contracts for standard IO (Insert, Select, Fetch, Update, Delete) so only custom operations need to be defined here.
	/// </summary>
	[ServiceContract]
	public interface ICustomerDALServiceContract : AB_IBusinessObjectDALBaseServiceContract<CustomerEntity>
	{
	   // A4DN_Tag: Add Custom Method Example - Step 1: Define the custom method signature.
	   // Any custom methods that are defined is this interface must be marked with the attribute [OperationContract] in order to be accessed through WCF. 
			  
	   // Example:
	   //[OperationContract]
	   // string MyCustomMethod(string myString);
	}
	
}