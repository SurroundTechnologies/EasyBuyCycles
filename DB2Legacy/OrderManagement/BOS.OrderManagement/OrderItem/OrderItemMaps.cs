//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DataMaps.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Collections.Generic;
using A4DN.Core.BOS.Base;
using SharedSystemProperties = BOS.OrderManagement.Shared.Properties;
using BOS.OrderItemDataEntity;

namespace BOS.OrderItemDataMaps
{
	public class OrderItemMaps : AB_DataMaps
	{
		private const string YD1ITableName = "YD1I";
		private const string YD1OTableName = "YD1O";
		private const string YD1PTableName = "YD1P";

		public OrderItemMaps() : base() { }
		public OrderItemMaps(string qualifier) : base(qualifier) { }
		  
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
		 
            //relationshipMap.am_AddRelationshipMap(YD1OTableName, useDistinctJoins: false)
			//.am_JoinWhere(primaryTableField:"YD1I1OID", joinTableField:"YD1OIID");

            relationshipMap.am_AddRelationshipMap(YD1PTableName, useDistinctJoins: false)
			.am_JoinWhere(primaryTableField:"YD1I1PID", joinTableField:"YD1PIID");

			return relationshipMap;
		}
		

		/// <summary>
		/// Defines the maps for the Data Source Field Names and Entity Properties for module.
		/// </summary>
		public override Dictionary<string,AB_DataSourcePropertyReference> am_LoadFieldMaps(string qualifier)
		{	
			// Set the Primary File Name, Foreign fields will have to be mapped on a case-by-case basis
			ap_PrimaryTable = YD1ITableName;
			//Create a dictionary to hold the maps
			var maps = new AB_DataMapsDictionary(ap_PrimaryTable, qualifier);
		 
			maps.am_AddDataMap("YD1IIID", OrderItemEntity.OrderItemInternalIDProperty);
			maps.am_AddDataMap("YD1I1OID", OrderItemEntity.OrderInternalIDProperty);
			maps.am_AddDataMap("YD1I1PID", OrderItemEntity.ProductInternalIDProperty);
			maps.am_AddDataMap("YD1IQT", OrderItemEntity.QuantityProperty);
			maps.am_AddDataMap("YD1IPRUN", OrderItemEntity.UnitPriceProperty);
			maps.am_AddDataMap("YD1IDSPC", OrderItemEntity.DiscountPercentProperty);
			maps.am_AddDataMap("YD1IM1", OrderItemEntity.MemoProperty);
			maps.am_AddDataMap("YD1ICRDT", OrderItemEntity.CreateDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1ICRTM", OrderItemEntity.CreateTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1ICRUS", OrderItemEntity.CreateUserProperty);
			maps.am_AddDataMap("YD1ICRJB", OrderItemEntity.CreateJobProperty);
			maps.am_AddDataMap("YD1ICRJN", OrderItemEntity.CreateJobNumberProperty);
			maps.am_AddDataMap("YD1ILCDT", OrderItemEntity.LastChangeDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1ILCTM", OrderItemEntity.LastChangeTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1ILCUS", OrderItemEntity.LastChangeUserProperty);
			maps.am_AddDataMap("YD1ILCJB", OrderItemEntity.LastChangeJobProperty);
			maps.am_AddDataMap("YD1ILCJN", OrderItemEntity.LastChangeJobNumberProperty);
			maps.am_AddDataMap(string.Format("{0}.{1}", YD1PTableName, "YD1PNM"), OrderItemEntity.ProductNameProperty, targetTable: YD1PTableName);

			//TODO: OrderItemMaps Real Field Example
			//maps.am_AddDataMap("<Field Name>", OrderItemEntity.<Property Name>);
			//TODO: OrderItemMaps Virtual Field Example
			//maps.am_AddDataMap("<Field Name>", OrderItemEntity.<Property Name>, isVirtual: true);
			//TODO: OrderItemMaps Foreign Field Example
			//maps.am_AddDataMap(string.Format("{0}.{1}", "<Target Table Name>", "<Field Name>"), OrderItemEntity.<Property Name>, targetTable: "<Target Table Name>"); 
			//TODO: OrderItemMaps Configure Example (for setting options not available as constructor arguments)
			//maps.am_AddDataMap(...).am_Configure((map) => { map.ap_FunctionExpresion = "..."; });
		  
			return maps;
		}

	}
}