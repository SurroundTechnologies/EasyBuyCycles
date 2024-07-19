//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.DataMaps.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Collections.Generic;
using A4DN.Core.BOS.Base;
using SharedSystemProperties = BOS.OrderManagement.Shared.Properties;
using BOS.ProductDataEntity;

namespace BOS.ProductDataMaps
{
	public class ProductMaps : AB_DataMaps
	{
		private const string YD1PTableName = "YD1P";
        // CTE Names
        private const string OrderedProductsCTE = "ORDEREDPRODUCTSCTE";
        private const string OrderItemsCTE = "OrderItemsCTE";

        public const string CAT_CustomerOrderedProducts = "Category_CustomerOrderedProducts";

        public ProductMaps() : base() { }
        public ProductMaps(string qualifier) : base(qualifier) { }

        /// <summary>
        /// Loads maps to join two database files.
        /// </summary>
        public override AB_CommonTableExpressionDictionary am_LoadCommonTableExpressions()
        {
            var ctes = new AB_CommonTableExpressionDictionary();

            ctes.am_AddCTE(OrderItemsCTE, @"SELECT ""YD1I1PID""
                                                ,COUNT(DISTINCT ""YD1O"".""YD1O1CID"") AS ""CustomerCount""
                                                ,COUNT(DISTINCT ""YD1I"".""YD1I1OID"") AS ""OrderCount""
                                                ,COUNT(*) AS ""OrderItemCount""
                                                ,DECIMAL(AVG(""YD1I"".""YD1IQT""),6,2) AS ""AverageOrderQuantity""
                                                ,MIN(""YD1I"".""YD1IQT"") AS ""SmallestOrderQuantity""
                                                ,MAX(""YD1I"".""YD1IQT"") AS ""LargestOrderQuantity""
                                                ,DECIMAL(AVG(""YD1I"".""YD1IPRUN""),8,2) AS ""AverageOrderUnitPrice""
                                                ,MIN(""YD1I"".""YD1IPRUN"") AS ""LowestOrderUnitPrice""
                                                ,MAX(""YD1I"".""YD1IPRUN"") AS ""HighestOrderUnitPrice""
                                                ,MIN(""YD1O"".""YD1ODT"") AS ""FirstOrderDate""
                                                ,MIN(""YD1O"".""YD1OTM"") AS ""FirstOrderTime""
                                                ,MAX(""YD1O"".""YD1ODT"") AS ""LastOrderDate""
                                                ,MAX(""YD1O"".""YD1OTM"") AS ""LastOrderTime""
                                            FROM ""YD1I""
                                                LEFT JOIN ""YD1O"" ON ""YD1O"".""YD1OIID"" = ""YD1I"".""YD1I1OID""
                                            GROUP BY ""YD1I1PID""");

            ctes.am_AddCTE(OrderedProductsCTE, @"SELECT DISTINCT YD1I1PID,
                                             YD1O.YD1O1CID AS CUSTOMERINTERNALID
                                          FROM YD1I
                                          LEFT JOIN YD1O ON YD1O.YD1OIID = YD1I.YD1I1OID");

            return ctes;
        }

        /// <summary>
        /// Loads maps to join two database files.
        /// </summary>
        public override Dictionary<string, AB_RelationshipMap> am_LoadRelationshipMaps()
		{            
			var relationshipMap = new AB_RelationshipMapsDictionary(ap_PrimaryTable);

            relationshipMap.am_AddRelationshipMap(OrderItemsCTE, useDistinctJoins: false)
                .am_JoinWhere(primaryTableField: "YD1PIID", joinTableField: "YD1I1PID");

            relationshipMap.am_AddRelationshipMap(OrderedProductsCTE, useDistinctJoins: false)
                .am_JoinWhere(primaryTableField: "YD1PIID", joinTableField: "YD1I1PID");

            return relationshipMap;
		}
		

		/// <summary>
		/// Defines the maps for the Data Source Field Names and Entity Properties for module.
		/// </summary>
		public override Dictionary<string,AB_DataSourcePropertyReference> am_LoadFieldMaps(string qualifier)
		{	
			// Set the Primary File Name, Foreign fields will have to be mapped on a case-by-case basis
			ap_PrimaryTable = YD1PTableName;
			//Create a dictionary to hold the maps
			var maps = new AB_DataMapsDictionary(ap_PrimaryTable, qualifier);
            maps.ap_DefaultDataCategories = new List<string>() { "*ALL", CAT_CustomerOrderedProducts };
		 
			maps.am_AddDataMap("YD1PIID", ProductEntity.ProductInternalIDProperty);
			maps.am_AddDataMap("YD1PCD", ProductEntity.CodeProperty);
			maps.am_AddDataMap("YD1PNM", ProductEntity.NameProperty);
			maps.am_AddDataMap("YD1PDS", ProductEntity.DescriptionProperty);
			maps.am_AddDataMap("YD1PCT", ProductEntity.CategoryProperty);
			maps.am_AddDataMap("YD1PSTCS", ProductEntity.StandardCostProperty);
			maps.am_AddDataMap("YD1PLSPR", ProductEntity.ListPriceProperty);
			maps.am_AddDataMap("YD1PROLV", ProductEntity.ReorderLevelProperty);
			maps.am_AddDataMap("YD1PTGLV", ProductEntity.TargetLevelProperty);
			maps.am_AddDataMap("YD1PMRQT", ProductEntity.MinimumReorderQuantityProperty);
			maps.am_AddDataMap("YD1PDC", ProductEntity.DiscontinuedProperty, dbTrueEquivalent: "Y", dbFalseEquivalent: "N");
			maps.am_AddDataMap("YD1PM1", ProductEntity.MemoProperty);
			maps.am_AddDataMap("YD1PIMPT", ProductEntity.ImagePathProperty);

			#region Audit Stamps

			maps.am_AddDataMap("YD1PCRDT", ProductEntity.CreateDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1PCRTM", ProductEntity.CreateTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1PCRUS", ProductEntity.CreateUserProperty);
			maps.am_AddDataMap("YD1PCRJB", ProductEntity.CreateJobProperty);
			maps.am_AddDataMap("YD1PCRJN", ProductEntity.CreateJobNumberProperty);
			maps.am_AddDataMap("YD1PLCDT", ProductEntity.LastChangeDateProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1PLCTM", ProductEntity.LastChangeTimeProperty, databaseFieldType: AB_EntityFieldType.Decimal);
			maps.am_AddDataMap("YD1PLCUS", ProductEntity.LastChangeUserProperty);
			maps.am_AddDataMap("YD1PLCJB", ProductEntity.LastChangeJobProperty);
			maps.am_AddDataMap("YD1PLCJN", ProductEntity.LastChangeJobNumberProperty);

			maps.am_AddDataMap("CreateDateTime", ProductEntity.CreateDateTimeProperty, isVirtual: true);
			maps.am_AddDataMap("LastChangeDateTime", ProductEntity.LastChangeDateTimeProperty, isVirtual: true);

			#endregion

			#region Virtual Fields

			maps.am_AddDataMap("Margin", ProductEntity.MarginProperty).am_Configure(map =>
                map.ap_FunctionExpression = $@"(""YD1P"".""YD1PLSPR"" - ""YD1P"".""YD1PSTCS"") AS ""Margin""");

			#endregion

			#region Order Item Fields

			maps.am_AddDataMap("CustomerCount", ProductEntity.CustomerCountProperty, targetTable: OrderItemsCTE);
			maps.am_AddDataMap("OrderCount", ProductEntity.OrderCountProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("OrderItemCount", ProductEntity.OrderItemCountProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("AverageOrderQuantity", ProductEntity.AverageOrderQuantityProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("SmallestOrderQuantity", ProductEntity.SmallestOrderQuantityProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LargestOrderQuantity", ProductEntity.LargestOrderQuantityProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("AverageOrderUnitPrice", ProductEntity.AverageOrderUnitPriceProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LowestOrderUnitPrice", ProductEntity.LowestOrderUnitPriceProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("HighestOrderUnitPrice", ProductEntity.HighestOrderUnitPriceProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("FirstOrderDate", ProductEntity.FirstOrderDateProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("FirstOrderTime", ProductEntity.FirstOrderTimeProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LastOrderDate", ProductEntity.LastOrderDateProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LastOrderTime", ProductEntity.LastOrderTimeProperty, targetTable: OrderItemsCTE);

			maps.am_AddDataMap("FirstOrderDateTime", ProductEntity.FirstOrderDateTimeProperty, isVirtual: true);
			maps.am_AddDataMap("LastOrderDateTime", ProductEntity.LastOrderDateTimeProperty, isVirtual: true);

			#endregion

			maps.am_AddDataMap("CUSTOMERINTERNALID", ProductEntity.CustomerInternalIDProperty, targetTable: OrderedProductsCTE, dataCategories: new List<string> { CAT_CustomerOrderedProducts });

            return maps;
		}

	}
}