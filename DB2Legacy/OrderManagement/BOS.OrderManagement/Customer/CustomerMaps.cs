//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DataMaps.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Collections.Generic;
using A4DN.Core.BOS.Base;
using SharedSystemProperties = BOS.OrderManagement.Shared.Properties;
using BOS.CustomerDataEntity;

namespace BOS.CustomerDataMaps
{
	public class CustomerMaps : AB_DataMaps
	{
		private const string YD1CTableName = "YD1C";
		private const string YD1CTableAlias_b = "b";

		public CustomerMaps() : base() { }
		public CustomerMaps(string qualifier) : base(qualifier) { }
		  
		/// <summary>
		/// Loads maps to join two database files.
		/// </summary>
		public override Dictionary<string, AB_RelationshipMap> am_LoadRelationshipMaps()
		{            
			var relationshipMap = new AB_RelationshipMapsDictionary(ap_PrimaryTable);
		   
			// TODO: Table Relationships Step 1 - Define and relationships and join conditions for each file and add relationships (Change 0 to 1, 2, ... n for each new file map)
			// AB_RelationshipMap map0 = new AB_RelationshipMap("PrimaryFile", "SecondaryFile", JoinType.LeftOuter);  // Create a map to a single file
			// TODO: Table Relationships Step 2 - Add Joins for each relationship
			// Two field Relationship 
			// map0.ap_JoinConditions.Add(new AB_JoinCondition(new AB_QueryField("FileName", "FieldName"), "=", new AB_QueryField("FileName", "FieldName")));
			// Single Field to Constant Relationship
			// map0.ap_JoinConditions.Add(new AB_JoinCondition(new AB_QueryField("FileName", "FieldName"), "=", new AB_QueryConstant("ConstantValue")));
			// relationshipMap.Add("Y06T", map0); // Add to the relationship Dictionary keyed by Secondary File
		 
            relationshipMap.am_AddRelationshipMap(YD1CTableName, useDistinctJoins: false, joinTableAlias:YD1CTableAlias_b)
			.am_JoinWhere(primaryTableField:"YD1CPTID", joinTableField:"YD1CIID");

			return relationshipMap;
		}
		

		/// <summary>
		/// Defines the maps for the Data Source Field Names and Entity Properties for module.
		/// </summary>
		public override Dictionary<string,AB_DataSourcePropertyReference> am_LoadFieldMaps(string qualifier)
		{	
			// Set the Primary File Name, Foreign fields will have to be mapped on a case-by-case basis
			ap_PrimaryTable = YD1CTableName;
			//Create a dictionary to hold the maps
			var maps = new AB_DataMapsDictionary(ap_PrimaryTable, qualifier);
		 
			maps.am_AddDataMap("YD1CIID", CustomerEntity.CustomerInternalIDProperty);
			maps.am_AddDataMap("YD1CPTID", CustomerEntity.ParentInternalIDProperty);
			maps.am_AddDataMap("YD1CPTRL", CustomerEntity.ParentRelationshipProperty);
			maps.am_AddDataMap("YD1CNM", CustomerEntity.NameProperty);
			maps.am_AddDataMap("YD1CNMLG", CustomerEntity.LegalNameProperty);
			maps.am_AddDataMap("YD1CCNLN", CustomerEntity.ContactLastNameProperty);
			maps.am_AddDataMap("YD1CCNFN", CustomerEntity.ContactFirstNameProperty);
			maps.am_AddDataMap("YD1CCNMN", CustomerEntity.ContactMiddleNameProperty);
			maps.am_AddDataMap("YD1CCNNN", CustomerEntity.ContactNickNameProperty);
			maps.am_AddDataMap("YD1CBLA1", CustomerEntity.BillingAddress1Property);
			maps.am_AddDataMap("YD1CBLA2", CustomerEntity.BillingAddress2Property);
			maps.am_AddDataMap("YD1CBLA3", CustomerEntity.BillingAddress3Property);
			maps.am_AddDataMap("YD1CBLPC", CustomerEntity.BillingPostalCodeProperty);
			maps.am_AddDataMap("YD1CBLCY", CustomerEntity.BillingCountryProperty);
			maps.am_AddDataMap("YD1CTL", CustomerEntity.TelephoneProperty);
			maps.am_AddDataMap("YD1CEM", CustomerEntity.EmailProperty);
			maps.am_AddDataMap("YD1CM1", CustomerEntity.MemoProperty);
			maps.am_AddDataMap("YD1CPRPT", CustomerEntity.PurchasePointsProperty);
			maps.am_AddDataMap("YD1CCRDT", CustomerEntity.CreateDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1CCRTM", CustomerEntity.CreateTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1CCRUS", CustomerEntity.CreateUserProperty);
			maps.am_AddDataMap("YD1CCRJB", CustomerEntity.CreateJobProperty);
			maps.am_AddDataMap("YD1CCRJN", CustomerEntity.CreateJobNumberProperty);
			maps.am_AddDataMap("YD1CLCDT", CustomerEntity.LastChangeDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1CLCTM", CustomerEntity.LastChangeTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1CLCUS", CustomerEntity.LastChangeUserProperty);
			maps.am_AddDataMap("YD1CLCJB", CustomerEntity.LastChangeJobProperty);
			maps.am_AddDataMap("YD1CLCJN", CustomerEntity.LastChangeJobNumberProperty);
			maps.am_AddDataMap(string.Format("{0}.{1}", YD1CTableAlias_b, "YD1CNM"), CustomerEntity.ParentNameProperty, targetTable: YD1CTableAlias_b);

			//TODO: CustomerMaps Real Field Example
			//maps.am_AddDataMap("<Field Name>", CustomerEntity.<Property Name>);
			//TODO: CustomerMaps Virtual Field Example
			//maps.am_AddDataMap("<Field Name>", CustomerEntity.<Property Name>, isVirtual: true);
			//TODO: CustomerMaps Foreign Field Example
			//maps.am_AddDataMap(string.Format("{0}.{1}", "<Target Table Name>", "<Field Name>"), CustomerEntity.<Property Name>, targetTable: "<Target Table Name>"); 
			//TODO: CustomerMaps Configure Example (for setting options not available as constructor arguments)
			//maps.am_AddDataMap(...).am_Configure((map) => { map.ap_FunctionExpresion = "..."; });
		  
			return maps;
		}

	}
}