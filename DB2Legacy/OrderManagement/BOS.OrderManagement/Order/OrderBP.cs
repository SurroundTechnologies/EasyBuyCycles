//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.BP.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using A4DN.Core.BOS.Base;
using BOS.CustomerBusinessProcess;
using BOS.CustomerDataEntity;
using BOS.OrderDataAccessLayer;
using BOS.OrderDataEntity;
using BOS.OrderItemBusinessProcess;
using BOS.OrderItemDataEntity;
using BOS.OrderManagement;
using BOS.OrderManagement.Shared.Properties;
using BOS.ShippingAddressBusinessProcess;
using BOS.ShippingAddressDataEntity;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace BOS.OrderBusinessProcess
{
    /// <summary>
    /// This interface defines the service contract used to expose module Business Process operations via WCF services.
    /// Any custom methods that are created in the Business Process and need to be accesible through WCF must be added to this interface
    /// and marked with the [OperationContract] attribute. This interface by default inherits from AB_IBusinessObjectProcessBaseServiceContract<T>
    /// which includes operation contracts for standard IO (Insert, Select, Fetch, Update, Delete) so only custom operations need to be defined here.
    /// </summary>
    [ServiceContract]
    public interface IOrderBPServiceContract : AB_IBusinessObjectProcessBaseServiceContract<OrderEntity>
    {
        // A4DN_Tag: Add Custom Method Example - Step 1: Define the custom method signature.
        // Any custom methods that are defined is this interface must be marked with the attribute [OperationContract] in order to be accessed through WCF. 

        // Example:
        //[OperationContract]
        // string MyCustomMethod(string myString);
    }

    /// <summary>
    /// The Business Process (BP) class contains all main business logic for this module. All new methods related
    /// to business logic or processing should be added here. Any methods within this class can also be exposed as 
    /// web services if a corresponding OperationContract is defined in the interface this class implements.
    /// For any methods in this class that call an operation in the Data Access Layer (DAL) the call should be 
    /// conditioned to support calling the DAL as a directly referenced object or via a web service call. Examples
    /// of implementing this can be found within the body of this class.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OrderBP : AB_FrameworkBusinessObjectProcessBase<OrderEntity>, IOrderBPServiceContract
    {

        /// <summary>
        /// Sets properties of the parent class before it is instantiated.
        /// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
        /// </summary>
        private void RG_SetParentProperties()
        {
            // Set WCF Service DAL Proxy Class Name
            ap_ServiceDalTypeFullName = "BOS.OrderManagement.Order.ServiceProxy.OrderDALServiceContractClient";
            // Set Direct Access DAL Type Class Name Type
            ap_DirectAccessDalType = typeof(OrderDALForSQL);
            // Set WCF Service Endpoint Name as defined in Service Host
            ap_SvcName = "OrderDAL.svc";
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
        /// This method is called once the DataAccessLayer (DAL) has been initialized.
        /// </summary>
        protected override void am_OnDalObjectInitialized()
        {

        }

        /// <summary>
        /// Any commands issued to am_ProcessRequest can be handled here before being passed to the 
        /// Data Access Layer (DAL) if necessary. If they are not intercepted, the base Business Process (BP)
        /// will handle the command and pass it through to the DAL. 
        /// </summary>
        public override AB_ProcessRequestReturnArgs am_ProcessRequest(AB_ProcessRequestInputArgs inputArgs)
        {
            switch (inputArgs.ap_CommandID)
            {
                //case "MyCommandID":

                //Do the work, all the while conditioning calls to the DAL based on whether the DAL is a service.

                default:
                    // By default, this will call ProcessRequest in the DAL.  Could be used for calling a 
                    // stored procedure or Web Service.
                    return base.am_ProcessRequest(inputArgs);
            }
        }

        //// A4DN_Tag: Add Custom Method Example - Step 2: Implement Method as defined in the Interface IContactBPServiceContract
        //// My Custom Method 
        ////=============================================================
        //public MyCustomReturnArgs MyCustomMethod(MyCustomInputArgs inputArgs)
        //{
        //    // Do Something and return 
        //    if (ap_DALisService)
        //    {
        //        // Call using WCF Services
        //        return ((OrderDALServiceContractClient)ap_DalObject).MyCustomMethod(inputArgs);
        //    }
        //    else
        //    {
        //        // Call using Direct Access
        //        return ((OrderDALForSQL)ap_DalObject).MyCustomMethod(inputArgs);
        //    }
        //}

        //// A4DN_Tag: Add Custom Select Method Example
        //// My Custom Select
        ////=============================================================
        //public AB_SelectReturnArgs<OrderEntity> SelectContactByID(AB_SelectInputArgs<MyCustomSearchEntity> inputArgs)
        //{
        //    OrderEntity EntityWithSearchCriteria = new OrderEntity() { SomeProperty = inputArgs.EntityWithSelectionParams.SomeProperty };
        //    AB_SelectInputArgs<OrderEntity> fullInputArgs = new AB_SelectInputArgs<OrderEntity>("MyView", inputArgs.ap_Method, EntityWithSearchCriteria, null, inputArgs.ap_MaxCount, false);
        //    return (AB_SelectReturnArgs<OrderEntity>)am_Select((AB_SelectInputArgs)fullInputArgs);
        //}

        #region Standard Operations

        /// <summary>
        /// Inserts data. Can be overriden if the insert functionality for an entire module
        /// should be different than what the parent Business Process (BP) does by default.
        /// </summary>
        public override AB_InsertReturnArgs am_Insert(AB_InsertInputArgs inputArgs)
        {
			if (!this.AreDropdownCodesValid(inputArgs, out AB_InsertReturnArgs error)) return error;
			var retArgs = base.am_Insert(inputArgs);
			return retArgs;
		}

        ///// <summary>
        ///// Selects data. Can be overriden if the insert functionality for an entire module
        ///// should be different than what the parent Business Process (BP) does by default.
        ///// </summary>
        //public override AB_SelectReturnArgs am_Select(AB_SelectInputArgs inputArgs)
        //{
        //	var retArgs = base.am_Select(inputArgs);
        //	return retArgs;
        //}

        ///// <summary>
        ///// Standard fetch IO operation. Can be overriden if the insert functionality for an entire module
        ///// should be different than what the parent Business Process (BP) does by default.
        ///// </summary>
        //public override AB_FetchReturnArgs am_Fetch(AB_FetchInputArgs fetchInputArgs)
        //{
        //	var retArgs = base.am_Fetch(fetchInputArgs);
        //	return retArgs;
        //}

        /// <summary>
        /// Updates data. Can be overriden if the insert functionality for an entire module
        /// should be different than what the parent Business Process (BP) does by default.
        /// </summary>
        public override AB_UpdateReturnArgs am_Update(AB_UpdateInputArgs inputArgs)
        {
			if (!this.AreDropdownCodesValid(inputArgs, out AB_UpdateReturnArgs error)) return error;
			var retArgs = base.am_Update(inputArgs);
			return retArgs;
		}

        ///// <summary>
        ///// Deletes data. Can be overriden if the insert functionality for an entire module
        ///// should be different than what the parent Business Process (BP) does by default.
        ///// </summary>
        //public override AB_PermDeleteReturnArgs am_Delete(AB_PermDeleteInputArgs inputArgs)
        //{
        //	var retArgs = base.am_Delete(inputArgs);
        //	return retArgs;
        //}

        #endregion Standard OperationsS

    }
}