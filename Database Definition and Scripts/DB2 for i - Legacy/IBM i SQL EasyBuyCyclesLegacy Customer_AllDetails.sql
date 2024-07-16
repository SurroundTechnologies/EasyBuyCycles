/*******************************************************************************

CONFIDENTIAL AND PROPRIETARY INFORMATION

The receipt or possession of this document does not convey any rights to
reproduce, disclose its contents or manufacture, use or sell anything it may
describe.  Reproduction, disclosure, or use without the specific written
authorization of Surround Technologies, LLC is strictly forbidden.

Copyright ....: (C) Surround Technologies, LLC 1998 - 2021
                9197 Estero River Circle
                Estero, FL 33928
                973-743-1277
                http://www.surroundtech.com/
                 
================================================================================

Source Filer..: IBM i SQL EasyBuyCyclesLegacy Customer_AllDetails.sql
Created by ...: Lee Paul
Created on ...: September 14, 2022
Object Type ..: View Definition
Description ..: All Customer Details

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM3/QDBSRC) SRCMBR("Customer")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA EASYBUYDEM;
-- SET SCHEMA EASYBUYDEV;

    WITH "CTE_SubCustomers" AS (
        SELECT "YD1CPTID" AS "ParentInternalID"
                ,COUNT(*) AS "SubCustomerCount"
                ,CLOB(LISTAGG("YD1CIID" CONCAT '-' CONCAT RTRIM("YD1CNM"),', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY "YD1CNM"),5000) AS "SubCustomerList"
            FROM "YD1C" AS "Customer"
            GROUP BY "YD1CPTID"
    )
    , "CTE_ShippingAddresses" AS (
        SELECT "YD1S1CID" AS "CustomerInternalID"
                ,COUNT(*) AS "ShippingAddressCount"
                ,CLOB(LISTAGG("YD1SIID" CONCAT '-' CONCAT RTRIM("YD1SNM"),', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY "YD1SNM"),5000) AS "ShippingAddressList"
            FROM "YD1S" AS "ShippingAddress"
            GROUP BY "YD1S1CID"
    )
    ,"CTE_OrderItems" AS (
        SELECT "YD1I1OID" AS "OrderInternalID"
                ,DECIMAL(SUM("YD1IQT" * "YD1IPRUN"),11,2) AS "OrderSubtotal"
                ,DECIMAL(SUM("YD1IQT" * "YD1IPRUN" * ("YD1IDSPC" * 0.01)),11,2) AS "OrderDiscount"
                ,DECIMAL(SUM("YD1IQT" * "YD1IPRUN" * (1 - ("YD1IDSPC" * 0.01))),11,2) AS "OrderTotal"
            FROM "YD1I" AS "OrderItem"
            GROUP BY "YD1I1OID"
    )
    ,"CTE_Orders" AS (
        SELECT "YD1O1CID" AS "CustomerInternalID"
                ,MAX(TIMESTAMP("Order"."YD1ODT", "Order"."YD1OTM")) AS "LastOrderDateTime"
                ,MIN(TIMESTAMP("Order"."YD1ODT", "Order"."YD1OTM")) AS "FirstOrderDateTime"
                ,COUNT(*) AS "OrderCount"
                ,SUM(CASE WHEN "Order"."YD1OST" IN('Open', 'New', 'Pending') THEN 1 ELSE 0 END) AS "IncompleteOrderCount"
                ,SUM(CASE WHEN "CTE_OrderItems"."OrderDiscount" > 0 THEN 1 ELSE 0 END) AS "DiscountedOrderCount"
                ,DECIMAL(MAX("CTE_OrderItems"."OrderSubtotal"),11,2) AS "HighestOrderSubtotal"
                ,DECIMAL(MAX("CTE_OrderItems"."OrderDiscount"),11,2) AS "HighestDiscount"
                ,DECIMAL(MAX("CTE_OrderItems"."OrderTotal"),11,2) AS "HighestOrderTotal"
                ,DECIMAL(AVG("CTE_OrderItems"."OrderSubtotal"),11,2) AS "AverageOrderSubtotal"
                ,DECIMAL(AVG("CTE_OrderItems"."OrderDiscount"),11,2) AS "AverageDiscount"
                ,DECIMAL(AVG("CTE_OrderItems"."OrderTotal"),11,2) AS "AverageOrderTotal"
                ,DECIMAL(MIN("CTE_OrderItems"."OrderSubtotal"),11,2) AS "LowestOrderSubtotal"
                ,DECIMAL(MIN("CTE_OrderItems"."OrderDiscount"),11,2) AS "LowestDiscount"
                ,DECIMAL(MIN("CTE_OrderItems"."OrderTotal"),11,2) AS "LowestOrderTotal"
            FROM "YD1O" AS "Order"
                LEFT JOIN "CTE_OrderItems" ON "CTE_OrderItems"."OrderInternalID" = "Order"."YD1OIID"
            GROUP BY "YD1O1CID" 
    )
    ,"CTE_OrderedProducts" AS (
        SELECT "YD1O1CID" AS "CustomerInternalID"
                ,COUNT(*) AS "OrderedItemsCount"
                ,COUNT(DISTINCT "YD1I1PID") AS "ProductsOrderedCount"
            FROM "YD1O" AS "Order"
                RIGHT JOIN "YD1I" AS "OrderItem" ON "OrderItem"."YD1I1OID" = "Order"."YD1OIID"
                LEFT JOIN "YD1P" AS "Product" ON "Product"."YD1PIID" = "OrderItem"."YD1I1PID"
            GROUP BY "YD1O1CID" 
    )
    SELECT "Customer"."YD1CIID" AS "InternalID"
            ,"Customer"."YD1CPTID" AS "ParentInternalID"
            ,RTRIM("Customer"."YD1CPTRL") AS "ParentRelationship"
            ,RTRIM("Customer"."YD1CNM") AS "Name"
            ,RTRIM("Customer"."YD1CNMLG") AS "LegalName"
            ,RTRIM("Customer"."YD1CCNLN") AS "ContactLastName"
            ,RTRIM("Customer"."YD1CCNFN") AS "ContactFirstName"
            ,RTRIM("Customer"."YD1CCNMN") AS "ContactMiddleName"
            ,RTRIM("Customer"."YD1CCNNN") AS "ContactNickName"
            ,RTRIM("Customer"."YD1CBLA1") AS "BillingAddress1"
            ,RTRIM("Customer"."YD1CBLA2") AS "BillingAddress2"
            ,RTRIM("Customer"."YD1CBLA3") AS "BillingAddress3"
            ,RTRIM("Customer"."YD1CBLPC") AS "BillingPostalCode"
            ,RTRIM("Customer"."YD1CBLCY") AS "BillingCountry"
            ,RTRIM("Customer"."YD1CTL") AS "Telephone"
            ,RTRIM("Customer"."YD1CEM") AS "Email"
            ,RTRIM("Customer"."YD1CM1") AS "Memo"
            ,"Customer"."YD1CPRPT" AS "PurchasePoints"
    
    -- Calculated Columns
            ,RTRIM("Customer"."YD1CCNFN")
               CONCAT CASE RTRIM("Customer"."YD1CCNMN") WHEN '' THEN '' ELSE SPACE(1) CONCAT RTRIM("Customer"."YD1CCNMN") END
               CONCAT SPACE(1) CONCAT RTRIM("Customer"."YD1CCNLN")
               CONCAT CASE RTRIM("Customer"."YD1CCNNN") WHEN '' THEN '' ELSE ' "' CONCAT RTRIM("Customer"."YD1CCNNN") CONCAT '"' END
               AS "ContactFullName"
            ,RTRIM("Customer"."YD1CBLA1")
               CONCAT CASE RTRIM("Customer"."YD1CBLA2") WHEN '' THEN '' ELSE ', ' CONCAT RTRIM("Customer"."YD1CBLA2") END
               CONCAT CASE RTRIM("Customer"."YD1CBLA3") WHEN '' THEN '' ELSE ', ' CONCAT RTRIM("Customer"."YD1CBLA3") END
               CONCAT CASE RTRIM("Customer"."YD1CBLPC") WHEN '' THEN '' ELSE SPACE(1) CONCAT RTRIM("Customer"."YD1CBLPC") END
               CONCAT CASE RTRIM("Customer"."YD1CBLCY") WHEN '' THEN '' ELSE ', ' CONCAT RTRIM("Customer"."YD1CBLCY") END
               AS "BillingAddressLine"
            ,RTRIM("Customer"."YD1CBLA1")
               CONCAT CASE RTRIM("Customer"."YD1CBLA2") WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM("Customer"."YD1CBLA2") END
               CONCAT CASE RTRIM("Customer"."YD1CBLA3") WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM("Customer"."YD1CBLA3") END
               CONCAT CASE RTRIM("Customer"."YD1CBLPC") WHEN '' THEN '' ELSE SPACE(1) CONCAT RTRIM("Customer"."YD1CBLPC") END
               CONCAT CASE RTRIM("Customer"."YD1CBLCY") WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM("Customer"."YD1CBLCY") END
               AS "BillingAddressBlock"
            ,CASE "Customer"."YD1CPTID" WHEN 0 THEN 0 ELSE 1 END AS "IsaSubCustomer"
            
    -- Join to Parent Customer
            ,RTRIM("ParentCustomer"."YD1CNM") AS "ParentCustomerName"
            ,RTRIM("ParentCustomer"."YD1CCNFN") 
               CONCAT CASE RTRIM("ParentCustomer"."YD1CCNMN") WHEN '' THEN '' ELSE SPACE(1) CONCAT RTRIM("ParentCustomer"."YD1CCNMN") END
               CONCAT SPACE(1) CONCAT RTRIM("ParentCustomer"."YD1CCNLN")
               CONCAT CASE RTRIM("ParentCustomer"."YD1CCNNN") WHEN '' THEN '' ELSE ' "' CONCAT RTRIM("ParentCustomer"."YD1CCNNN") CONCAT '"' END
               AS "ParentCustomerContactFullname"
            ,RTRIM("ParentCustomer"."YD1CTL") AS "ParentCustomerTelephone"
            ,RTRIM("ParentCustomer"."YD1CBLA1")
               CONCAT CASE RTRIM("ParentCustomer"."YD1CBLA2") WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM("ParentCustomer"."YD1CBLA2") END
               CONCAT CASE RTRIM("ParentCustomer"."YD1CBLA3") WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM("ParentCustomer"."YD1CBLA3") END
               CONCAT CASE RTRIM("ParentCustomer"."YD1CBLPC") WHEN '' THEN '' ELSE SPACE(1) CONCAT RTRIM("ParentCustomer"."YD1CBLPC") END
               CONCAT CASE RTRIM("ParentCustomer"."YD1CBLCY") WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM("ParentCustomer"."YD1CBLCY") END
               AS "ParentCustomerAddressBlock"
            
    -- Aggregate Join to Sub Customers
            ,CASE WHEN "CTE_SubCustomers"."SubCustomerCount" > 0 THEN 'TRUE' ELSE 'FALSE' END AS "IsParentCustomer"
            ,CASE WHEN "Customer"."YD1CPTID" IS NULL OR "Customer"."YD1CPTID" = 0 THEN 'FALSE' ELSE 'TRUE' END AS "IsSubCustomer"
            ,"CTE_SubCustomers"."SubCustomerCount"
            ,"CTE_SubCustomers"."SubCustomerList"
    
    -- Aggregate Join to Shipping Addresses
            ,"CTE_ShippingAddresses"."ShippingAddressCount"
            ,"CTE_ShippingAddresses"."ShippingAddressList"
            
    -- Aggregate Join to Orders
            --,"CTE_Orders"."LastOrderDateTime" -- set from Last Order Join
            ,"CTE_Orders"."FirstOrderDateTime"
            ,"CTE_Orders"."OrderCount"
            ,"CTE_Orders"."IncompleteOrderCount"
            ,"CTE_Orders"."DiscountedOrderCount"
            ,"CTE_Orders"."HighestOrderSubtotal"
            ,"CTE_Orders"."HighestDiscount"
            ,"CTE_Orders"."HighestOrderTotal"
            ,"CTE_Orders"."AverageOrderSubtotal"
            ,"CTE_Orders"."AverageDiscount"
            ,"CTE_Orders"."AverageOrderTotal"
            ,"CTE_Orders"."LowestOrderSubtotal"
            ,"CTE_Orders"."LowestDiscount"
            ,"CTE_Orders"."LowestOrderTotal"
    
    -- Join to Last Order and Shipping Address
            ,"LastOrder".*
    
    -- Aggregate Join to Order, Order Item, and Products
            ,"CTE_OrderedProducts"."OrderedItemsCount"
            ,"CTE_OrderedProducts"."ProductsOrderedCount"
    
    -- Audit Stamps
            ,"Customer"."YD1CCRDT"
            ,"Customer"."YD1CCRTM"
            ,CASE WHEN "Customer"."YD1CCRDT" != 0 THEN TIMESTAMP(DIGITS("Customer"."YD1CCRDT") CONCAT DIGITS("Customer"."YD1CCRTM")) END AS "CreatedAt"
            ,RTRIM("Customer"."YD1CCRUS") AS "CreatedBy"
            ,RTRIM("Customer"."YD1CCRJN") AS "CreateJobNumber"
            ,RTRIM("Customer"."YD1CCRJB") AS "CreateJob"
            ,CASE WHEN "Customer"."YD1CCRJB" != '' THEN RTRIM("Customer"."YD1CCRJN") CONCAT '/' CONCAT RTRIM("Customer"."YD1CCRUS")  CONCAT '/' CONCAT  RTRIM("Customer"."YD1CCRJB") END AS "CreatedWith"

            ,"Customer"."YD1CLCDT"
            ,"Customer"."YD1CLCTM"
            ,CASE WHEN "Customer"."YD1CLCDT" != 0 THEN TIMESTAMP(DIGITS("Customer"."YD1CLCDT") CONCAT DIGITS("Customer"."YD1CLCTM")) END AS "ModifiedAt"
            ,RTRIM("Customer"."YD1CLCUS") AS "LastChangedBy"
            ,RTRIM("Customer"."YD1CLCJN") AS "LastChangeJobNumber"
            ,RTRIM("Customer"."YD1CLCJB") AS "LastChangeJob"
            ,CASE WHEN "Customer"."YD1CCRJB" != '' THEN RTRIM("Customer"."YD1CLCJN") CONCAT '/' CONCAT RTRIM("Customer"."YD1CLCUS")  CONCAT '/' CONCAT  RTRIM("Customer"."YD1CLCJB") END AS "ModifiedWith"
    
        FROM YD1C AS "Customer"
            LEFT JOIN LATERAL (
                SELECT "Order"."YD1OIID" AS "LastOrderID"
                        ,TIMESTAMP("Order"."YD1ODT", "Order"."YD1OTM") AS "LastOrderDateTime"
                        ,"Order"."YD1OST" AS "LastOrderStatus"
                        ,"Order"."YD1O1SID" AS "LastUsedShippingAddressID"
                        ,"ShippingAddress"."YD1SNM" AS "LastUsedShippingAddressName"
                        ,"ShippingAddress"."YD1SSHA1"
                           CONCAT CASE "ShippingAddress"."YD1SSHA2" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."YD1SSHA2" END
                           CONCAT CASE "ShippingAddress"."YD1SSHA3" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."YD1SSHA3" END
                           CONCAT CASE "ShippingAddress"."YD1SSHPC" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."YD1SSHPC" END
                           CONCAT CASE "ShippingAddress"."YD1SSHCY" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."YD1SSHCY" END
                           AS "LastUsedShippingAddressLine"
                    FROM YD1O AS "Order"
                        RIGHT JOIN "YD1I" AS "OrderItem" ON "OrderItem"."YD1I1OID" = "Order"."YD1OIID"
                        LEFT JOIN "YD1S" AS "ShippingAddress" ON "ShippingAddress"."YD1SIID" = "Order"."YD1O1SID"
                    WHERE "Order"."YD1O1CID" = "Customer"."YD1CIID"
                    ORDER BY "Order"."YD1ODT" DESC, "Order"."YD1OTM" DESC, "Order"."YD1OIID" DESC
                    FETCH FIRST 1 ROW ONLY
                ) AS "LastOrder" ON 'X'='X'
            LEFT JOIN "YD1C" AS "ParentCustomer"
                ON "ParentCustomer"."YD1CIID" = "Customer"."YD1CPTID"
            LEFT JOIN "CTE_SubCustomers"
                ON "CTE_SubCustomers"."ParentInternalID" = "Customer"."YD1CIID"
            LEFT JOIN "CTE_ShippingAddresses"
                ON "CTE_ShippingAddresses"."CustomerInternalID" = "Customer"."YD1CIID"
            LEFT JOIN "CTE_Orders"
                ON "CTE_Orders"."CustomerInternalID" = "Customer"."YD1CIID"
            LEFT JOIN "CTE_OrderedProducts"
                ON "CTE_OrderedProducts"."CustomerInternalID" = "Customer"."YD1CIID"
