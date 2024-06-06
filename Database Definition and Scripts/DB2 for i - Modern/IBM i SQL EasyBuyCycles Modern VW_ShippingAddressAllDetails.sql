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
Source Member.: IBM i SQL EasyBuyCyclesModern VW_ShippingAddress_AllDetails.sql
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

SET SCHEMA "EasyBuyCycles";
-- SET SCHEMA "EasyBuyCyclesDev";

DROP VIEW IF EXISTS "VW_ShippingAddress_AllDetails";

CREATE OR REPLACE VIEW "VW_ShippingAddress_AllDetails"
    FOR SYSTEM NAME "YD1SV01" AS
    
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
    		   CONCAT CASE "ShippingAddress"."ContactMiddleName" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."ContactMiddleName" END
    		   CONCAT SPACE(1) CONCAT "ShippingAddress"."ContactLastName"
    		   CONCAT CASE "ShippingAddress"."ContactNickName" WHEN '' THEN '' ELSE ' "' CONCAT "ShippingAddress"."ContactNickName" CONCAT '"' END
    		   AS "ContactFullName"
    		,"ShippingAddress"."Address1"
    		   CONCAT CASE "ShippingAddress"."Address2" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Address2" END
    		   CONCAT CASE "ShippingAddress"."Address3" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Address3" END
    		   CONCAT CASE "ShippingAddress"."PostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."PostalCode" END
    		   CONCAT CASE "ShippingAddress"."Country" WHEN '' THEN '' ELSE ', ' CONCAT "ShippingAddress"."Country" END
    		   AS "AddressLine"
            ,"ShippingAddress"."Address1"
    		   CONCAT CASE "ShippingAddress"."Address2" WHEN '' THEN '' ELSE CHR(13) CONCAT "ShippingAddress"."Address2" END
    		   CONCAT CASE "ShippingAddress"."Address3" WHEN '' THEN '' ELSE CHR(13) CONCAT "ShippingAddress"."Address3" END
    		   CONCAT CASE "ShippingAddress"."PostalCode" WHEN '' THEN '' ELSE SPACE(1) CONCAT "ShippingAddress"."PostalCode" END
    		   CONCAT CASE "ShippingAddress"."Country" WHEN '' THEN '' ELSE CHR(13) CONCAT "ShippingAddress"."Country" END
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

-- View Description
LABEL ON TABLE "VW_ShippingAddress_AllDetails"
   IS 'Shipping Address All Details';

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "VW_ShippingAddress_AllDetails" (
     "CustomerName"          TEXT IS 'Customer Name'
     ,"AddressLine"          TEXT IS 'Address Line'
     ,"AddressBlock"         TEXT IS 'Address Block'
     ,"OrderCount"           TEXT IS 'Order Count'
);

-- Grant View Access
GRANT SELECT
ON "VW_ShippingAddress_AllDetails" TO PUBLIC WITH GRANT OPTION ;
GRANT ALTER, REFERENCES, SELECT
ON "VW_ShippingAddress_AllDetails" TO QPGMR WITH GRANT OPTION ;
