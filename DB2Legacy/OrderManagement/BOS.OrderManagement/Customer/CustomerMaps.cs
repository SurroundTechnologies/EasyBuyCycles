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
        private const string YD1CParentAlias = "PARENT";
        private const string SubCustomerCTE = "SUBCUSTOMER";
        private const string ShippingAddressCTE = "SHIPPINGADDRESS";
        private const string OrdersCTE = "ORDERS";

        public CustomerMaps() : base() { }
		public CustomerMaps(string qualifier) : base(qualifier) { }

        /// <summary>
        /// Loads maps to join two database files.
        /// </summary>
        /// 
        public override AB_CommonTableExpressionDictionary am_LoadCommonTableExpressions()
        {
            var ctes = new AB_CommonTableExpressionDictionary();

            // Add CTE's here
            ctes.am_AddCTE(SubCustomerCTE, @"SELECT YD1CPTID,
                                     COUNT(*) AS SUBCUSTOMERCOUNT,
                                     CLOB(LISTAGG(RTRIM(YD1CNM), ', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY YD1CNM), 5000) AS SUBCUSTOMERLIST
                                 FROM YD1C
                                 GROUP BY YD1CPTID");
            ctes.am_AddCTE(ShippingAddressCTE, @"SELECT YD1S1CID, 
                                         COUNT(*) AS SHIPPINGADDRESSCOUNT
                                     FROM YD1S
                                     GROUP BY YD1S1CID");
            ctes.am_AddCTE(OrdersCTE, @"SELECT YD1O1CID,
                                COUNT(*) AS ORDERCOUNT,
                                SUM(CASE WHEN YD1OST IN('Open', 'New', 'Pending') THEN 1 ELSE 0 END) AS INCOMPLETEORDERCOUNT,
                                SUM(CASE WHEN ORDERITEMS.ORDERDISCOUNT > 0 THEN 1 ELSE 0 END) AS DISCOUNTEDORDERCOUNT,
                                DECIMAL(MAX(ORDERITEMS.ORDERSUBTOTAL), 11, 2) AS HIGHESTORDERSUBTOTAL,
                                DECIMAL(MAX(ORDERITEMS.ORDERDISCOUNT), 11, 2) AS HIGHESTORDERDISCOUNT,
                                DECIMAL(MAX(ORDERITEMS.ORDERTOTAL), 11, 2) AS HIGHESTORDERTOTAL, 
                                DECIMAL(AVG(ORDERITEMS.ORDERSUBTOTAL), 11, 2) AS AVERAGEORDERSUBTOTAL, 
                                DECIMAL(AVG(ORDERITEMS.ORDERDISCOUNT), 11, 2) AS AVERAGEORDERDISCOUNT, 
                                DECIMAL(AVG(ORDERITEMS.ORDERTOTAL), 11, 2) AS AVERAGEORDERTOTAL, 
                                DECIMAL(MIN(ORDERITEMS.ORDERSUBTOTAL), 11, 2) AS LOWESTORDERSUBTOTAL, 
                                DECIMAL(MIN(ORDERITEMS.ORDERDISCOUNT), 11, 2) AS LOWESTORDERDISCOUNT,
                                DECIMAL(MIN(ORDERITEMS.ORDERTOTAL), 11, 2) AS LOWESTORDERTOTAL
                            FROM YD1O
                            LEFT JOIN (SELECT YD1I1OID,
                                          COUNT(*) AS ORDERITEMCOUNT,
                                          DECIMAL(SUM(YD1IQT * YD1IPRUN), 11, 2) AS ORDERSUBTOTAL,
                                          DECIMAL(SUM(YD1IQT * YD1IPRUN * (YD1IDSPC * 0.01)), 11, 2) AS ORDERDISCOUNT,
                                          DECIMAL(SUM(YD1IQT * YD1IPRUN * (1 - (YD1IDSPC * 0.01))), 11, 2) AS ORDERTOTAL
                                      FROM YD1I
                                      GROUP BY YD1I1OID) ORDERITEMS ON ORDERITEMS.YD1I1OID = YD1OIID
                            GROUP BY YD1O1CID");

            return ctes;
        }

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

            AB_RelationshipMap map0 = new AB_RelationshipMap(YD1CTableName, YD1CTableName, JoinType.LeftOuter);
            map0.ap_JoinedFileAlias = YD1CParentAlias;
            map0.ap_JoinConditions.Add(new AB_JoinCondition(new AB_QueryField(map0.ap_PrimaryFile, "YD1CPTID"), "=", new AB_QueryField(map0.ap_JoinedFileAlias, "YD1CIID")));
            relationshipMap.Add(map0.ap_JoinedFileAlias, map0);

            AB_RelationshipMap map1 = new AB_RelationshipMap(YD1CTableName, SubCustomerCTE, JoinType.LeftOuter);
            map1.ap_JoinConditions.Add(new AB_JoinCondition(new AB_QueryField(map1.ap_PrimaryFile, "YD1CIID"), "=", new AB_QueryField(map1.ap_JoinedFile, "YD1CPTID")));
            relationshipMap.Add(map1.ap_JoinedFile, map1);

            relationshipMap.am_AddRelationshipMap(ShippingAddressCTE, useDistinctJoins: false).am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1S1CID");

            relationshipMap.am_AddRelationshipMap(OrdersCTE, useDistinctJoins: false).am_JoinWhere(primaryTableField: "YD1CIID", joinTableField: "YD1O1CID");

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
            maps.am_AddDataMap("IsSubCustomer", CustomerEntity.IsSubCustomerProperty, isVirtual: true);
            maps.am_AddDataMap("SUBCUSTOMERCOUNT", CustomerEntity.SubCustomerCountProperty, targetTable: SubCustomerCTE);
            maps.am_AddDataMap("SUBCUSTOMERLIST", CustomerEntity.SubCustomerListProperty, targetTable: SubCustomerCTE);
            maps.am_AddDataMap("YD1CNM", CustomerEntity.ParentNameProperty, targetTable: YD1CParentAlias);
            maps.am_AddDataMap("IsParentCustomer", CustomerEntity.IsParentCustomerProperty, isVirtual: true);
            maps.am_AddDataMap("SHIPPINGADDRESSCOUNT", CustomerEntity.ShippingAddressCountProperty, targetTable: ShippingAddressCTE);
            maps.am_AddDataMap("ORDERCOUNT", CustomerEntity.OrderCountProperty, targetTable: OrdersCTE);
            maps.am_AddDataMap("AVERAGEORDERSUBTOTAL", CustomerEntity.AverageOrderSubtotalProperty, targetTable: OrdersCTE);
            maps.am_AddDataMap("AVERAGEORDERDISCOUNT", CustomerEntity.AverageOrderDiscountProperty, targetTable: OrdersCTE);
            maps.am_AddDataMap("AVERAGEORDERTOTAL", CustomerEntity.AverageOrderTotalProperty, targetTable: OrdersCTE);
            maps.am_AddDataMap("ContactFullName", CustomerEntity.ContactFullNameProperty, isVirtual: true);
			maps.am_AddDataMap(string.Format("{0}.{1}", YD1CTableAlias_b, "YD1CNM"), CustomerEntity.ParentNameProperty, targetTable: YD1CTableAlias_b);
            maps.am_AddDataMap("ContactFullName", CustomerEntity.ContactFullNameProperty, isVirtual: true);
            maps.am_AddDataMap("BillingAddressLine", CustomerEntity.BillingAddressLineProperty, isVirtual: true);
            maps.am_AddDataMap("BillingAddressBlock", CustomerEntity.BillingAddressBlockProperty, isVirtual: true);

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