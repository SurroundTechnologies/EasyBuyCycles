//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DataMaps.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Collections.Generic;
using A4DN.Core.BOS.Base;
using SharedSystemProperties = BOS.OrderManagement.Shared.Properties;
using BOS.ShippingAddressDataEntity;

namespace BOS.ShippingAddressDataMaps
{
	public class ShippingAddressMaps : AB_DataMaps
	{
		private const string YD1STableName = "YD1S";
		private const string YD1CTableName = "YD1C";

		public ShippingAddressMaps() : base() { }
		public ShippingAddressMaps(string qualifier) : base(qualifier) { }
		  
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
		 
            relationshipMap.am_AddRelationshipMap(YD1CTableName, useDistinctJoins: false)
			.am_JoinWhere(primaryTableField:"YD1S1CID", joinTableField:"YD1CIID");

			return relationshipMap;
		}
		

		/// <summary>
		/// Defines the maps for the Data Source Field Names and Entity Properties for module.
		/// </summary>
		public override Dictionary<string,AB_DataSourcePropertyReference> am_LoadFieldMaps(string qualifier)
		{	
			// Set the Primary File Name, Foreign fields will have to be mapped on a case-by-case basis
			ap_PrimaryTable = YD1STableName;
			//Create a dictionary to hold the maps
			var maps = new AB_DataMapsDictionary(ap_PrimaryTable, qualifier);
		 
			maps.am_AddDataMap("YD1SIID", ShippingAddressEntity.ShippingAddressInternalIDProperty);
			maps.am_AddDataMap("YD1S1CID", ShippingAddressEntity.CustomerInternalIDProperty);
			maps.am_AddDataMap("YD1SNM", ShippingAddressEntity.NameProperty);
			maps.am_AddDataMap("YD1SCNLN", ShippingAddressEntity.ContactLastNameProperty);
			maps.am_AddDataMap("YD1SCNFN", ShippingAddressEntity.ContactFirstNameProperty);
			maps.am_AddDataMap("YD1SCNMN", ShippingAddressEntity.ContactMiddleNameProperty);
			maps.am_AddDataMap("YD1SCNNN", ShippingAddressEntity.ContactNickNameProperty);
			maps.am_AddDataMap("YD1SSHA1", ShippingAddressEntity.Address1Property);
			maps.am_AddDataMap("YD1SSHA2", ShippingAddressEntity.Address2Property);
			maps.am_AddDataMap("YD1SSHA3", ShippingAddressEntity.Address3Property);
			maps.am_AddDataMap("YD1SSHPC", ShippingAddressEntity.PostalCodeProperty);
			maps.am_AddDataMap("YD1SSHCY", ShippingAddressEntity.CountryProperty);
			maps.am_AddDataMap("YD1STL", ShippingAddressEntity.TelephoneProperty);
			maps.am_AddDataMap("YD1SEM", ShippingAddressEntity.EmailProperty);
			maps.am_AddDataMap("YD1SM1", ShippingAddressEntity.MemoProperty);
			maps.am_AddDataMap("YD1SPRPT", ShippingAddressEntity.PurchasePointsProperty);
			maps.am_AddDataMap("YD1SCRDT", ShippingAddressEntity.CreateDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1SCRTM", ShippingAddressEntity.CreateTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1SCRUS", ShippingAddressEntity.CreateUserProperty);
			maps.am_AddDataMap("YD1SCRJB", ShippingAddressEntity.CreateJobProperty);
			maps.am_AddDataMap("YD1SCRJN", ShippingAddressEntity.CreateJobNumberProperty);
			maps.am_AddDataMap("YD1SLCDT", ShippingAddressEntity.LastChangeDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1SLCTM", ShippingAddressEntity.LastChangeTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1SLCUS", ShippingAddressEntity.LastChangeUserProperty);
			maps.am_AddDataMap("YD1SLCJB", ShippingAddressEntity.LastChangeJobProperty);
			maps.am_AddDataMap("YD1SLCJN", ShippingAddressEntity.LastChangeJobNumberProperty);
			maps.am_AddDataMap(string.Format("{0}.{1}", YD1CTableName, "YD1CNM"), ShippingAddressEntity.CustomerNameProperty, targetTable: YD1CTableName);

			//TODO: ShippingAddressMaps Real Field Example
			//maps.am_AddDataMap("<Field Name>", ShippingAddressEntity.<Property Name>);
			//TODO: ShippingAddressMaps Virtual Field Example
			//maps.am_AddDataMap("<Field Name>", ShippingAddressEntity.<Property Name>, isVirtual: true);
			//TODO: ShippingAddressMaps Foreign Field Example
			//maps.am_AddDataMap(string.Format("{0}.{1}", "<Target Table Name>", "<Field Name>"), ShippingAddressEntity.<Property Name>, targetTable: "<Target Table Name>"); 
			//TODO: ShippingAddressMaps Configure Example (for setting options not available as constructor arguments)
			//maps.am_AddDataMap(...).am_Configure((map) => { map.ap_FunctionExpresion = "..."; });
		  
			return maps;
		}

	}
}