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
		private const string YD1CTableName = "YD1C";

		public OrderItemMaps() : base() { }
		public OrderItemMaps(string qualifier) : base(qualifier) { }
		  
		/// <summary>
		/// Loads maps to join two database files.
		/// </summary>
		public override Dictionary<string, AB_RelationshipMap> am_LoadRelationshipMaps()
		{            
			var relationshipMap = new AB_RelationshipMapsDictionary(ap_PrimaryTable);
		 

            relationshipMap.am_AddRelationshipMap(YD1PTableName, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField:"YD1I1PID", joinTableField:"YD1PIID");

            relationshipMap.am_AddRelationshipMap(YD1OTableName, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField: "YD1I1OID", joinTableField: "YD1OIID");

			relationshipMap.am_AddRelationshipMap(YD1CTableName, primaryTable: YD1OTableName, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField: "YD1O1CID", joinTableField: "YD1CIID");


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

			#region Virtual Fields

			maps.am_AddDataMap("UnitDiscount", OrderItemEntity.UnitDiscountProperty, isVirtual: true);
			maps.am_AddDataMap("OrderPrice", OrderItemEntity.OrderPriceProperty, isVirtual: true);
			maps.am_AddDataMap("OrderDiscount", OrderItemEntity.OrderDiscountProperty, isVirtual: true);
			maps.am_AddDataMap("OrderTotal", OrderItemEntity.OrderTotalProperty, isVirtual: true);

			#endregion

			#region Order Fields

			maps.am_AddDataMap("YD1ODT", OrderItemEntity.OrderDateProperty, targetTable: YD1OTableName);
			maps.am_AddDataMap("YD1OTM", OrderItemEntity.OrderTimeProperty, targetTable: YD1OTableName);
			maps.am_AddDataMap("YD1CNM", OrderItemEntity.CustomerNameProperty, targetTable: YD1CTableName);
			maps.am_AddDataMap("YD1OPONO", OrderItemEntity.PurchaseOrderNumberIDProperty, targetTable: YD1OTableName);

			#endregion

			#region Product Fields

			maps.am_AddDataMap("YD1PCD", OrderItemEntity.ProductCodeProperty, targetTable: YD1PTableName);
			maps.am_AddDataMap("YD1PNM", OrderItemEntity.ProductNameProperty, targetTable: YD1PTableName);
			maps.am_AddDataMap("YD1PCT", OrderItemEntity.ProductCategoryProperty, targetTable: YD1PTableName);
			maps.am_AddDataMap("YD1PLSPR", OrderItemEntity.ProductListPriceProperty, targetTable: YD1PTableName);
			maps.am_AddDataMap("YD1PDS", OrderItemEntity.ProductDescriptionProperty, targetTable: YD1PTableName);
			maps.am_AddDataMap("YD1PIMPT", OrderItemEntity.ProductImagePathProperty, targetTable: YD1PTableName);

			#endregion

			#region Audit Stamps

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

			maps.am_AddDataMap("CreateDateTime", OrderItemEntity.CreateDateTimeProperty, isVirtual: true);
			maps.am_AddDataMap("LastChangeDateTime", OrderItemEntity.LastChangeDateTimeProperty, isVirtual: true);

			#endregion

			return maps;
		}

	}
}