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
Source Member.: SQL Server SQL EasyBuyCyclesModern VW_Customer_AllDetails.sql
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

-- USE "EasyBuyCycles";
-- USE "EasyBuyCyclesDev";

GO

DROP VIEW IF EXISTS "VW_Customer_AllDetails";

GO

CREATE OR ALTER VIEW "VW_Customer_AllDetails"

    AS
    WITH "SubCustomers" AS (
    	SELECT "ParentInternalID"
    			,COUNT(*) AS "SubCustomerCount"
				,STRING_AGG(CAST(RTRIM("InternalID") + '-' + RTRIM("Name") AS NVARCHAR(MAX)), ',') WITHIN GROUP (ORDER BY "InternalID") AS "SubCustomerList" -- Results are not being truncated... yet.
    		FROM "Customer"
    		GROUP BY "ParentInternalID"
    )
    , "ShippingAddresses" AS (
    	SELECT "CustomerInternalID"
    			,COUNT(*) AS "ShippingAddressCount"
				,STRING_AGG(CAST(RTRIM("InternalID") + '-' + RTRIM("Name") AS NVARCHAR(MAX)), ',') WITHIN GROUP (ORDER BY "InternalID") AS "ShippingAddressList" -- Results are not being truncated... yet.
    		FROM "ShippingAddress"
    		GROUP BY "CustomerInternalID"
    )
    ,"OrderItems" AS (
    	SELECT "OrderInternalID"
    			,CAST(SUM("Quantity" * "UnitPrice") AS DECIMAL(11,2)) AS "OrderSubtotal"
    			,CAST(SUM("Quantity" * "UnitPrice" * ("DiscountPercent" * 0.01)) AS DECIMAL(11,2)) AS "OrderDiscount"
    			,CAST(SUM("Quantity" * "UnitPrice" * (1 - ("DiscountPercent" * 0.01))) AS DECIMAL(11,2)) AS "OrderTotal"
    		FROM "OrderItem"
    		GROUP BY "OrderInternalID"
    )
    ,"Orders" AS (
    	SELECT "CustomerInternalID"
    			,COUNT(*) AS "OrderCount"
    			,SUM(CASE WHEN "Order"."Status" IN('Open', 'New', 'Pending') THEN 1 ELSE 0 END) AS "IncompleteOrderCount"
    			,SUM(CASE WHEN "OrderItems"."OrderDiscount" > 0 THEN 1 ELSE 0 END) AS "DiscountedOrderCount"
    			,CAST(MAX("OrderItems"."OrderSubtotal") AS DECIMAL(11,2)) AS "HighestOrderSubtotal"
    			,CAST(MAX("OrderItems"."OrderDiscount") AS DECIMAL(11,2)) AS "HighestDiscount"
    			,CAST(MAX("OrderItems"."OrderTotal") AS DECIMAL(11,2)) AS "HighestOrderTotal"
    			,CAST(AVG("OrderItems"."OrderSubtotal") AS DECIMAL(11,2)) AS "AverageOrderSubtotal"
    			,CAST(AVG("OrderItems"."OrderDiscount") AS DECIMAL(11,2)) AS "AverageDiscount"
    			,CAST(AVG("OrderItems"."OrderTotal") AS DECIMAL(11,2)) AS "AverageOrderTotal"
    			,CAST(MIN("OrderItems"."OrderSubtotal") AS DECIMAL(11,2)) AS "LowestOrderSubtotal"
    			,CAST(MIN("OrderItems"."OrderDiscount") AS DECIMAL(11,2)) AS "LowestDiscount"
    			,CAST(MIN("OrderItems"."OrderTotal") AS DECIMAL(11,2)) AS "LowestOrderTotal"
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
    		   + CASE WHEN "Customer"."ContactMiddleName" IS NULL OR "Customer"."ContactMiddleName" = '' THEN '' ELSE SPACE(1) + "Customer"."ContactMiddleName" END
    		   + SPACE(1) + "Customer"."ContactLastName"
    		   + CASE WHEN "Customer"."ContactNickName" IS NULL OR "Customer"."ContactNickName" = '' THEN '' ELSE ' "' + "Customer"."ContactNickName" + '"' END
    		   AS "ContactFullName"
    		,"Customer"."BillingAddress1"
    		   + CASE WHEN "Customer"."BillingAddress2" IS NULL OR "Customer"."BillingAddress2" = '' THEN '' ELSE ', ' + "Customer"."BillingAddress2" END
    		   + CASE WHEN "Customer"."BillingAddress3" IS NULL OR "Customer"."BillingAddress3" = '' THEN '' ELSE ', ' + "Customer"."BillingAddress3" END
    		   + CASE WHEN "Customer"."BillingPostalCode" IS NULL OR "Customer"."BillingPostalCode" = '' THEN '' ELSE SPACE(1) + "Customer"."BillingPostalCode" END
    		   + CASE WHEN "Customer"."BillingCountry" IS NULL OR "Customer"."BillingCountry" = '' THEN '' ELSE ', ' + "Customer"."BillingCountry" END
    		   AS "BillingAddressLine"
    		,"Customer"."BillingAddress1"
    		   + CASE WHEN "Customer"."BillingAddress2" IS NULL OR "Customer"."BillingAddress2" = '' THEN '' ELSE CHAR(10) + "Customer"."BillingAddress2" END
    		   + CASE WHEN "Customer"."BillingAddress3" IS NULL OR "Customer"."BillingAddress3" = '' THEN '' ELSE CHAR(10) + "Customer"."BillingAddress3" END
    		   + CASE WHEN "Customer"."BillingPostalCode" IS NULL OR "Customer"."BillingPostalCode" = '' THEN '' ELSE SPACE(1) + "Customer"."BillingPostalCode" END
    		   + CASE WHEN "Customer"."BillingCountry" IS NULL OR "Customer"."BillingCountry" = '' THEN '' ELSE CHAR(10) + "Customer"."BillingCountry" END
    		   AS "BillingAddressBlock"
    		,CASE "Customer"."ParentInternalID" WHEN 0 THEN 0 ELSE 1 END AS "IsaSubCustomer"

    		
    -- Joins to Customer
    		,"ParentCustomer"."Name" AS "ParentCustomerName"
    		,"ParentCustomer"."ContactFirstName" 
    		   + CASE WHEN "ParentCustomer"."ContactMiddleName" IS NULL OR "ParentCustomer"."ContactMiddleName" = '' THEN '' ELSE SPACE(1) + "ParentCustomer"."ContactMiddleName" END
    		   + SPACE(1) + "ParentCustomer"."ContactLastName"
    		   + CASE WHEN "ParentCustomer"."ContactNickName" IS NULL OR "ParentCustomer"."ContactNickName" = '' THEN '' ELSE ' "' + "ParentCustomer"."ContactNickName" + '"' END
    		   AS "ParentCustomerContactFullname"
    		,"ParentCustomer"."Telephone" AS "ParentCustomerTelephone"
    		,"ParentCustomer"."BillingAddress1"
    		   + CASE WHEN "ParentCustomer"."BillingAddress2" IS NULL OR "ParentCustomer"."BillingAddress2" = '' THEN '' ELSE CHAR(10) + "ParentCustomer"."BillingAddress2" END
    		   + CASE WHEN "ParentCustomer"."BillingAddress3" IS NULL OR "ParentCustomer"."BillingAddress3" = '' THEN '' ELSE CHAR(10) + "ParentCustomer"."BillingAddress3" END
    		   + CASE WHEN "ParentCustomer"."BillingPostalCode" IS NULL OR "ParentCustomer"."BillingPostalCode" = '' THEN '' ELSE SPACE(1) + "ParentCustomer"."BillingPostalCode" END
    		   + CASE WHEN "ParentCustomer"."BillingCountry" IS NULL OR "ParentCustomer"."BillingCountry" = '' THEN '' ELSE CHAR(10) + "ParentCustomer"."BillingCountry" END
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
    
    ---- Join to Last Order and Shipping Address
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
    	OUTER APPLY (
    		SELECT TOP 1 "Order"."InternalID" AS "LastOrderID"
    				,"Order"."OrderDateTime" AS "LastOrderDateTime"
    				,"Order"."Status" AS "LastOrderStatus"
    				,"Order"."ShippingAddressInternalID" AS "LastUsedShippingAddressID"
    				,"ShippingAddress"."Name" AS "LastUsedShippingAddressName"
            		,"ShippingAddress"."Address1"
            		   + CASE WHEN "ShippingAddress"."Address2" IS NULL OR "ShippingAddress"."Address2" = '' THEN '' ELSE ', ' + "ShippingAddress"."Address2" END
            		   + CASE WHEN "ShippingAddress"."Address3" IS NULL OR "ShippingAddress"."Address3" = '' THEN '' ELSE ', ' + "ShippingAddress"."Address3" END
            		   + CASE WHEN "ShippingAddress"."PostalCode" IS NULL OR "ShippingAddress"."PostalCode" = '' THEN '' ELSE SPACE(1) + "ShippingAddress"."PostalCode" END
            		   + CASE WHEN "ShippingAddress"."Country" IS NULL OR "ShippingAddress"."Country" = '' THEN '' ELSE ', ' + "ShippingAddress"."Country" END
                       AS "LastUsedShippingAddressLine"
    			FROM "Order"
    			RIGHT JOIN "OrderItem" ON "OrderInternalID" = "Order"."InternalID"
    			LEFT JOIN "ShippingAddress" ON "ShippingAddress"."InternalID" = "Order"."ShippingAddressInternalID"
    			WHERE "Order"."CustomerInternalID" = "Customer"."InternalID"
    			ORDER BY "Order"."OrderDateTime" DESC, "Order"."InternalID" DESC
    		) AS "LastOrder"
    	LEFT JOIN "Customer" AS "ParentCustomer" ON "ParentCustomer"."InternalID" = "Customer"."ParentInternalID"
    	LEFT JOIN "SubCustomers" ON "SubCustomers"."ParentInternalID" = "Customer"."InternalID"
    	LEFT JOIN "ShippingAddresses" ON "ShippingAddresses"."CustomerInternalID" = "Customer"."InternalID"
    	LEFT JOIN "Orders" ON "Orders"."CustomerInternalID" = "Customer"."InternalID"
    	LEFT JOIN "OrderedProducts" ON "OrderedProducts"."CustomerInternalID" = "Customer"."InternalID"
		;
GO

	-- View Description
	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
	@name = N'MS_Description', @value = N'Customer All Details';

	-- Column Labels - Description (50 Chars)
	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_Customer_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'ContactFullName',
    @name=N'MS_Description', @value=N'Contact Full Name';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'BillingAddressLine',
	@name = N'MS_Description', @value = N'Billing Address Line';
	
	
	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'BillingAddressBlock',
	@name = N'MS_Description',  @value = N'Billing Address Block';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'IsaSubCustomer',
	@name = N'MS_Description',  @value = N'Is a Sub Customer';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ParentCustomerName',
	@name = N'MS_Description', @value = N'Parent Customer Name';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ParentCustomerContactFullname',
	@name = N'MS_Description', @value = N'Parent Customer Contact Full Name';

	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ParentCustomerTelephone',
	@name = N'MS_Description', @value = N'Parent Customer Telephone';

	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ParentCustomerAddressBlock',
	@name = N'MS_Description', @value = N'Parent Customer Address Block';

	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'SubCustomerCount',
    @name = N'MS_Description', @value = N'Sub Customer Count';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'SubCustomerList',
    @name = N'MS_Description', @value = N'Sub Customer List';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ShippingAddressCount',
    @name = N'MS_Description', @value = N'Shipping Address Count';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ShippingAddressList',
    @name = N'MS_Description', @value = N'Shipping Address List';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'OrderCount',
    @name = N'MS_Description', @value = N'Order Count';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'IncompleteOrderCount',
    @name = N'MS_Description', @value = N'Incomplete Order Count';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'DiscountedOrderCount',
    @name = N'MS_Description', @value = N'Discounted Order Count';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'HighestOrderSubtotal',
    @name = N'MS_Description', @value = N'Highest Order Subtotal';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'HighestDiscount',
    @name = N'MS_Description', @value = N'Highest Discount';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'HighestOrderTotal',
    @name = N'MS_Description', @value = N'Highest Order Total';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'AverageOrderSubtotal',
    @name = N'MS_Description', @value = N'Average Order Subtotal';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'AverageDiscount',
    @name = N'MS_Description', @value = N'Average Discount';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'AverageOrderTotal',
    @name = N'MS_Description', @value = N'Average Order Total';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LowestOrderSubtotal',
    @name = N'MS_Description', @value = N'Lowest Order Subtotal';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LowestDiscount',
    @name = N'MS_Description', @value = N'Lowest Discount';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LowestOrderTotal',
    @name = N'MS_Description', @value = N'Lowest Order Total';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LastOrderID',
    @name = N'MS_Description', @value = N'Last Order ID';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LastOrderDateTime',
    @name = N'MS_Description', @value = N'Last Order Date and Time';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LastOrderStatus',
    @name = N'MS_Description', @value = N'Last Order Status';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LastUsedShippingAddressID',
    @name = N'MS_Description', @value = N'Last Used Shipping Address ID';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LastUsedShippingAddressName',
    @name = N'MS_Description', @value = N'Last Used Shipping Address Name';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'LastUsedShippingAddressLine',
    @name = N'MS_Description', @value = N'Last Used Shipping Address Line';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'OrderedItemsCount',
    @name = N'MS_Description', @value = N'Ordered Items Count';

	EXEC sp_addextendedproperty 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_Customer_AllDetails',
    @level2type = N'COLUMN', @level2name = N'ProductsOrderedCount',
    @name = N'MS_Description', @value = N'Products Ordered Count';


 GO

