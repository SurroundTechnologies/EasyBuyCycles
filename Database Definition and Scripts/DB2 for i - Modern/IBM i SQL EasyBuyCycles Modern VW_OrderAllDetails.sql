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
Source Member.: IBM i SQL EasyBuyCyclesModern VW_Order_AllDetails.sql
Created by ...: Lee Paul
Created on ...: September 14, 2022
Object Type ..: View Definition
Description ..: All Order Details

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM3/QDBSRC) SRCMBR("Order")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCycles";
-- SET SCHEMA "EasyBuyCyclesDev";

DROP VIEW IF EXISTS "VW_Order_AllDetails";

CREATE OR REPLACE VIEW "VW_Order_AllDetails"
    FOR SYSTEM NAME "YD1OV01" AS
    
    WITH "OrderItems" AS (
    	SELECT "OrderInternalID"
                ,COUNT(*) AS "OrderItemCount"
    			,DECIMAL(SUM("Quantity" * "UnitPrice"),11,2) AS "OrderTotal"
    			,DECIMAL(SUM("Quantity" * "UnitPrice" * ("DiscountPercent" * 0.01)),11,2) AS "OrderDiscount"
    			,DECIMAL(SUM("Quantity" * "UnitPrice" * (1 - ("DiscountPercent" * 0.01))),11,2) AS "OrderDiscountedTotal"
    		FROM "OrderItem"
    		GROUP BY "OrderInternalID"
    )
    SELECT "Order"."InternalID"
    		,"Order"."CustomerInternalID"
    		,"Order"."OrderDateTime"
    		,"Order"."PurchaseOrderNumber"
    		,"Order"."WarehouseInternalID"
    		,"Order"."WarehouseName"
    		,"Order"."DeliveryMemo"
    		,"Order"."ShippingAddressInternalID"
    		,"Order"."OrderMemo"
    		,"Order"."Status"
    		,"Order"."SalesPersonInternalID"
    		,"Order"."SalesPersonName"
    		,"Order"."PurchasePoints"
    
    -- Joins to Customer
    		,"Customer"."Name" AS "CustomerName"
            ,"Customer"."BillingAddress1"
    		   CONCAT CASE "Customer"."BillingAddress2" WHEN '' THEN '' ELSE ', ' CONCAT "Customer"."BillingAddress2" END
    		   CONCAT CASE "Customer"."BillingAddress3" WHEN '' THEN '' ELSE ', ' CONCAT "Customer"."BillingAddress3" END
    		   CONCAT CASE "Customer"."BillingPostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "Customer"."BillingPostalCode" END
    		   CONCAT CASE "Customer"."BillingCountry" WHEN '' THEN '' ELSE ', ' CONCAT "Customer"."BillingCountry" END
    		   AS "CustomerBillingAddressLine"
    		,"Customer"."BillingAddress1"
    		   CONCAT CASE "Customer"."BillingAddress2" WHEN '' THEN '' ELSE CHR(13) CONCAT "Customer"."BillingAddress2" END
    		   CONCAT CASE "Customer"."BillingAddress3" WHEN '' THEN '' ELSE CHR(13) CONCAT "Customer"."BillingAddress3" END
    		   CONCAT CASE "Customer"."BillingPostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "Customer"."BillingPostalCode" END
    		   CONCAT CASE "Customer"."BillingCountry" WHEN '' THEN '' ELSE CHR(13) CONCAT "Customer"."BillingCountry" END
    		   AS "CustomerBillingAddressBlock"    
    
    -- Joins to ShippingAddress
            ,"ShippingAddress"."Name" AS "ShippingAddressName"
            ,"ShippingAddress"."Address1"
    		   CONCAT CASE "ShippingAddress"."Address2" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Address2" END
    		   CONCAT CASE "ShippingAddress"."Address3" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Address3" END
    		   CONCAT CASE "ShippingAddress"."PostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."PostalCode" END
    		   CONCAT CASE "ShippingAddress"."Country" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Country" END
    		   AS "ShippingAddressLine"
    		,"ShippingAddress"."Address1"
    		   CONCAT CASE "ShippingAddress"."Address2" WHEN '' THEN '' ELSE CHR(13) CONCAT "ShippingAddress"."Address2" END
    		   CONCAT CASE "ShippingAddress"."Address3" WHEN '' THEN '' ELSE CHR(13) CONCAT "ShippingAddress"."Address3" END
    		   CONCAT CASE "ShippingAddress"."PostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."PostalCode" END
    		   CONCAT CASE "ShippingAddress"."Country" WHEN '' THEN '' ELSE CHR(13) CONCAT "ShippingAddress"."Country" END
    		   AS "ShippingAddressBlock"   
    
    -- Audit Stamps
            ,"Order"."CreatedAt" AS "CreatedAt"
            ,"Order"."CreatedBy" AS "CreatedBy"
            ,"Order"."CreatedWith" AS "CreatedWith"
            ,"Order"."LastModifiedAt" AS "ModifiedAt"
            ,"Order"."LastModifiedBy" AS "ModifiedBy"
            ,"Order"."LastModifiedWith" AS "ModifiedWith"
    
    	FROM "Order"
    	LEFT JOIN "OrderItems" ON "OrderItems"."OrderInternalID" = "Order"."InternalID"
        LEFT JOIN "Customer" ON "Customer"."InternalID" = "Order"."CustomerInternalID"
        LEFT JOIN "ShippingAddress" ON "ShippingAddress"."InternalID" = "Order"."ShippingAddressInternalID"
;

-- View Description
LABEL ON TABLE "VW_Order_AllDetails"
   IS 'Order All Details';

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "VW_Order_AllDetails" (
     "CustomerName"              TEXT IS 'Customer Name'
     ,"CustomerBillingAddressLine"          TEXT IS 'Customer Billing Address Line'
     ,"CustomerBillingAddressBlock"         TEXT IS 'Customer Billing Address Block'
     ,"ShippingAddressName"              TEXT IS 'Shipping Address Name'
     ,"ShippingAddressLine"          TEXT IS 'Shipping Address Line'
     ,"ShippingAddressBlock"          TEXT IS 'Shipping Address Block'
);

-- Grant View Access
GRANT SELECT
ON "VW_Order_AllDetails" TO PUBLIC WITH GRANT OPTION ;
GRANT ALTER, REFERENCES, SELECT
ON "VW_Order_AllDetails" TO QPGMR WITH GRANT OPTION ;
