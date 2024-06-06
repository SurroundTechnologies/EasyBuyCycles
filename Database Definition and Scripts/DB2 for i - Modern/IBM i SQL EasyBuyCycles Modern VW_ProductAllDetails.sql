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
Source Member.: IBM i SQL EasyBuyCyclesModern VW_Product_AllDetails.sql
Created by ...: Lee Paul
Created on ...: September 14, 2022
Object Type ..: View Definition
Description ..: All Product Details

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM3/QDBSRC) SRCMBR("Product")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCycles";
-- SET SCHEMA "EasyBuyCyclesDev";

DROP VIEW IF EXISTS "VW_Product_AllDetails";

CREATE OR REPLACE VIEW "VW_Product_AllDetails"
    FOR SYSTEM NAME "YD1PV01" AS
    
    WITH "OrderItems" AS (
  	         SELECT "ProductInternalID"
                ,COUNT(DISTINCT "OrderItem"."OrderInternalID") AS "OrderCount"
       			,COUNT(*) AS "OrderItemCount"
                ,DECIMAL(AVG("OrderItem"."Quantity"),6,2) AS "AverageOrderQuantity"
                ,MIN("OrderItem"."Quantity") AS "SmallestOrderQuantity"
                ,MAX("OrderItem"."Quantity") AS "LargestOrderQuantity"
                ,DECIMAL(AVG("OrderItem"."UnitPrice"),8,2) AS "AverageOrderUnitPrice"
                ,MIN("OrderItem"."UnitPrice") AS "LowestOrderUnitPrice"
                ,MAX("OrderItem"."UnitPrice") AS "HighestOrderUnitPrice"
                ,MAX("Order"."OrderDateTime") AS "LastOrderDateTime"
                ,COUNT(DISTINCT "Order"."CustomerInternalID") AS "CustomerCount"
       		FROM "OrderItem"
               LEFT JOIN "Order" ON "Order"."InternalID" = "OrderItem"."OrderInternalID"
       		GROUP BY "ProductInternalID"
   )
    SELECT "Product"."InternalID"
    		,"Product"."Code"
    		,"Product"."Name"
    		,"Product"."Description"
    		,"Product"."Category"
    		,"Product"."StandardCost"
    		,"Product"."ListPrice"
    		,"Product"."ReorderLevel"
    		,"Product"."TargetLevel"
    		,"Product"."MinimumReorderQuantity"
    		,"Product"."Discontinued"
    		,"Product"."Memo"
    		,"Product"."ImagePath"
 
    -- Joins to Order Item
    		,"OrderItems"."OrderCount"
            ,"OrderItems"."OrderItemCount"
            ,"OrderItems"."AverageOrderQuantity"
            ,"OrderItems"."SmallestOrderQuantity"
            ,"OrderItems"."LargestOrderQuantity"
            ,"OrderItems"."AverageOrderUnitPrice"
            ,"OrderItems"."LowestOrderUnitPrice"
            ,"OrderItems"."HighestOrderUnitPrice"
            ,"OrderItems"."LastOrderDateTime"
            ,"OrderItems"."CustomerCount"
    		
    -- Audit Stamps
            ,"Product"."CreatedAt" AS "CreatedAt"
            ,"Product"."CreatedBy" AS "CreatedBy"
            ,"Product"."CreatedWith" AS "CreatedWith"
            ,"Product"."LastModifiedAt" AS "ModifiedAt"
            ,"Product"."LastModifiedBy" AS "ModifiedBy"
            ,"Product"."LastModifiedWith" AS "ModifiedWith"
    
    	FROM "Product"
    	LEFT JOIN "OrderItems" ON "OrderItems"."ProductInternalID" = "Product"."InternalID"
;

-- View Description
LABEL ON TABLE "VW_Product_AllDetails"
   IS 'Product All Details';

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "VW_Product_AllDetails" (
    "OrderCount"                   TEXT IS 'Order Count'
    ,"OrderItemCount"              TEXT IS 'Order Item Count'
    ,"AverageOrderQuantity"        TEXT IS 'Average Order Quantity'
    ,"SmallestOrderQuantity"       TEXT IS 'Smallest Order Quantity'
    ,"LargestOrderQuantity"        TEXT IS 'Largest Order Quantity'
    ,"AverageOrderUnitPrice"       TEXT IS 'Average Order Unit Price'
    ,"LowestOrderUnitPrice"        TEXT IS 'Lowest Order Unit Price'
    ,"HighestOrderUnitPrice"       TEXT IS 'Highest Order Unit Price'
    ,"LastOrderDateTime"           TEXT IS 'Last Order Date Time'
    ,"CustomerCount"               TEXT IS 'Customer Count'
);

-- Grant View Access
GRANT SELECT
ON "VW_Product_AllDetails" TO PUBLIC WITH GRANT OPTION ;
GRANT ALTER, REFERENCES, SELECT
ON "VW_Product_AllDetails" TO QPGMR WITH GRANT OPTION ;
