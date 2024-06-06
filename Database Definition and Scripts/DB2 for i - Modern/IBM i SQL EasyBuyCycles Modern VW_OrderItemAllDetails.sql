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
Source Member.: IBM i SQL EasyBuyCyclesModern VW_OrderItem_AllDetails.sql
Created by ...: Lee Paul
Created on ...: September 14, 2022
Object Type ..: View Definition
Description ..: All OrderItem Details

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM3/QDBSRC) SRCMBR("OrderItem")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCycles";
-- SET SCHEMA "EasyBuyCyclesDev";

DROP VIEW IF EXISTS "VW_OrderItem_AllDetails";

CREATE OR REPLACE VIEW "VW_OrderItem_AllDetails"
    FOR SYSTEM NAME "YD1IV01" AS
    
    SELECT "OrderItem"."InternalID"
    		,"OrderItem"."OrderInternalID"
    		,"OrderItem"."ProductInternalID"
    		,"OrderItem"."Quantity"
    		,"OrderItem"."UnitPrice"
    		,"OrderItem"."DiscountPercent"
    		,"OrderItem"."Memo"
    
    -- Calculated Columns
               ,DECIMAL("OrderItem"."Quantity" * "OrderItem"."DiscountPercent" * 0.01,11,2) AS "UnitDiscount"
               ,DECIMAL("OrderItem"."Quantity" * "OrderItem"."UnitPrice",11,2) AS "Price"
    		   ,DECIMAL("OrderItem"."Quantity" * "OrderItem"."UnitPrice" * ("OrderItem"."DiscountPercent" * 0.01),11,2) AS "OrderDiscount"
    	 	   ,DECIMAL("OrderItem"."Quantity" * "OrderItem"."UnitPrice" * (1 - ("OrderItem"."DiscountPercent" * 0.01)),11,2) AS "OrderTotal"
    
    -- Join to Order
    		,"Order"."OrderDateTime"
    		,"Order"."PurchaseOrderNumber" 
    		,"Order"."CustomerInternalID"
    
    -- Join to Customer
    		,"Customer"."Name" AS "CustomerName" 
    
    -- Join to Product
            ,"Product"."Code" AS "ProductCode"
            ,"Product"."Name" AS "ProductName"
            ,"Product"."Category" AS "ProductCategory"
            ,"Product"."StandardCost" AS "ProductStandardCost"
            ,"Product"."ListPrice" AS "ProductListPrice"
            ,"Product"."ImagePath" AS "ProductImagePath"
    
    -- Audit Stamps
            ,"OrderItem"."CreatedAt" AS "CreatedAt"
            ,"OrderItem"."CreatedBy" AS "CreatedBy"
            ,"OrderItem"."CreatedWith" AS "CreatedWith"
            ,"OrderItem"."LastModifiedAt" AS "ModifiedAt"
            ,"OrderItem"."LastModifiedBy" AS "ModifiedBy"
            ,"OrderItem"."LastModifiedWith" AS "ModifiedWith"
    
    	FROM "OrderItem"
        LEFT JOIN "Order" ON "Order"."InternalID" = "OrderItem"."OrderInternalID"
        LEFT JOIN "Customer" ON "Order"."CustomerInternalID" = "Customer"."InternalID"
        LEFT JOIN "Product" ON "Product"."InternalID" = "OrderItem"."ProductInternalID"
;

-- View Description
LABEL ON TABLE "VW_OrderItem_AllDetails"
   IS 'Order Item All Details';

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "VW_OrderItem_AllDetails" (
     "UnitDiscount"             TEXT IS 'Unit Discount'
     ,"Price"                   TEXT IS 'Price'
     ,"OrderDiscount"           TEXT IS 'Order Discount'
     ,"OrderTotal"              TEXT IS 'Order Total'
     ,"OrderDateTime"           TEXT IS 'Order Date Time'
     ,"PurchaseOrderNumber"     TEXT IS 'Purchase Order Number'
     ,"CustomerInternalID"      TEXT IS 'Customer Internal ID'
     ,"CustomerName"            TEXT IS 'Customer Name'    
     ,"ProductCode"             TEXT IS 'Product Code'
     ,"ProductName"             TEXT IS 'Product Name'
     ,"ProductCategory"         TEXT IS 'Product Category'
     ,"ProductStandardCost"     TEXT IS 'Product Standard Cost'
     ,"ProductListPrice"        TEXT IS 'Product List Price'    
     ,"ProductImagePath"        TEXT IS 'Product Image Path'
);

-- Grant View Access
GRANT SELECT
ON "VW_OrderItem_AllDetails" TO PUBLIC WITH GRANT OPTION ;
GRANT ALTER, REFERENCES, SELECT
ON "VW_OrderItem_AllDetails" TO QPGMR WITH GRANT OPTION ;
