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
using BOS.OrderItemDataEntity;

namespace BOS.CustomerDataMaps
{
	public class CustomerMaps : AB_DataMaps
	{
		private const string YD1CTableName = "YD1C";
        private const string YD1CParentAlias = "ParentAlias";
        private const string SubCustomerCTE = "SubCustomerCTE";
        private const string ShippingAddressCTE = "ShippingAddressCTE";
        private const string OrdersItems = "OrderItems";
        private const string OrdersCTE = "OrdersCTE";
        private const string OrderedProductsCTE = "OrderedProductsCTE";
        private const string LastOrderCTE = "LastOrderCTE";

        public CustomerMaps() : base() { }
		public CustomerMaps(string qualifier) : base(qualifier) { }

        /// <summary>
        /// Loads maps to join two database files.
        /// </summary>
        /// 
        public override AB_CommonTableExpressionDictionary am_LoadCommonTableExpressions()
        {
            var ctes = new AB_CommonTableExpressionDictionary();

            ctes.am_AddCTE(SubCustomerCTE, @"SELECT YD1CPTID,
												 COUNT(*) AS ""SubCustomerCount"",
												 CLOB(LISTAGG(RTRIM(YD1CNM), ', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY YD1CNM), 5000) AS ""SubCustomerList""
											 FROM YD1C
											 GROUP BY YD1CPTID");
            ctes.am_AddCTE(ShippingAddressCTE, @"SELECT YD1S1CID, 
													 COUNT(*) AS ""ShippingAddressCount""
													,CLOB(LISTAGG(""YD1SIID"" CONCAT '-' CONCAT TRIM(""YD1SNM""),', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY ""YD1SNM""),5000) AS ""ShippingAddressList""
												 FROM YD1S
												 GROUP BY YD1S1CID");

			ctes.am_AddCTE(OrdersCTE, $@"SELECT ""YD1O1CID""
                                            ,MIN(""YD1O"".""YD1ODT"") AS ""FirstOrderDate""
                                            ,MIN(""YD1O"".""YD1OTM"") AS ""FirstOrderTime""
                                            ,COUNT(*) AS ""OrderCount""
                                            ,SUM(CASE WHEN ""YD1O"".""YD1OST"" IN('Open', 'New', 'Pending') THEN 1 ELSE 0 END) AS ""IncompleteOrderCount""
                                            ,SUM(CASE WHEN ""{OrdersItems}"".""OrderDiscount"" > 0 THEN 1 ELSE 0 END) AS ""DiscountedOrderCount""
                                            ,DECIMAL(MAX(""{OrdersItems}"".""OrderSubtotal""),11,2) AS ""HighestOrderSubtotal""
                                            ,DECIMAL(MAX(""{OrdersItems}"".""OrderDiscount""),11,2) AS ""HighestDiscount""
                                            ,DECIMAL(MAX(""{OrdersItems}"".""OrderTotal""),11,2) AS ""HighestOrderTotal""
                                            ,DECIMAL(AVG(""{OrdersItems}"".""OrderSubtotal""),11,2) AS ""AverageOrderSubtotal""
                                            ,DECIMAL(AVG(""{OrdersItems}"".""OrderDiscount""),11,2) AS ""AverageDiscount""
                                            ,DECIMAL(AVG(""{OrdersItems}"".""OrderTotal""),11,2) AS ""AverageOrderTotal""
                                            ,DECIMAL(MIN(""{OrdersItems}"".""OrderSubtotal""),11,2) AS ""LowestOrderSubtotal""
                                            ,DECIMAL(MIN(""{OrdersItems}"".""OrderDiscount""),11,2) AS ""LowestDiscount""
                                            ,DECIMAL(MIN(""{OrdersItems}"".""OrderTotal""),11,2) AS ""LowestOrderTotal""
                                        FROM ""YD1O""
                                        LEFT JOIN (SELECT ""YD1I1OID""
												,DECIMAL(SUM(""YD1IQT"" * ""YD1IPRUN""),11,2) AS ""OrderSubtotal""
												,DECIMAL(SUM(""YD1IQT"" * ""YD1IPRUN"" * (""YD1IDSPC"" * 0.01)),11,2) AS ""OrderDiscount""
												,DECIMAL(SUM(""YD1IQT"" * ""YD1IPRUN"" * (1 - (""YD1IDSPC"" * 0.01))),11,2) AS ""OrderTotal""
											FROM ""YD1I""
											GROUP BY ""YD1I1OID"") ""{OrdersItems}"" ON ""{OrdersItems}"".""YD1I1OID"" = ""YD1OIID""
                                        GROUP BY ""YD1O1CID""");

            ctes.am_AddCTE(OrderedProductsCTE, @"SELECT YD1O1CID
                                                    ,COUNT(*) AS ""OrderedItemsCount""
                                                    ,COUNT(DISTINCT YD1I1PID) AS ""ProductsOrderedCount""
                                                FROM YD1O
                                                RIGHT JOIN YD1I ON YD1I.YD1I1OID = YD1O.YD1OIID
                                                LEFT JOIN YD1P ON YD1P.YD1PIID = YD1I.YD1I1PID
                                                GROUP BY YD1O1CID");

			ctes.am_AddCTE(LastOrderCTE, $@"SELECT ""{YD1CTableName}"".""YD1CIID""
                                                ,""LastOrder"".*
                                            FROM ""{YD1CTableName}""
                                            LEFT JOIN LATERAL (
                                                SELECT ""YD1O"".""YD1OIID"" AS ""LastOrderID""
                                                        ,""YD1O"".""YD1ODT"" AS ""LastOrderDate""
                                                        ,""YD1O"".""YD1OTM"" AS ""LastOrderTime""
                                                        ,""YD1O"".""YD1OST"" AS ""LastOrderStatus""
                                                        ,""YD1O"".""YD1O1SID"" AS ""LastUsedShippingAddressID""
                                                        ,""YD1S"".""YD1SNM"" AS ""LastUsedShippingAddressName""
                                                        ,TRIM(""YD1S"".""YD1SSHA1"")
                                                           CONCAT CASE ""YD1S"".""YD1SSHA2"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""YD1S"".""YD1SSHA2"") END
                                                           CONCAT CASE ""YD1S"".""YD1SSHA3"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""YD1S"".""YD1SSHA3"") END
                                                           CONCAT CASE ""YD1S"".""YD1SSHPC"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""YD1S"".""YD1SSHPC"") END
                                                           CONCAT CASE ""YD1S"".""YD1SSHCY"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""YD1S"".""YD1SSHCY"") END
                                                           AS ""LastUsedShippingAddressLine""
                                                    FROM ""YD1O""
                                                        RIGHT JOIN ""YD1I"" ON ""YD1I1OID"" = ""YD1O"".""YD1OIID""
                                                        LEFT JOIN ""YD1S"" ON ""YD1S"".""YD1SIID"" = ""YD1O"".""YD1O1SID""
                                                    WHERE ""YD1O"".""YD1O1CID"" = ""YD1C"".""YD1CIID""
                                                    ORDER BY ""YD1O"".""YD1ODT"" DESC
                                                            ,""YD1O"".""YD1OTM"" DESC
                                                            ,""YD1O"".""YD1OIID"" DESC
                                                    FETCH FIRST 1 ROW ONLY
                                                ) AS ""LastOrder"" ON 'X'='X'");

			return ctes;
        }

        public override Dictionary<string, AB_RelationshipMap> am_LoadRelationshipMaps()
		{            
			var relationshipMap = new AB_RelationshipMapsDictionary(ap_PrimaryTable);

			relationshipMap.am_AddRelationshipMap(YD1CTableName, joinTableAlias: YD1CParentAlias, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField: "YD1CPTID", joinTableField: "YD1CIID");

			relationshipMap.am_AddRelationshipMap(SubCustomerCTE, useDistinctJoins: false)
				.am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1CPTID");

			relationshipMap.am_AddRelationshipMap(ShippingAddressCTE, useDistinctJoins: false)
                .am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1S1CID");

            relationshipMap.am_AddRelationshipMap(OrdersCTE, useDistinctJoins: false)
                .am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1O1CID");

			relationshipMap.am_AddRelationshipMap(OrderedProductsCTE, useDistinctJoins: false)
                .am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1O1CID");

			relationshipMap.am_AddRelationshipMap(LastOrderCTE, useDistinctJoins: false)
                .am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1CIID");

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

			#region Additional Fields

			maps.am_AddDataMap("ContactFullName", CustomerEntity.ContactFullNameProperty, isVirtual: true);

			maps.am_AddDataMap("ContactFullNameSearch", CustomerEntity.ContactFullNameSearchProperty).am_Configure(map =>
				map.ap_FunctionExpression = $@"(TRIM(""{YD1CTableName}"".""YD1CCNFN"")
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CCNMN"" WHEN '' THEN '' ELSE ' ' CONCAT TRIM(""{YD1CTableName}"".""YD1CCNMN"") END
                                               CONCAT ' ' CONCAT TRIM(""{YD1CTableName}"".""YD1CCNLN"")
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CCNNN"" WHEN '' THEN '' ELSE ' ""' CONCAT TRIM(""{YD1CTableName}"".""YD1CCNNN"") CONCAT '""' END)
                                               AS ""ContactFullNameSearch""");

			maps.am_AddDataMap("BillingAddressLine", CustomerEntity.BillingAddressLineProperty, isVirtual: true);
			maps.am_AddDataMap("BillingAddressBlock", CustomerEntity.BillingAddressBlockProperty, isVirtual: true);

			maps.am_AddDataMap("BillingAddressLineSearch", CustomerEntity.BillingAddressLineSearchProperty).am_Configure(map =>
				map.ap_FunctionExpression = $@"(""{YD1CTableName}"".""YD1CBLA1""
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLA2"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CTableName}"".""YD1CBLA2"") END
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLA3"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CTableName}"".""YD1CBLA3"") END
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLPC"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1CTableName}"".""YD1CBLPC"") END
                                               CONCAT CASE ""{YD1CTableName}"".""YD1CBLCY"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CTableName}"".""YD1CBLCY"") END)
                                               AS ""BillingAddressLineSearch""");

			#endregion

			#region Parent Customer Join

			maps.am_AddDataMap("YD1CNM", CustomerEntity.ParentNameProperty, targetTable: YD1CParentAlias);
			maps.am_AddDataMap("ParentCustomerContactFullName", CustomerEntity.ParentCustomerContactFullNameProperty).am_Configure(map =>
			{
				map.am_RequiresTableNames(YD1CParentAlias);
				map.ap_FunctionExpression = $@"(TRIM(""{YD1CParentAlias}"".""YD1CCNFN"")
                                               CONCAT CASE ""{YD1CParentAlias}"".""YD1CCNMN"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1CParentAlias}"".""YD1CCNMN"") END
                                               CONCAT SPACE(1) CONCAT TRIM(""{YD1CParentAlias}"".""YD1CCNLN"")
                                               CONCAT CASE ""{YD1CParentAlias}"".""YD1CCNNN"" WHEN '' THEN '' ELSE ' ""' CONCAT TRIM(""{YD1CParentAlias}"".""YD1CCNNN"") CONCAT '""' END)
                                               AS ""ParentCustomerContactFullname""";
			});
			maps.am_AddDataMap("YD1CTL", CustomerEntity.ParentCustomerTelephoneProperty, targetTable: YD1CParentAlias);
			maps.am_AddDataMap("ParentCustomerAddressLine", CustomerEntity.ParentCustomerAddressLineProperty).am_Configure(map =>
			{
				map.am_RequiresTableNames(YD1CParentAlias);
				map.ap_FunctionExpression = $@"(TRIM(""{YD1CParentAlias}"".""YD1CBLA1"")
                                               CONCAT CASE ""{YD1CParentAlias}"".""YD1CBLA2"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CParentAlias}"".""YD1CBLA2"") END
                                               CONCAT CASE ""{YD1CParentAlias}"".""YD1CBLA3"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CParentAlias}"".""YD1CBLA3"") END
                                               CONCAT CASE ""{YD1CParentAlias}"".""YD1CBLPC"" WHEN '' THEN '' ELSE SPACE(1) CONCAT TRIM(""{YD1CParentAlias}"".""YD1CBLPC"") END
                                               CONCAT CASE ""{YD1CParentAlias}"".""YD1CBLCY"" WHEN '' THEN '' ELSE ', ' CONCAT TRIM(""{YD1CParentAlias}"".""YD1CBLCY"") END)
                                               AS ""ParentCustomerAddressLine""";
			});

			#endregion

			#region Last Order Information

			maps.am_AddDataMap("LastOrderID", CustomerEntity.LastOrderIDProperty, targetTable: LastOrderCTE);
			maps.am_AddDataMap("LastOrderDate", CustomerEntity.LastOrderDateProperty, targetTable: LastOrderCTE);
			maps.am_AddDataMap("LastOrderTime", CustomerEntity.LastOrderTimeProperty, targetTable: LastOrderCTE);
			maps.am_AddDataMap("LastOrderStatus", CustomerEntity.LastOrderStatusProperty, targetTable: LastOrderCTE);
			maps.am_AddDataMap("LastUsedShippingAddressID", CustomerEntity.LastUsedShippingAddressIDProperty, targetTable: LastOrderCTE);
			maps.am_AddDataMap("LastUsedShippingAddressName", CustomerEntity.LastUsedShippingAddressNameProperty, targetTable: LastOrderCTE);
			maps.am_AddDataMap("LastUsedShippingAddressLine", CustomerEntity.LastUsedShippingAddressLineProperty, targetTable: LastOrderCTE);

			#endregion

			#region Sub-Customer Information

			maps.am_AddDataMap("IsParentCustomer", CustomerEntity.IsParentCustomerProperty, dbTrueEquivalent: "TRUE", dbFalseEquivalent: "FALSE").am_Configure(map =>
				map.ap_FunctionExpression = $@"(CASE WHEN ""{SubCustomerCTE}"".""SubCustomerCount"" > 0 THEN 'TRUE' ELSE 'FALSE' END) AS ""IsParentCustomer""");

			maps.am_AddDataMap("IsSubCustomer", CustomerEntity.IsSubCustomerProperty, dbTrueEquivalent: "TRUE", dbFalseEquivalent: "FALSE").am_Configure(map =>
			    map.ap_FunctionExpression = $@"(CASE WHEN ""{YD1CTableName}"".""YD1CPTID"" IS NULL OR ""{YD1CTableName}"".""YD1CPTID"" = 0 THEN 'FALSE' ELSE 'TRUE' END) AS ""IsSubCustomer""");

			maps.am_AddDataMap("SubCustomerCount", CustomerEntity.SubCustomerCountProperty, targetTable: SubCustomerCTE);
			maps.am_AddDataMap("SubCustomerList", CustomerEntity.SubCustomerListProperty, targetTable: SubCustomerCTE);

			#endregion

			#region Shipping Address Information

			maps.am_AddDataMap("ShippingAddressCount", CustomerEntity.ShippingAddressCountProperty, targetTable: ShippingAddressCTE);
			maps.am_AddDataMap("ShippingAddressList", CustomerEntity.ShippingAddressListProperty, targetTable: ShippingAddressCTE);

			#endregion

			#region Orders Information

			maps.am_AddDataMap("FirstOrderDate", CustomerEntity.FirstOrderDateProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("FirstOrderTime", CustomerEntity.FirstOrderTimeProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("OrderCount", CustomerEntity.OrderCountProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("IncompleteOrderCount", CustomerEntity.IncompleteOrderCountProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("DiscountedOrderCount", CustomerEntity.DiscountedOrderCountProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("HighestOrderSubtotal", CustomerEntity.HighestOrderSubtotalProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("HighestDiscount", CustomerEntity.HighestDiscountProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("HighestOrderTotal", CustomerEntity.HighestOrderTotalProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("AverageOrderSubtotal", CustomerEntity.AverageOrderSubtotalProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("AverageDiscount", CustomerEntity.AverageDiscountProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("AverageOrderTotal", CustomerEntity.AverageOrderTotalProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("LowestOrderSubtotal", CustomerEntity.LowestOrderSubtotalProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("LowestDiscount", CustomerEntity.LowestDiscountProperty, targetTable: OrdersCTE);
			maps.am_AddDataMap("LowestOrderTotal", CustomerEntity.LowestOrderTotalProperty, targetTable: OrdersCTE);

			#endregion

			#region Order Products Information

			maps.am_AddDataMap("OrderedItemsCount", CustomerEntity.OrderedItemsCountProperty, targetTable: OrderedProductsCTE);
			maps.am_AddDataMap("ProductsOrderedCount", CustomerEntity.ProductsOrderedCountProperty, targetTable: OrderedProductsCTE);

			#endregion

			#region Audit Stamps

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

			maps.am_AddDataMap("CreateDateTime", CustomerEntity.CreateDateTimeProperty, isVirtual: true);
			maps.am_AddDataMap("LastChangeDateTime", CustomerEntity.LastChangeDateTimeProperty, isVirtual: true);

			#endregion

			return maps;
		}

	}
}