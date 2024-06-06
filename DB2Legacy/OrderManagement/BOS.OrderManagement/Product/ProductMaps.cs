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
        private const string OrderItemsCTE = "ORDERITEMS";

        public ProductMaps() : base() { }
        public ProductMaps(string qualifier) : base(qualifier) { }

        /// <summary>
        /// Loads maps to join two database files.
        /// </summary>
        public override AB_CommonTableExpressionDictionary am_LoadCommonTableExpressions()
        {
            var ctes = new AB_CommonTableExpressionDictionary();

            // Add CTE's here
            ctes.am_AddCTE(OrderItemsCTE, @"SELECT YD1I1PID,
                                    COUNT(DISTINCT YD1I.YD1IIID) AS ORDERCOUNT,
                                    COUNT(*) AS ORDERITEMCOUNT,
                                    DECIMAL(AVG(YD1I.YD1IQT), 6, 2) AS AVERAGEORDERQUANTITY,
                                    MIN(YD1I.YD1IQT) AS SMALLESTORDERQUANTITY,
                                    MAX(YD1I.YD1IQT) AS LARGESTORDERQUANTITY,
                                    DECIMAL(AVG(YD1I.YD1IPRUN), 8, 2) AS AVERAGEORDERUNITPRICE,
                                    MIN(YD1I.YD1IPRUN) AS LOWESTORDERUNITPRICE,
                                    MAX(YD1I.YD1IPRUN) AS HIGHESTORDERUNITPRICE,
                                    MAX(YD1O.YD1ODT) AS LASTORDERDATETIME,
                                    COUNT(DISTINCT YD1O.YD1O1CID) AS CUSTOMERCOUNT
                                 FROM YD1I
                                 LEFT JOIN YD1O ON YD1O.YD1OIID = YD1I.YD1IIID
                                 GROUP BY YD1I1PID");

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

            // TODO: Table Relationships Step 1 - Define and relationships and join conditions for each file and add relationships (Change 0 to 1, 2, ... n for each new file map)
            // AB_RelationshipMap map0 = new AB_RelationshipMap("PrimaryFile", "SecondaryFile", JoinType.LeftOuter);  // Create a map to a single file
            // TODO: Table Relationships Step 2 - Add Joins for each relationship
            // Two field Relationship 
            // map0.ap_JoinConditions.Add(new AB_JoinCondition(new AB_QueryField("FileName", "FieldName"), "=", new AB_QueryField("FileName", "FieldName")));
            // Single Field to Constant Relationship
            // map0.ap_JoinConditions.Add(new AB_JoinCondition(new AB_QueryField("FileName", "FieldName"), "=", new AB_QueryConstant("ConstantValue")));
            // relationshipMap.Add("Y06T", map0); // Add to the relationship Dictionary keyed by Secondary File

            relationshipMap.am_AddRelationshipMap(OrderItemsCTE, useDistinctJoins: false).am_JoinWhere(primaryTableField: "YD1PIID", joinTableField: "YD1I1PID");
            relationshipMap.am_AddRelationshipMap(OrderedProductsCTE, useDistinctJoins: false).am_JoinWhere(primaryTableField: "YD1PIID", joinTableField: "YD1I1PID");

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
			maps.am_AddDataMap("YD1PDC", ProductEntity.DiscontinuedProperty);
			maps.am_AddDataMap("YD1PM1", ProductEntity.MemoProperty);
			maps.am_AddDataMap("YD1PIMPT", ProductEntity.ImagePathProperty);
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
            maps.am_AddDataMap("ORDERCOUNT", ProductEntity.OrderCountProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("ORDERITEMCOUNT", ProductEntity.OrderItemCountProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("AVERAGEORDERQUANTITY", ProductEntity.AverageOrderQuantityProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("SMALLESTORDERQUANTITY", ProductEntity.SmallestOrderQuantityProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LARGESTORDERQUANTITY", ProductEntity.LargestOrderQuantityProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("AVERAGEORDERUNITPRICE", ProductEntity.AverageOrderUnitPriceProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LOWESTORDERUNITPRICE", ProductEntity.LowestOrderUnitPriceProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("HIGHESTORDERUNITPRICE", ProductEntity.HighestOrderUnitPriceProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("LASTORDERDATETIME", ProductEntity.LastOrderDateTimeProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("CUSTOMERCOUNT", ProductEntity.CustomerCountProperty, targetTable: OrderItemsCTE);
            maps.am_AddDataMap("CUSTOMERINTERNALID", ProductEntity.CustomerInternalIDProperty, targetTable: OrderedProductsCTE);

            //TODO: ProductMaps Real Field Example
            //maps.am_AddDataMap("<Field Name>", ProductEntity.<Property Name>);
            //TODO: ProductMaps Virtual Field Example
            //maps.am_AddDataMap("<Field Name>", ProductEntity.<Property Name>, isVirtual: true);
            //TODO: ProductMaps Foreign Field Example
            //maps.am_AddDataMap(string.Format("{0}.{1}", "<Target Table Name>", "<Field Name>"), ProductEntity.<Property Name>, targetTable: "<Target Table Name>"); 
            //TODO: ProductMaps Configure Example (for setting options not available as constructor arguments)
            //maps.am_AddDataMap(...).am_Configure((map) => { map.ap_FunctionExpresion = "..."; });

            return maps;
		}

	}
}