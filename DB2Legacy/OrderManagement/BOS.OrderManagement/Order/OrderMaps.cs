//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DataMaps.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Collections.Generic;
using A4DN.Core.BOS.Base;
using SharedSystemProperties = BOS.OrderManagement.Shared.Properties;
using BOS.OrderDataEntity;

namespace BOS.OrderDataMaps
{
	public class OrderMaps : AB_DataMaps
	{
		private const string YD1OTableName = "YD1O";
		private const string YD1STableName = "YD1S";
		private const string YD1CTableName = "YD1C";

		public OrderMaps() : base() { }
		public OrderMaps(string qualifier) : base(qualifier) { }
		  
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
		 
            relationshipMap.am_AddRelationshipMap(YD1STableName, useDistinctJoins: false)
			.am_JoinWhere(primaryTableField:"YD1O1SID", joinTableField:"YD1SIID");

            relationshipMap.am_AddRelationshipMap(YD1CTableName, useDistinctJoins: false)
			.am_JoinWhere(primaryTableField:"YD1O1CID", joinTableField:"YD1CIID");

			return relationshipMap;
		}
		

		/// <summary>
		/// Defines the maps for the Data Source Field Names and Entity Properties for module.
		/// </summary>
		public override Dictionary<string,AB_DataSourcePropertyReference> am_LoadFieldMaps(string qualifier)
		{	
			// Set the Primary File Name, Foreign fields will have to be mapped on a case-by-case basis
			ap_PrimaryTable = YD1OTableName;
			//Create a dictionary to hold the maps
			var maps = new AB_DataMapsDictionary(ap_PrimaryTable, qualifier);
		 
			maps.am_AddDataMap("YD1OIID", OrderEntity.OrderInternalIDProperty);
			maps.am_AddDataMap("YD1O1CID", OrderEntity.CustomerInternalIDProperty);
			maps.am_AddDataMap("YD1ODT", OrderEntity.OrderDateProperty);
			maps.am_AddDataMap("YD1OTM", OrderEntity.OrderTimeProperty);
			maps.am_AddDataMap("YD1OPONO", OrderEntity.PurchaseOrderNumberIDProperty);
			maps.am_AddDataMap("YD1O1WID", OrderEntity.WarehouseInternalIDProperty);
			maps.am_AddDataMap("YD1O1WNM", OrderEntity.WarehouseNameProperty);
			maps.am_AddDataMap("YD1ODLM1", OrderEntity.DeliveryMemoProperty);
			maps.am_AddDataMap("YD1O1SID", OrderEntity.ShippingAddressInternalIDProperty);
			maps.am_AddDataMap("YD1OM1", OrderEntity.OrderMemoProperty);
			maps.am_AddDataMap("YD1OST", OrderEntity.StatusProperty);
			maps.am_AddDataMap("YD1O1AID", OrderEntity.SalesPersonInternalIDProperty);
			maps.am_AddDataMap("YD1O1ANM", OrderEntity.SalesPersonNameProperty);
			maps.am_AddDataMap("YD1CPRPT", OrderEntity.PurchasePointsProperty);
			maps.am_AddDataMap("YD1OCRDT", OrderEntity.CreateDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1OCRTM", OrderEntity.CreateTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1OCRUS", OrderEntity.CreateUserProperty);
			maps.am_AddDataMap("YD1OCRJB", OrderEntity.CreateJobProperty);
			maps.am_AddDataMap("YD1OCRJN", OrderEntity.CreateJobNumberProperty);
			maps.am_AddDataMap("YD1OLCDT", OrderEntity.LastChangeDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1OLCTM", OrderEntity.LastChangeTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1OLCUS", OrderEntity.LastChangeUserProperty);
			maps.am_AddDataMap("YD1OLCJB", OrderEntity.LastChangeJobProperty);
			maps.am_AddDataMap("YD1OLCJN", OrderEntity.LastChangeJobNumberProperty);
			maps.am_AddDataMap(string.Format("{0}.{1}", YD1CTableName, "YD1CNM"), OrderEntity.CustomerNameProperty, targetTable: YD1CTableName);
			maps.am_AddDataMap(string.Format("{0}.{1}", YD1STableName, "YD1SNM"), OrderEntity.ShippingAddressNameProperty, targetTable: YD1STableName);

			//TODO: OrderMaps Real Field Example
			//maps.am_AddDataMap("<Field Name>", OrderEntity.<Property Name>);
			//TODO: OrderMaps Virtual Field Example
			//maps.am_AddDataMap("<Field Name>", OrderEntity.<Property Name>, isVirtual: true);
			//TODO: OrderMaps Foreign Field Example
			//maps.am_AddDataMap(string.Format("{0}.{1}", "<Target Table Name>", "<Field Name>"), OrderEntity.<Property Name>, targetTable: "<Target Table Name>"); 
			//TODO: OrderMaps Configure Example (for setting options not available as constructor arguments)
			//maps.am_AddDataMap(...).am_Configure((map) => { map.ap_FunctionExpresion = "..."; });
		  
			return maps;
		}

	}
}