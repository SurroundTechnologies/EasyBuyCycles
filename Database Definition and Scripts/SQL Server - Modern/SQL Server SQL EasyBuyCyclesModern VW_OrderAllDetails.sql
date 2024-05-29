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

-- USE "EasyBuyCycles";
-- USE "EasyBuyCyclesDev";

DROP VIEW IF EXISTS "VW_Order_AllDetails";

GO

CREATE OR ALTER VIEW "VW_Order_AllDetails"
    AS
    WITH "OrderItems" AS (
    	SELECT "OrderInternalID"
                ,COUNT(*) AS "OrderItemCount"
    			,CAST(SUM("Quantity" * "UnitPrice") AS DECIMAL(11,2)) AS "OrderTotal"
    			,CAST(SUM("Quantity" * "UnitPrice" * ("DiscountPercent" * 0.01)) AS DECIMAL(11,2)) AS "OrderDiscount"
    			,CAST(SUM("Quantity" * "UnitPrice" * (1 - ("DiscountPercent" * 0.01))) AS DECIMAL (11,2)) AS "OrderDiscountedTotal"
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
    		   + CASE WHEN "Customer"."BillingAddress2" IS NULL OR "Customer"."BillingAddress2" = '' THEN '' ELSE ', ' + "Customer"."BillingAddress2" END
    		   + CASE WHEN "Customer"."BillingAddress3" IS NULL OR "Customer"."BillingAddress3" = '' THEN '' ELSE ', ' + "Customer"."BillingAddress3" END
    		   + CASE WHEN "Customer"."BillingPostalCode" IS NULL OR "Customer"."BillingPostalCode" = '' THEN '' ELSE SPACE(1) + "Customer"."BillingPostalCode" END
    		   + CASE WHEN "Customer"."BillingCountry" IS NULL OR "Customer"."BillingCountry" = '' THEN '' ELSE ', ' + "Customer"."BillingCountry" END
    		   AS "CustomerBillingAddressLine"
    		,"Customer"."BillingAddress1"
    		   + CASE WHEN "Customer"."BillingAddress2" IS NULL OR "Customer"."BillingAddress2" = '' THEN '' ELSE CHAR(10) + "Customer"."BillingAddress2" END
    		   + CASE WHEN "Customer"."BillingAddress3" IS NULL OR "Customer"."BillingAddress3" = '' THEN '' ELSE CHAR(10) + "Customer"."BillingAddress3" END
    		   + CASE WHEN "Customer"."BillingPostalCode" IS NULL OR "Customer"."BillingPostalCode" = '' THEN '' ELSE SPACE(1) + "Customer"."BillingPostalCode" END
    		   + CASE WHEN "Customer"."BillingCountry" IS NULL OR "Customer"."BillingCountry" = '' THEN '' ELSE CHAR(10) + "Customer"."BillingCountry" END
    		   AS "CustomerBillingAddressBlock"    
    
    -- Joins to ShippingAddress
            ,"ShippingAddress"."Name" AS "ShippingAddressName"
            ,"ShippingAddress"."Address1"
    		   + CASE WHEN "ShippingAddress"."Address2" IS NULL OR "ShippingAddress"."Address2" = '' THEN '' ELSE ', ' + "ShippingAddress"."Address2" END
    		   + CASE WHEN "ShippingAddress"."Address3" IS NULL OR "ShippingAddress"."Address3" = '' THEN '' ELSE ', ' + "ShippingAddress"."Address3" END
    		   + CASE WHEN "ShippingAddress"."PostalCode" IS NULL OR "ShippingAddress"."PostalCode" = '' THEN '' ELSE SPACE(1) + "ShippingAddress"."PostalCode" END
    		   + CASE WHEN "ShippingAddress"."Country" IS NULL OR "ShippingAddress"."Country" = '' THEN '' ELSE ', ' + "ShippingAddress"."Country" END
    		   AS "ShippingAddressLine"
    		,"ShippingAddress"."Address1"
    		   + CASE WHEN "ShippingAddress"."Address2" IS NULL OR "ShippingAddress"."Address2" = '' THEN '' ELSE CHAR(10) + "ShippingAddress"."Address2" END
    		   + CASE WHEN "ShippingAddress"."Address3" IS NULL OR "ShippingAddress"."Address3" = '' THEN '' ELSE CHAR(10) + "ShippingAddress"."Address3" END
    		   + CASE WHEN "ShippingAddress"."PostalCode" IS NULL OR "ShippingAddress"."PostalCode" = '' THEN '' ELSE SPACE(1) + "ShippingAddress"."PostalCode" END
    		   + CASE WHEN "ShippingAddress"."Country" IS NULL OR "ShippingAddress"."Country" = '' THEN '' ELSE CHAR(10) + "ShippingAddress"."Country" END
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

GO

-- View Description
EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Order_AllDetails',
	@name = N'MS_Description', @value = N'Order All Details';

-- Column Labels - Description (50 Chars)
	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Order_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerName',
    @name=N'MS_Description', @value=N'Customer Name';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Order_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerBillingAddressLine',
    @name=N'MS_Description', @value=N'Customer Billing Address Line';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Order_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerBillingAddressBlock',
    @name=N'MS_Description', @value=N'Customer Billing Address Block';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Order_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ShippingAddressName',
    @name=N'MS_Description', @value=N'Shipping Address Name';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Order_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ShippingAddressLine',
    @name=N'MS_Description', @value=N'Shipping Address Line';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Order_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ShippingAddressBlock',
    @name=N'MS_Description', @value=N'Shipping Address Block';