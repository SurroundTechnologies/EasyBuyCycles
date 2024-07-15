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
		private const string YD1ITableName = "YD1I";

		private const string OrderItemsCTEName = "OrderItemsCTE";

		public OrderMaps() : base() { }
		public OrderMaps(string qualifier) : base(qualifier) { }

		public override AB_CommonTableExpressionDictionary am_LoadCommonTableExpressions()
		{
			var ctes = new AB_CommonTableExpressionDictionary();

			ctes.am_AddCTE(OrderItemsCTEName, $@"SELECT ""YD1I1OID""
													,COUNT(*) AS ""OrderItemCount""
													,DECIMAL(SUM(""YD1IQT"" * ""YD1IPRUN""),11,2) AS ""OrderTotal""
													,DECIMAL(SUM(""YD1IQT"" * ""YD1IPRUN"" * (""YD1IDSPC"" * 0.01)),11,2) AS ""OrderDiscount""
													,DECIMAL(SUM(""YD1IQT"" * ""YD1IPRUN"" * (1 - (""YD1IDSPC"" * 0.01))),11,2) AS ""OrderDiscountedTotal""
												FROM ""{YD1ITableName}""
												GROUP BY ""YD1I1OID""");

			return ctes;
		}

		/// <summary>
		/// Loads maps to join two database files.
		/// </summary>
		public override Dictionary<string, AB_RelationshipMap> am_LoadRelationshipMaps()
		{            
			var relationshipMap = new AB_RelationshipMapsDictionary(ap_PrimaryTable);

			relationshipMap.am_AddRelationshipMap(YD1CTableName, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField: "YD1O1CID", joinTableField: "YD1CIID");

			relationshipMap.am_AddRelationshipMap(YD1STableName, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField:"YD1O1SID", joinTableField:"YD1SIID");

			relationshipMap.am_AddRelationshipMap(OrderItemsCTEName, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField: "YD1OIID", joinTableField: "YD1I1OID");

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
			maps.am_AddDataMap("YD1ODT", OrderEntity.OrderDateProperty, databaseFieldType: AB_EntityFieldType.Date);
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

			#region Audit Stamps

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

			#endregion

			#region Customer Join

			maps.am_AddDataMap("YD1CNM", OrderEntity.CustomerNameProperty, targetTable: YD1CTableName);
			maps.am_AddDataMap("CustomerContactFullName", OrderEntity.CustomerContactFullNameProperty).am_Configure(map =>
			{
				map.am_RequiresTableNames(YD1CTableName);
				map.ap_FunctionExpression = $@"(TRIM(""{YD1CTableName}"".""YD1CCNFN"")
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CCNMN"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1CTableName}"".""YD1CCNMN"") END
                                               CONCAT SPACE(1) CONCAT TRIM(""{YD1CTableName}"".""YD1CCNLN"")
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CCNNN"" WHEN '' THEN '' ELSE ' ""' CONCAT TRIM(""{YD1CTableName}"".""YD1CCNNN"") CONCAT '""' END)
                                               AS ""CustomerContactFullname""";
			});
			maps.am_AddDataMap("YD1CTL", OrderEntity.CustomerTelephoneProperty, targetTable: YD1CTableName);
			maps.am_AddDataMap("CustomerBillingAddressLine", OrderEntity.CustomerBillingAddressLineProperty).am_Configure(map =>
			{
				map.am_RequiresTableNames(YD1CTableName);
				map.ap_FunctionExpression = $@"(TRIM(""{YD1CTableName}"".""YD1CBLA1"")
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLA2"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CTableName}"".""YD1CBLA2"") END
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLA3"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CTableName}"".""YD1CBLA3"") END
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLPC"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1CTableName}"".""YD1CBLPC"") END
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLCY"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CTableName}"".""YD1CBLCY"") END)
                                               AS ""CustomerBillingAddressLineProperty""";
			});

			#endregion

			#region Shipping Address Join

			maps.am_AddDataMap("YD1SNM", OrderEntity.ShippingAddressNameProperty, targetTable: YD1STableName);
			maps.am_AddDataMap("ShippingAddressContactFullName", OrderEntity.ShippingAddressContactFullNameProperty).am_Configure(map =>
			{
				map.am_RequiresTableNames(YD1STableName);
				map.ap_FunctionExpression = $@"(TRIM(""{YD1STableName}"".""YD1SCNFN"")
                                               CONCAT CASE ""{YD1STableName}"".""YD1SCNMN"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1STableName}"".""YD1SCNMN"") END
                                               CONCAT SPACE(1) CONCAT TRIM(""{YD1STableName}"".""YD1SCNLN"")
                                               CONCAT CASE ""{YD1STableName}"".""YD1SCNNN"" WHEN '' THEN '' ELSE ' ""' CONCAT TRIM(""{YD1STableName}"".""YD1SCNNN"") CONCAT '""' END)
                                               AS ""ShippingAddressContactFullName""";
			});
			maps.am_AddDataMap("YD1STL", OrderEntity.ShippingAddressTelephoneProperty, targetTable: YD1STableName);
			maps.am_AddDataMap("ShippingAddressLine", OrderEntity.ShippingAddressLineProperty).am_Configure(map =>
			{
				map.am_RequiresTableNames(YD1STableName);
				map.ap_FunctionExpression = $@"(TRIM(""{YD1STableName}"".""YD1SSHA1"")
                                               CONCAT CASE ""{YD1STableName}"".""YD1SSHA2"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1STableName}"".""YD1SSHA2"") END
                                               CONCAT CASE ""{YD1STableName}"".""YD1SSHA3"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1STableName}"".""YD1SSHA3"") END
                                               CONCAT CASE ""{YD1STableName}"".""YD1SSHPC"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1STableName}"".""YD1SSHPC"") END
                                               CONCAT CASE ""{YD1STableName}"".""YD1SSHCY"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1STableName}"".""YD1SSHCY"") END)
                                               AS ""ShippingAddressLine""";
			});

			#endregion

			#region Order Item Fields

			maps.am_AddDataMap("OrderItemCount", OrderEntity.OrderItemCountProperty, targetTable: OrderItemsCTEName);
			maps.am_AddDataMap("OrderTotal", OrderEntity.OrderTotalProperty, targetTable: OrderItemsCTEName);
			maps.am_AddDataMap("OrderDiscount", OrderEntity.OrderDiscountProperty, targetTable: OrderItemsCTEName);
			maps.am_AddDataMap("OrderDiscountedTotal", OrderEntity.OrderDiscountedTotalProperty, targetTable: OrderItemsCTEName);

			#endregion

			return maps;
		}

	}
}