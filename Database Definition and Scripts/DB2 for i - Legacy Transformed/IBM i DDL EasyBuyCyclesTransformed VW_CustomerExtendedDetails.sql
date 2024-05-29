-- =============================================================================
-- DROP VIEW "EasyBuyCyclesSQL"."CustomerExtendedDetails";

CREATE OR REPLACE VIEW "EasyBuyCyclesSQL"."CustomerExtendedDetails"
   FOR SYSTEM NAME "YD1CVW1" (
      "InternalID",
      "IsASubCustomer",  "ParentInternalID",  "ParentCustomerName",
      "ParentRelationship",  "Name",  "LegalName",
      "ContactLastName",  "ContactFirstName",  "ContactMiddleName",
      "ContactNickName",  "ContactFullName",
      "BillingAddress1",  "BillingAddress2",  "BillingAddress3",
      "BillingPostalCode",  "BillingCountry",  "BillingAddressBlock",
      "BillingAddressLine",
      "Telephone",  "Email",  "Memo",  "PurchasePoints",
      "OrderCount",  "LastOrderDate",  "ShippingAddressCount",
      "CreateDate",  "CreateTime",  "CreateUser",  "CreateJob",
      "CreateJobNumber",
      "LastChangeDate",  "LastChangeTime",  "LastChangeUser",  "LastChangeJob",
      "LastChangeJobNumber"
   ) AS

   WITH Orders ("CustomerInternalID", "OrderCount", "LastOrderDate") AS (
      SELECT "CustomerInternalID", COUNT(*) AS "OrderCount",
         MAX("OrderDate") AS "LastOrderDate"
      FROM "EasyBuyCyclesSQL"."Order"
      GROUP BY "CustomerInternalID"
   ),
   ShippingAddresses ("CustomerInternalID", "ShippingAddressCount") AS (
      SELECT "CustomerInternalID", COUNT(*) AS "ShippingAddressCount"
      FROM "EasyBuyCyclesSQL"."ShippingAddress"
      GROUP BY "CustomerInternalID"
   )
   SELECT CS."InternalID",
      CASE WHEN CS."ParentInternalID" IS NULL THEN 0 ELSE 1 END AS "IsASubCustomer",
      CS."ParentInternalID", PC."Name" AS "ParentCustomerName", CS."ParentRelationship",
      CS."Name", CS."LegalName",
      CS."ContactLastName", CS."ContactFirstName", CS."ContactMiddleName", CS."ContactNickName",
      RTRIM(CS."ContactFirstName") CONCAT ' ' CONCAT RTRIM(CS."ContactMiddleName") CONCAT ' ' CONCAT RTRIM(CS."ContactLastName") CONCAT ' ' CONCAT RTRIM(CS."ContactNickName") AS "ContactFullName",
      CS."BillingAddress1", CS."BillingAddress2", CS."BillingAddress3", CS."BillingPostalCode", CS."BillingCountry",
      RTRIM(CS."BillingAddress1") CONCAT CHR(13) CONCAT RTRIM(CS."BillingAddress2") CONCAT CHR(13) CONCAT RTRIM(CS."BillingAddress3") CONCAT ' ' CONCAT RTRIM(CS."BillingPostalCode") CONCAT CHR(13) CONCAT RTRIM(CS."BillingCountry") AS "BillingAddressBlock",
      RTRIM(CS."BillingAddress1") CONCAT ', ' CONCAT RTRIM(CS."BillingAddress2") CONCAT ', ' CONCAT RTRIM(CS."BillingAddress3") CONCAT ', ' CONCAT RTRIM(CS."BillingPostalCode") CONCAT ', ' CONCAT RTRIM(CS."BillingCountry") AS "BillingAddressLine",
      CS."Telephone", CS."Email", CS."Memo", CS."PurchasePoints",
      Orders."OrderCount", Orders."LastOrderDate",
      ShippingAddresses."ShippingAddressCount",
      CS."CreateDate", CS."CreateTime", CS."CreateUser", CS."CreateJob", CS."CreateJobNumber",
      CS."LastChangeDate", CS."LastChangeTime", CS."LastChangeUser", CS."LastChangeJob", CS."LastChangeJobNumber"
  
      FROM "EasyBuyCyclesSQL"."Customer" CS
      LEFT JOIN "EasyBuyCyclesSQL"."Customer" PC
         ON PC."InternalID" = CS."ParentInternalID"
      LEFT JOIN Orders
         ON Orders."CustomerInternalID" = CS."InternalID"
      LEFT JOIN ShippingAddresses
         ON ShippingAddresses."CustomerInternalID" = CS."InternalID";

LABEL ON TABLE "EasyBuyCyclesSQL"."CustomerExtendedDetails" IS
   'Customer Extended Details';
