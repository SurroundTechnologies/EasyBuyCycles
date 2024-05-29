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
Source Member.: SQL Server SQL EasyBuyCyclesModern VW_ShippingAddress_AllDetails.sql
Created by ...: Lee Paul
Created on ...: September 14, 2022
Object Type ..: View Definition
Description ..: All ShippingAddress Details

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM3/QDBSRC) SRCMBR("ShippingAddress")
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

DROP VIEW IF EXISTS "VW_ShippingAddress_AllDetails";

GO

CREATE OR ALTER VIEW "VW_ShippingAddress_AllDetails"
    AS
    WITH "Orders" AS (
    	SELECT "CustomerInternalID"
    			,COUNT(*) AS "OrderCount"
    		FROM "Order"
    		GROUP BY "CustomerInternalID"
    )
    SELECT "ShippingAddress"."InternalID"
    		,"ShippingAddress"."CustomerInternalID"
    		,"ShippingAddress"."Name"
    		,"ShippingAddress"."ContactLastName"
    		,"ShippingAddress"."ContactFirstName"
    		,"ShippingAddress"."ContactMiddleName"
    		,"ShippingAddress"."ContactNickName"
    		,"ShippingAddress"."Address1"
    		,"ShippingAddress"."Address2"
    		,"ShippingAddress"."Address3"
    		,"ShippingAddress"."PostalCode"
    		,"ShippingAddress"."Country"
    		,"ShippingAddress"."Telephone"
    		,"ShippingAddress"."Email"
    		,"ShippingAddress"."Memo"
    		,"ShippingAddress"."PurchasePoints"
    
    -- Calculated Columns
    		,"ShippingAddress"."ContactFirstName" 
    		   + CASE WHEN "ShippingAddress"."ContactMiddleName" IS NULL OR "ShippingAddress"."ContactMiddleName" = '' THEN '' ELSE SPACE(1) + "ShippingAddress"."ContactMiddleName" END
    		   + SPACE(1) + "ShippingAddress"."ContactLastName"
    		   + CASE WHEN "ShippingAddress"."ContactNickName" IS NULL OR "ShippingAddress"."ContactNickName" = ''  THEN '' ELSE ' "' + "ShippingAddress"."ContactNickName" + '"' END
    		   AS "ContactFullName"
    		,"ShippingAddress"."Address1"
    		   + CASE WHEN "ShippingAddress"."Address2" IS NULL OR "ShippingAddress"."Address2" = '' THEN '' ELSE ', ' + "ShippingAddress"."Address2" END
    		   + CASE WHEN "ShippingAddress"."Address3" IS NULL OR "ShippingAddress"."Address3" = '' THEN '' ELSE ', ' + "ShippingAddress"."Address3" END
    		   + CASE WHEN "ShippingAddress"."PostalCode" IS NULL OR "ShippingAddress"."PostalCode" = ''THEN '' ELSE SPACE(1) + "ShippingAddress"."PostalCode" END
    		   + CASE WHEN "ShippingAddress"."Country" IS NULL OR "ShippingAddress"."Country" = '' THEN '' ELSE ', ' + "ShippingAddress"."Country" END
    		   AS "AddressLine"
            ,"ShippingAddress"."Address1"
    		   + CASE WHEN "ShippingAddress"."Address2" IS NULL OR "ShippingAddress"."Address2" = '' THEN '' ELSE CHAR(10) + "ShippingAddress"."Address2" END
    		   + CASE WHEN "ShippingAddress"."Address3" IS NULL OR "ShippingAddress"."Address3" = '' THEN '' ELSE CHAR(10) + "ShippingAddress"."Address3" END
    		   + CASE WHEN "ShippingAddress"."PostalCode" IS NULL OR "ShippingAddress"."PostalCode" = '' THEN '' ELSE SPACE(1) + "ShippingAddress"."PostalCode" END
    		   + CASE WHEN "ShippingAddress"."Country" IS NULL OR "ShippingAddress"."Country" = '' THEN '' ELSE CHAR(10) + "ShippingAddress"."Country" END
    		   AS "AddressBlock"
    		
    -- Joins to Customer
    		,"Customer"."Name" AS "CustomerName"
    
    -- Join to Order
    		,"Orders"."OrderCount"
    
    -- Audit Stamps
            ,"ShippingAddress"."CreatedAt" AS "CreatedAt"
            ,"ShippingAddress"."CreatedBy" AS "CreatedBy"
            ,"ShippingAddress"."CreatedWith" AS "CreatedWith"
            ,"ShippingAddress"."LastModifiedAt" AS "ModifiedAt"
            ,"ShippingAddress"."LastModifiedBy" AS "ModifiedBy"
            ,"ShippingAddress"."LastModifiedWith" AS "ModifiedWith"
    
    	FROM "ShippingAddress"
    	LEFT JOIN "Customer" AS "Customer" ON "Customer"."InternalID" = "ShippingAddress"."CustomerInternalID"
    	LEFT JOIN "Orders" ON "Orders"."CustomerInternalID" = "Customer"."InternalID"
;

GO

-- View Description
	EXEC sp_addextendedproperty
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'VIEW', @level1name = N'VW_ShippingAddress_AllDetails',
	@name = N'MS_Description', @value = N'Shipping Address All Details';

-- Column Labels - Description (50 Chars)
	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_ShippingAddress_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'CustomerName',
    @name=N'MS_Description', @value=N'Customer Name';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_ShippingAddress_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'AddressLine',
    @name=N'MS_Description', @value=N'Address Line';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_ShippingAddress_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'AddressBlock',
    @name=N'MS_Description', @value=N'Address Block';

	EXEC sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'VIEW',    @level1name=N'VW_ShippingAddress_AllDetails',
	@level2type=N'COLUMN',   @level2name=N'OrderCount',
    @name=N'MS_Description', @value=N'Order Count';

