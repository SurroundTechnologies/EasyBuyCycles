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

-- USE "EasyBuyCycles";
-- USE "EasyBuyCyclesDev";

GO

DROP VIEW IF EXISTS "VW_OrderItem_AllDetails";

GO

CREATE OR ALTER VIEW "VW_OrderItem_AllDetails"
    AS
    SELECT "OrderItem"."InternalID"
    		,"OrderItem"."OrderInternalID"
    		,"OrderItem"."ProductInternalID"
    		,"OrderItem"."Quantity"
    		,"OrderItem"."UnitPrice"
    		,"OrderItem"."DiscountPercent"
    		,"OrderItem"."Memo"
    
    -- Calculated Columns
               ,CAST("OrderItem"."Quantity" * "OrderItem"."DiscountPercent" * 0.01 AS DECIMAL(11,2)) AS "UnitDiscount"
               ,CAST("OrderItem"."Quantity" * "OrderItem"."UnitPrice" AS DECIMAL(11,2)) AS "Price"
    		   ,CAST("OrderItem"."Quantity" * "OrderItem"."UnitPrice" * ("OrderItem"."DiscountPercent" * 0.01) AS DECIMAL(11,2)) AS "OrderDiscount"
    	 	   ,CAST("OrderItem"."Quantity" * "OrderItem"."UnitPrice" * (1 - ("OrderItem"."DiscountPercent" * 0.01)) AS DECIMAL(11,2)) AS "OrderTotal"
    
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

GO

-- View Description
	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_OrderItem_AllDetails',
	@name = N'MS_Description', @value = N'Order Item All Details';

-- Column Labels - Description (50 Chars)
	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'UnitDiscount',
    @name=N'MS_Description', @value=N'Unit Discount';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'Price',
    @name=N'MS_Description', @value=N'Price';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'OrderDiscount',
    @name=N'MS_Description', @value=N'Order Discount';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'OrderTotal',
    @name=N'MS_Description', @value=N'Order Total';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'OrderDateTime',
    @name=N'MS_Description', @value=N'Order Date Time';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'PurchaseOrderNumber',
    @name=N'MS_Description', @value=N'Purchase Order Number';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerInternalID',
    @name=N'MS_Description', @value=N'Customer Internal ID';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerName',
    @name=N'MS_Description', @value=N'Customer Name';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ProductCode',
    @name=N'MS_Description', @value=N'Product Code';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ProductName',
    @name=N'MS_Description', @value=N'Product Name';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ProductCategory',
    @name=N'MS_Description', @value=N'Product Category';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ProductStandardCost',
    @name=N'MS_Description', @value=N'Product Standard Cost';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ProductListPrice',
    @name=N'MS_Description', @value=N'Product List Price';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_OrderItem_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ProductImagePath',
    @name=N'MS_Description', @value=N'Product Image Path';
