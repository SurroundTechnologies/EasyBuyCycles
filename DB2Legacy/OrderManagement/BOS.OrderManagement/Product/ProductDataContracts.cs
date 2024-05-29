//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DataContracts.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using A4DN.Core.BOS.Base;

namespace BOS.ProductDataEntity
{
	// Data Contracts
	// *** Use this document to define input and return data objects for custom operations.
	// *** Input DataContracts should extend AB_InputArgsBase, which includes standard criteria such as UserID, Password, etc
	// *** Return DataContracts should extend AB_ReturnArgsBase, which includes standard message and return code properties
	// *** If the custom operation does not return anything beyond messages and/or a status/return code, AB_ReturnArgsBase can be used
	// *** 
	// *** Also define custom data contracts for any select operations for which you want to limit the fields passed with search input
	// *** Custom select, or "search", objects should extend AB_BusinessObjectEntityBase
	//=============================================================

	#region Custom Input Data

	//// My Custom Input Args
	////========================
	//[DataContract]
	//public class MyCustomInputArgs : AB_InputArgsBase
	//{
	//    [DataMember]
	//    public int MyCustomID { get; set; }

	//    [DataMember]
	//    public string MyCustomDescription { get; set; }
	//}

	#endregion Custom Input Data

	#region Custom Return Data

	//// My Custom Return Args
	////========================
	//[DataContract]
	//public class MyCustomReturnArgs : AB_ReturnArgsBase
	//{
	//    [DataMember]
	//    public int MyReturnNumber { get; set; }
	//}

	#endregion

	#region Select Input Data  
  
	//// My Custom Search Entity
	////========================
	//[DataContract]
	//public class MyCustomSearchEntity : AB_BusinessObjectEntityBase
	//{
	//    [DataMember]
	//    public string FirstName { get; set; }

	//    [DataMember]
	//    public string LastName { get; set; }
	//}

	#endregion Select Input Data
}