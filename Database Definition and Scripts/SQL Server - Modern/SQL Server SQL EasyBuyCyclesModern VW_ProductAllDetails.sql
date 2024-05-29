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

-- USE "EasyBuyCycles";
-- USE "EasyBuyCyclesDev";

GO

DROP VIEW IF EXISTS "VW_Product_AllDetails";

GO

CREATE OR ALTER VIEW "VW_Product_AllDetails"
    AS
    WITH "OrderItems" AS (
  	         SELECT "ProductInternalID"
                ,COUNT(DISTINCT "OrderItem"."OrderInternalID") AS "OrderCount"
       			,COUNT(*) AS "OrderItemCount"
                ,CAST(AVG("OrderItem"."Quantity") AS DECIMAL(6,2)) AS "AverageOrderQuantity"
                ,MIN("OrderItem"."Quantity") AS "SmallestOrderQuantity"
                ,MAX("OrderItem"."Quantity") AS "LargestOrderQuantity"
                ,CAST(AVG("OrderItem"."UnitPrice") AS DECIMAL(8,2)) AS "AverageOrderUnitPrice"
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

GO

-- View Description
	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Product_AllDetails',
	@name = N'MS_Description', @value = N'Product All Details';

-- Column Labels - Description (50 Chars)
	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'OrderCount',
    @name=N'MS_Description', @value=N'Order Count';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'OrderItemCount',
    @name=N'MS_Description', @value=N'Order Item Count';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'AverageOrderQuantity',
    @name=N'MS_Description', @value=N'Average Order Quantity';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'SmallestOrderQuantity',
    @name=N'MS_Description', @value=N'Smallest Order Quantity';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'LargestOrderQuantity',
    @name=N'MS_Description', @value=N'Largest Order Quantity';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'AverageOrderUnitPrice',
    @name=N'MS_Description', @value=N'Average Order Unit Price';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'LowestOrderUnitPrice',
    @name=N'MS_Description', @value=N'Lowest Order Unit Price';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'HighestOrderUnitPrice',
    @name=N'MS_Description', @value=N'Highest Order Unit Price';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'LastOrderDateTime',
    @name=N'MS_Description', @value=N'Last Order Date Time';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Product_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerCount',
    @name=N'MS_Description', @value=N'Customer Count';