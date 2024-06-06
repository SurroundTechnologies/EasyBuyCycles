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

Source File...: QDBSRC
Source Member.: IBM i SQL EasyBuyCyclesModern VW_Customer_AllDetails.sql
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

SET SCHEMA "EasyBuyCycles";
-- SET SCHEMA "EasyBuyCyclesDev";

DROP VIEW IF EXISTS "VW_Customer_AllDetails";

CREATE OR REPLACE VIEW "VW_Customer_AllDetails"
    FOR SYSTEM NAME "YD1CV01" AS
    WITH "SubCustomers" AS (
    	SELECT "ParentInternalID"
    			,COUNT(*) AS "SubCustomerCount"
    			,CLOB(LISTAGG("InternalID" CONCAT '-' CONCAT "Name",', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY "Name"),5000) AS "SubCustomerList"
    		FROM "Customer"
    		GROUP BY "ParentInternalID"
    )
    , "ShippingAddresses" AS (
    	SELECT "CustomerInternalID"
    			,COUNT(*) AS "ShippingAddressCount"
    			,CLOB(LISTAGG("InternalID" CONCAT '-' CONCAT "Name",', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY "Name"),5000) AS "ShippingAddressList"
    		FROM "ShippingAddress"
    		GROUP BY "CustomerInternalID"
    )
    ,"OrderItems" AS (
    	SELECT "OrderInternalID"
    			,DECIMAL(SUM("Quantity" * "UnitPrice"),11,2) AS "OrderSubtotal"
    			,DECIMAL(SUM("Quantity" * "UnitPrice" * ("DiscountPercent" * 0.01)),11,2) AS "OrderDiscount"
    			,DECIMAL(SUM("Quantity" * "UnitPrice" * (1 - ("DiscountPercent" * 0.01))),11,2) AS "OrderTotal"
    		FROM "OrderItem"
    		GROUP BY "OrderInternalID"
    )
    ,"Orders" AS (
    	SELECT "CustomerInternalID"
    			,COUNT(*) AS "OrderCount"
    			,SUM(CASE WHEN "Order"."Status" IN('Open', 'New', 'Pending') THEN 1 ELSE 0 END) AS "IncompleteOrderCount"
    			,SUM(CASE WHEN "OrderItems"."OrderDiscount" > 0 THEN 1 ELSE 0 END) AS "DiscountedOrderCount"
    			,DECIMAL(MAX("OrderItems"."OrderSubtotal"),11,2) AS "HighestOrderSubtotal"
    			,DECIMAL(MAX("OrderItems"."OrderDiscount"),11,2) AS "HighestDiscount"
    			,DECIMAL(MAX("OrderItems"."OrderTotal"),11,2) AS "HighestOrderTotal"
    			,DECIMAL(AVG("OrderItems"."OrderSubtotal"),11,2) AS "AverageOrderSubtotal"
    			,DECIMAL(AVG("OrderItems"."OrderDiscount"),11,2) AS "AverageDiscount"
    			,DECIMAL(AVG("OrderItems"."OrderTotal"),11,2) AS "AverageOrderTotal"
    			,DECIMAL(MIN("OrderItems"."OrderSubtotal"),11,2) AS "LowestOrderSubtotal"
    			,DECIMAL(MIN("OrderItems"."OrderDiscount"),11,2) AS "LowestDiscount"
    			,DECIMAL(MIN("OrderItems"."OrderTotal"),11,2) AS "LowestOrderTotal"
    		FROM "Order"
    		LEFT JOIN "OrderItems" ON "OrderItems"."OrderInternalID" = "Order"."InternalID"
    		GROUP BY "CustomerInternalID" 
    )
    ,"OrderedProducts" AS (
    	SELECT "CustomerInternalID"
    			,COUNT(*) AS "OrderedItemsCount"
    			,COUNT(DISTINCT "ProductInternalID") AS "ProductsOrderedCount"
    		FROM "Order"
    		RIGHT JOIN "OrderItem" ON "OrderItem"."OrderInternalID" = "Order"."InternalID"
    		LEFT JOIN "Product" ON "Product"."InternalID" = "OrderItem"."ProductInternalID"
    		GROUP BY "CustomerInternalID" 
    )
    SELECT "Customer"."InternalID"
    		,"Customer"."ParentInternalID"
    		,"Customer"."ParentRelationship"
    		,"Customer"."Name"
    		,"Customer"."LegalName"
    		,"Customer"."ContactLastName"
    		,"Customer"."ContactFirstName"
    		,"Customer"."ContactMiddleName"
    		,"Customer"."ContactNickName"
    		,"Customer"."BillingAddress1"
    		,"Customer"."BillingAddress2"
    		,"Customer"."BillingAddress3"
    		,"Customer"."BillingPostalCode"
    		,"Customer"."BillingCountry"
    		,"Customer"."Telephone"
    		,"Customer"."Email"
    		,"Customer"."Memo"
    		,"Customer"."PurchasePoints"
    
    -- Calculated Columns
    		,"Customer"."ContactFirstName" 
    		   CONCAT CASE "Customer"."ContactMiddleName" WHEN '' THEN '' ELSE SPACE(1) CONCAT "Customer"."ContactMiddleName" END
    		   CONCAT SPACE(1) CONCAT "Customer"."ContactLastName"
    		   CONCAT CASE "Customer"."ContactNickName" WHEN '' THEN '' ELSE ' "' CONCAT "Customer"."ContactNickName" CONCAT '"' END
    		   AS "ContactFullName"
    		,"Customer"."BillingAddress1"
    		   CONCAT CASE "Customer"."BillingAddress2" WHEN '' THEN '' ELSE ', ' CONCAT "Customer"."BillingAddress2" END
    		   CONCAT CASE "Customer"."BillingAddress3" WHEN '' THEN '' ELSE ', ' CONCAT "Customer"."BillingAddress3" END
    		   CONCAT CASE "Customer"."BillingPostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "Customer"."BillingPostalCode" END
    		   CONCAT CASE "Customer"."BillingCountry" WHEN '' THEN '' ELSE ', ' CONCAT "Customer"."BillingCountry" END
    		   AS "BillingAddressLine"
    		,"Customer"."BillingAddress1"
    		   CONCAT CASE "Customer"."BillingAddress2" WHEN '' THEN '' ELSE CHR(13) CONCAT "Customer"."BillingAddress2" END
    		   CONCAT CASE "Customer"."BillingAddress3" WHEN '' THEN '' ELSE CHR(13) CONCAT "Customer"."BillingAddress3" END
    		   CONCAT CASE "Customer"."BillingPostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "Customer"."BillingPostalCode" END
    		   CONCAT CASE "Customer"."BillingCountry" WHEN '' THEN '' ELSE CHR(13) CONCAT "Customer"."BillingCountry" END
    		   AS "BillingAddressBlock"
    		,CASE "Customer"."ParentInternalID" WHEN 0 THEN 0 ELSE 1 END AS "IsaSubCustomer"
    		
    -- Joins to Customer
    		,"ParentCustomer"."Name" AS "ParentCustomerName"
    		,"ParentCustomer"."ContactFirstName" 
    		   CONCAT CASE "ParentCustomer"."ContactMiddleName" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ParentCustomer"."ContactMiddleName" END
    		   CONCAT SPACE(1) CONCAT "ParentCustomer"."ContactLastName"
    		   CONCAT CASE "ParentCustomer"."ContactNickName" WHEN '' THEN '' ELSE ' "' CONCAT "ParentCustomer"."ContactNickName" CONCAT '"' END
    		   AS "ParentCustomerContactFullname"
    		,"ParentCustomer"."Telephone" AS "ParentCustomerTelephone"
    		,"ParentCustomer"."BillingAddress1"
    		   CONCAT CASE "ParentCustomer"."BillingAddress2" WHEN '' THEN '' ELSE CHR(13) CONCAT "ParentCustomer"."BillingAddress2" END
    		   CONCAT CASE "ParentCustomer"."BillingAddress3" WHEN '' THEN '' ELSE CHR(13) CONCAT "ParentCustomer"."BillingAddress3" END
    		   CONCAT CASE "ParentCustomer"."BillingPostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ParentCustomer"."BillingPostalCode" END
    		   CONCAT CASE "ParentCustomer"."BillingCountry" WHEN '' THEN '' ELSE CHR(13) CONCAT "ParentCustomer"."BillingCountry" END
    		   AS "ParentCustomerAddressBlock"
    		
    		,"SubCustomers"."SubCustomerCount"
    		,"SubCustomers"."SubCustomerList"
    
    -- Join to Shipping Address
    		,"ShippingAddresses"."ShippingAddressCount"
    		,"ShippingAddresses"."ShippingAddressList"
    		
    -- Join to Order
    		,"Orders"."OrderCount"
    		,"Orders"."IncompleteOrderCount"
    		,"Orders"."DiscountedOrderCount"
    		,"Orders"."HighestOrderSubtotal"
    		,"Orders"."HighestDiscount"
    		,"Orders"."HighestOrderTotal"
    		,"Orders"."AverageOrderSubtotal"
    		,"Orders"."AverageDiscount"
    		,"Orders"."AverageOrderTotal"
    		,"Orders"."LowestOrderSubtotal"
    		,"Orders"."LowestDiscount"
    		,"Orders"."LowestOrderTotal"
    
    -- Join to Last Order and Shipping Address
    		,"LastOrder".*
    
    -- Join to Order, Order Item, and Products
    		,"OrderedProducts"."OrderedItemsCount"
    		,"OrderedProducts"."ProductsOrderedCount"
    
    -- Audit Stamps
            ,"Customer"."CreatedAt" AS "CreatedAt"
            ,"Customer"."CreatedBy" AS "CreatedBy"
            ,"Customer"."CreatedWith" AS "CreatedWith"
            ,"Customer"."LastModifiedAt" AS "ModifiedAt"
            ,"Customer"."LastModifiedBy" AS "ModifiedBy"
            ,"Customer"."LastModifiedWith" AS "ModifiedWith"
    
    	FROM "Customer"
    	LEFT JOIN LATERAL (
    		SELECT "Order"."InternalID" AS "LastOrderID"
    				,"Order"."OrderDateTime" AS "LastOrderDateTime"
    				,"Order"."Status" AS "LastOrderStatus"
    				,"Order"."ShippingAddressInternalID" AS "LastUsedShippingAddressID"
    				,"ShippingAddress"."Name" AS "LastUsedShippingAddressName"
            		,"ShippingAddress"."Address1"
            		   CONCAT CASE "ShippingAddress"."Address2" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Address2" END
            		   CONCAT CASE "ShippingAddress"."Address3" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Address3" END
            		   CONCAT CASE "ShippingAddress"."PostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."PostalCode" END
            		   CONCAT CASE "ShippingAddress"."Country" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Country" END
                       AS "LastUsedShippingAddressLine"
    			FROM "Order"
    			RIGHT JOIN "OrderItem" ON "OrderInternalID" = "Order"."InternalID"
    			LEFT JOIN "ShippingAddress" ON "ShippingAddress"."InternalID" = "Order"."ShippingAddressInternalID"
    			WHERE "Order"."CustomerInternalID" = "Customer"."InternalID"
    			ORDER BY "Order"."OrderDateTime" DESC, "Order"."InternalID" DESC
    			FETCH FIRST 1 ROW ONLY
    		) AS "LastOrder" ON 'X'='X'
    	LEFT JOIN "Customer" AS "ParentCustomer" ON "ParentCustomer"."InternalID" = "Customer"."ParentInternalID"
    	LEFT JOIN "SubCustomers" ON "SubCustomers"."ParentInternalID" = "Customer"."InternalID"
    	LEFT JOIN "ShippingAddresses" ON "ShippingAddresses"."CustomerInternalID" = "Customer"."InternalID"
    	LEFT JOIN "Orders" ON "Orders"."CustomerInternalID" = "Customer"."InternalID"
    	LEFT JOIN "OrderedProducts" ON "OrderedProducts"."CustomerInternalID" = "Customer"."InternalID"
;

-- View Description
LABEL ON TABLE "VW_Customer_AllDetails"
   IS 'Customer All Details';

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "VW_Customer_AllDetails" (
     "ContactFullName"              TEXT IS 'Contact Full Name'
     ,"BillingAddressLine"          TEXT IS 'Billing Address Line'
     ,"BillingAddressBlock"         TEXT IS 'Billing Address Block'
     ,"IsaSubCustomer"              TEXT IS 'Is a Sub Customer'
     ,"ParentCustomerName"          TEXT IS 'Parent Customer Name'
     ,"ParentCustomerContactFullname" TEXT IS 'Parent Customer Contact Full Name'
     ,"ParentCustomerTelephone"     TEXT IS 'Parent Customer Telephone'
     ,"ParentCustomerAddressBlock"  TEXT IS 'Parent Customer Address Block'
     ,"SubCustomerCount"            TEXT IS 'Sub Customer Count'
     ,"SubCustomerList"             TEXT IS 'Sub Customer List'
     ,"ShippingAddressCount"        TEXT IS 'Shipping Address Count'
     ,"ShippingAddressList"         TEXT IS 'Shipping Address List'
     ,"OrderCount"                  TEXT IS 'Order Count'
     ,"IncompleteOrderCount"        TEXT IS 'Incomplete Order Count'
     ,"DiscountedOrderCount"        TEXT IS 'Discounted Order Count'
     ,"HighestOrderSubtotal"        TEXT IS 'Highest Order Subtotal'
     ,"HighestDiscount"             TEXT IS 'Highest Discount'
     ,"HighestOrderTotal"           TEXT IS 'Highest Order Total'
     ,"AverageOrderSubtotal"        TEXT IS 'Average Order Subtotal'
     ,"AverageDiscount"             TEXT IS 'Average Discount'
     ,"AverageOrderTotal"           TEXT IS 'Average Order Total'
     ,"LowestOrderSubtotal"         TEXT IS 'Lowest Order Subtotal'
     ,"LowestDiscount"              TEXT IS 'Lowest Discount'
     ,"LowestOrderTotal"            TEXT IS 'Lowest Order Total'
     ,"LastOrderID"                 TEXT IS 'Last Order ID'
     ,"LastOrderDateTime"           TEXT IS 'Last Order Date Time'
     ,"LastOrderStatus"             TEXT IS 'Last Order Status'
     ,"LastUsedShippingAddressID"   TEXT IS 'Last Used Shipping Address ID'
     ,"LastUsedShippingAddressName" TEXT IS 'Last Used Shipping Address Name'
     ,"LastUsedShippingAddressLine" TEXT IS 'Last Used Shipping Address Line'
     ,"OrderedItemsCount"           TEXT IS 'Ordered Items Count'
     ,"ProductsOrderedCount"        TEXT IS 'Products Ordered Count'
);

-- Grant View Access
GRANT SELECT
ON "VW_Customer_AllDetails" TO PUBLIC WITH GRANT OPTION ;
GRANT ALTER, REFERENCES, SELECT
ON "VW_Customer_AllDetails" TO QPGMR WITH GRANT OPTION ;
