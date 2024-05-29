-- =============================================================================
-- CONFIDENTIAL AND PROPRIETARY INFORMATION
--
-- The receipt or possession of this document does not convey any rights to
-- reproduce, disclose its contents or manufacture, use or sell anything it may
-- describe.  Reproduction, disclosure, or use without the specific written
-- authorization of Surround Technologies, LLC is strictly forbidden.
--
-- Copyright ....: (C) Surround Technologies, LLC 1998 - 2021
--                 9197 Estero River Circle
--                 Estero, FL 33928
--                 973-743-1277
--                 http://www.surroundtech.com/
--                  
-- =============================================================================
-- Source File...: QDBSRC
-- Source Member.: "Customer"
-- Created by ...: Lee Paul
-- Created on ...: August 29, 2021
-- Object Type ..: Table Definition
-- Description ..: Customer
--
-- BUILD COMMAND.:
-- RUNSQLSTM SRCFILE(EASYBUYDM2/QDBSRC) SRCMBR("Customer")
--     COMMIT(*NONE) ERRLVL(20)
--
-- =============================================================================
--                              Amendment Details
--                              -----------------
-- Date       Programmer      Description
-- ---------- --------------- --------------------------------------------------
-- 
--
-- =============================================================================

SET SCHEMA "EasyBuyCyclesTransformed";
-- SET SCHEMA "EasyBuyCyclesTransformedDev";

-- ALTER TABLE "Customer"
--    DROP CONSTRAINT "PK_Customer",
--    DROP CONSTRAINT "FK_Customer_{ReferenceTable}";

DROP INDEX IF EXISTS "UQ_Customer_Name";
DROP INDEX IF EXISTS "IX_Customer_ParentInternalID_Name";
DROP INDEX IF EXISTS "IX_Customer_LegalName_Name";
DROP INDEX IF EXISTS "IX_Customer_ContactLastName_ContactFirstName_Name";
DROP INDEX IF EXISTS "IX_Customer_BillingPostalCode_Name";
DROP INDEX IF EXISTS "IX_Customer_Telephone_Name";
DROP INDEX IF EXISTS "IX_Customer_PurchasePoints_Name";

-- DROP VIEW IF EXISTS "VW_{ViewName}";

-- DROP ALIAS IF EXISTS "{AliasName}";

-- DROP TABLE IF EXISTS "Customer";

CREATE OR REPLACE TABLE "Customer"
    (
     "InternalID" FOR "YD1CIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "ParentInternalID" FOR "YD1CPTID" NUMERIC(8,0),
     "ParentRelationship" FOR "YD1CPTRL" CHAR(20),
     "Name" FOR "YD1CNM" CHAR(50) NOT NULL DEFAULT ' ',
     "LegalName" FOR "YD1CNMLG" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactLastName" FOR "YD1CCNLN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactFirstName" FOR "YD1CCNFN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactMiddleName" FOR "YD1CCNMN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactNickName" FOR "YD1CCNNN" CHAR(50) NOT NULL DEFAULT ' ',
     "BillingAddress1" FOR "YD1CBLA1" CHAR(30) NOT NULL DEFAULT ' ',
     "BillingAddress2" FOR "YD1CBLA2" CHAR(30) NOT NULL DEFAULT ' ',
     "BillingAddress3" FOR "YD1CBLA3" CHAR(30) NOT NULL DEFAULT ' ',
     "BillingPostalCode" FOR "YD1CBLPC" CHAR(10) NOT NULL DEFAULT ' ',
     "BillingCountry" FOR "YD1CBLCY" CHAR(50) NOT NULL DEFAULT ' ',
     "Telephone" FOR "YD1CTL" CHAR(20) NOT NULL DEFAULT ' ',
     "Email" FOR "YD1CEM" CHAR(50) NOT NULL DEFAULT ' ',
     "Memo" FOR "YD1CM1" CHAR(100) NOT NULL DEFAULT ' ',
     "PurchasePoints" FOR "YD1CPRPT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateDate" FOR "YD1CCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime" FOR "YD1CCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser" FOR "YD1CCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob" FOR "YD1CCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber" FOR "YD1CCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate" FOR "YD1CLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime" FOR "YD1CLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser" FOR "YD1CLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob" FOR "YD1CLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber" FOR "YD1CLCJN" CHAR(6) NOT NULL DEFAULT ' ',

      -- Primary Key Constraint
      CONSTRAINT "PK_Customer"
         PRIMARY KEY ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_Customer_ParentCustomer"
         FOREIGN KEY ("ParentInternalID")
         REFERENCES "Customer"("InternalID")
         ON DELETE RESTRICT ON UPDATE RESTRICT
   )
RCDFMT YD1CPR
ON REPLACE PRESERVE ROWS;

-- Table Description
LABEL ON TABLE "Customer"
   IS 'Customer';

-- Column Labels
LABEL ON COLUMN "Customer" (
     "InternalID" TEXT IS 'Internal ID',
     "ParentInternalID" TEXT IS 'Parent Internal ID',
     "ParentRelationship" TEXT IS 'Parent Relationship',
     "Name" TEXT IS 'Name',
     "LegalName" TEXT IS 'Legal Name',
     "ContactLastName" TEXT IS 'Contact Last Name',
     "ContactFirstName" TEXT IS 'Contact First Name',
     "ContactMiddleName" TEXT IS 'Contact Middle Name',
     "ContactNickName" TEXT IS 'Contact Nick Name',
     "BillingAddress1" TEXT IS 'Billing Address 1',
     "BillingAddress2" TEXT IS 'Billing Address 2',
     "BillingAddress3" TEXT IS 'Billing Address 3',
     "BillingPostalCode" TEXT IS 'Billing Postal Code',
     "BillingCountry" TEXT IS 'Billing Country',
     "Telephone" TEXT IS 'Telephone',
     "Email" TEXT IS 'Email',
     "Memo" TEXT IS 'Memo',
     "PurchasePoints" TEXT IS 'Purchase Points',
     "CreateDate" TEXT IS 'Create Date',
     "CreateTime" TEXT IS 'Create Time',
     "CreateUser" TEXT IS 'Create User',
     "CreateJob" TEXT IS 'Create Job',
     "CreateJobNumber" TEXT IS 'Create Job Number',
     "LastChangeDate" TEXT IS 'Last Change Date',
     "LastChangeTime" TEXT IS 'Last Change Time',
     "LastChangeUser" TEXT IS 'Last Change User',
     "LastChangeJob" TEXT IS 'Last Change Job',
     "LastChangeJobNumber" TEXT IS 'Last Change Job Number'
     );

-- Column Labels
LABEL ON COLUMN "Customer" (
     "InternalID" IS 'Internal ID',
     "ParentInternalID" IS 'Parent              Internal ID',
     "ParentRelationship" IS 'Parent              Relationship',
     "Name" IS 'Name',
     "LegalName" IS 'Legal Name',
     "ContactLastName" IS 'Contact             Last Name',
     "ContactFirstName" IS 'Contact             First Name',
     "ContactMiddleName" IS 'Contact             Middle Name',
     "ContactNickName" IS 'Contact             Nick Name',
     "BillingAddress1" IS 'Billing             Address 1',
     "BillingAddress2" IS 'Billing             Address 2',
     "BillingAddress3" IS 'Billing             Address 3',
     "BillingPostalCode" IS 'Billing             Postal Code',
     "BillingCountry" IS 'Billing             Country',
     "Telephone" IS 'Telephone',
     "Email" IS 'Email',
     "Memo" IS 'Memo',
     "PurchasePoints" IS 'Purchase Points',
     "CreateDate" IS 'Create Date',
     "CreateTime" IS 'Create Time',
     "CreateUser" IS 'Create User',
     "CreateJob" IS 'Create Job',
     "CreateJobNumber" IS 'Create              Job Number',
     "LastChangeDate" IS 'Last Change         Date',
     "LastChangeTime" IS 'Last Change         Time',
     "LastChangeUser" IS 'Last Change         User',
     "LastChangeJob" IS 'Last Change         Job',
     "LastChangeJobNumber" IS 'Last Change         Job Number'
     );

CREATE UNIQUE INDEX "UQ_Customer_Name"
   FOR SYSTEM NAME YD1CIXU1 ON "Customer" (
   "Name"
   );
LABEL ON INDEX "UQ_Customer_Name"
   IS 'Sort by Customer Name';

CREATE INDEX "IX_Customer_ParentInternalID_Name"
   FOR SYSTEM NAME "YD1CIX2"
   ON "Customer" (
   "ParentInternalID", "Name"
   );
LABEL ON INDEX "IX_Customer_ParentInternalID_Name"
   IS 'Sort by Parrent Internal ID';

CREATE INDEX "IX_Customer_LegalName_Name"
   FOR SYSTEM NAME "YD1CIX3"
   ON "Customer" (
   "LegalName", "Name"
   );
LABEL ON INDEX "IX_Customer_LegalName_Name"
   IS 'Sort by Legal Name';

CREATE INDEX "IX_Customer_ContactLastName_ContactFirstName_Name"
   FOR SYSTEM NAME "YD1CIX4"
   ON "Customer" (
   "ContactLastName", "ContactFirstName", "Name"
   );
LABEL ON INDEX "IX_Customer_ContactLastName_ContactFirstName_Name"
   IS 'Sort by Contact Last and First Names';

CREATE INDEX "IX_Customer_BillingPostalCode_Name"
   FOR SYSTEM NAME "YD1CIX5"
   ON "Customer" (
   "BillingPostalCode", "Name"
   );
LABEL ON INDEX "IX_Customer_BillingPostalCode_Name"
   IS 'Sort by Billing Postal Code';

CREATE INDEX "IX_Customer_Telephone_Name"
   FOR SYSTEM NAME "YD1CIX6"
   ON "Customer" (
   "Telephone", "Name"
   );
LABEL ON INDEX "IX_Customer_Telephone_Name"
   IS 'Sort by Telephone';

CREATE INDEX "IX_Customer_PurchasePoints_Name"
   FOR SYSTEM NAME "YD1CIX7"
   ON "Customer" (
   "PurchasePoints", "Name"
   );
LABEL ON INDEX "IX_Customer_PurchasePoints_Name"
   IS 'Sort by Purchase Points';

-- Aliases
-- CREATE OR REPLACE ALIAS  FOR "Customer";

GRANT DELETE, INSERT, SELECT, UPDATE
ON "Customer" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "Customer" TO QPGMR WITH GRANT OPTION ;

SET SCHEMA DEFAULT;
-- =============================================================================
-- CONFIDENTIAL AND PROPRIETARY INFORMATION
--
-- The receipt or possession of this document does not convey any rights to
-- reproduce, disclose its contents or manufacture, use or sell anything it may
-- describe.  Reproduction, disclosure, or use without the specific written
-- authorization of Surround Technologies, LLC is strictly forbidden.
--
-- Copyright ....: (C) Surround Technologies, LLC 1998 - 2021
--                 9197 Estero River Circle
--                 Estero, FL 33928
--                 973-743-1277
--                 http://www.surroundtech.com/
--                  
-- =============================================================================
-- Source File...: QDBSRC
-- Source Member.: "OrdItm"
-- Created by ...: Lee Paul
-- Created on ...: August 29, 2021
-- Object Type ..: Table Definition
-- Description ..: Order Item
--
-- BUILD COMMAND.:
-- RUNSQLSTM SRCFILE(EASYBUYSQL/QDBSRC) SRCMBR("OrdItm")
--     COMMIT(*NONE) ERRLVL(20)
--
-- =============================================================================
--                              Amendment Details
--                              -----------------
-- Date       Programmer      Description
-- ---------- --------------- --------------------------------------------------
-- 
--
-- =============================================================================

SET SCHEMA "EasyBuyCyclesSQL";

-- ALTER TABLE "OrderItem"
--    DROP CONSTRAINT "PK_OrderItem";
--    DROP CONSTRAINT "FK_OrderItem_{ReferenceTable}";

-- DROP INDEX IF EXISTS "UQ_OrderItem_{IndexDescriptor}";
DROP INDEX IF EXISTS "IX_OrderItem_OrderInternalID_ProductInternalID_InternalID";
DROP INDEX IF EXISTS "IX_OrderItem_ProductInternalID_OrderInternalID_InternalID";
DROP INDEX IF EXISTS "IX_OrderItem_OrderInternalID_InternalID";

-- DROP VIEW IF EXISTS "VW_{ViewName}_{ViewDescriptor}";

-- DROP ALIAS IF EXISTS "{AliasName}";

CREATE OR REPLACE TABLE "OrderItem"
    FOR SYSTEM NAME "OrdItm"  (
     "InternalID" FOR "YD1IIID" NUMERIC(12,0) NOT NULL DEFAULT 0,
     "OrderInternalID" FOR "YD1I1OID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "ProductInternalID" FOR "YD1I1PID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "Quantity" FOR "YD1IQT" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "UnitPrice" FOR "YD1IPRUN" NUMERIC(8,2) NOT NULL DEFAULT 0,
     "Discount(Percent)" FOR "YD1IDSPC" NUMERIC(4,1) NOT NULL DEFAULT 0,
     "Memo" FOR "YD1IM1" CHAR(100) NOT NULL DEFAULT ' ',
     "CreateDate" FOR "YD1ICRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime" FOR "YD1ICRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser" FOR "YD1ICRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob" FOR "YD1ICRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber" FOR "YD1ICRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate" FOR "YD1ILCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime" FOR "YD1ILCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser" FOR "YD1ILCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob" FOR "YD1ILCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber" FOR "YD1ILCJN" CHAR(6) NOT NULL DEFAULT ' ',

      -- Primary Key Constraint
      CONSTRAINT "PK_OrderItem"
         PRIMARY KEY ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_OrderItem_Order"
         FOREIGN KEY ("OrderInternalID")
         REFERENCES "Order"("InternalID")
         ON DELETE RESTRICT ON UPDATE RESTRICT,
      CONSTRAINT "FK_OrderItem_Product"
         FOREIGN KEY ("ProductInternalID")
         REFERENCES "Product"("InternalID")
         ON DELETE RESTRICT ON UPDATE RESTRICT
   )
RCDFMT YD1IPR
ON REPLACE PRESERVE ROWS;

-- Table Description
LABEL ON TABLE "OrderItem"
   IS 'Order Item';

-- Column Labels
LABEL ON COLUMN "OrderItem" (
     "InternalID" TEXT IS 'Internal ID',
     "OrderInternalID" TEXT IS 'Order Internal ID',
     "ProductInternalID" TEXT IS 'Product Internal ID',
     "Quantity" TEXT IS 'Quantity',
     "UnitPrice" TEXT IS 'Unit Price',
     "Discount(Percent)" TEXT IS 'Discount (Percent)',
     "Memo" TEXT IS 'Memo',
     "CreateDate" TEXT IS 'Create Date',
     "CreateTime" TEXT IS 'Create Time',
     "CreateUser" TEXT IS 'Create User',
     "CreateJob" TEXT IS 'Create Job',
     "CreateJobNumber" TEXT IS 'Create Job Number',
     "LastChangeDate" TEXT IS 'Last Change Date',
     "LastChangeTime" TEXT IS 'Last Change Time',
     "LastChangeUser" TEXT IS 'Last Change User',
     "LastChangeJob" TEXT IS 'Last Change Job',
     "LastChangeJobNumber" TEXT IS 'Last Change Job Number'
     );

-- Column Labels
LABEL ON COLUMN "OrderItem" (
     "InternalID" IS 'Internal ID',
     "OrderInternalID" IS 'Order               Internal ID',
     "ProductInternalID" IS 'Product             Internal ID',
     "Quantity" IS 'Quantity',
     "UnitPrice" IS 'Unit Price',
     "Discount(Percent)" IS 'Discount (Percent)',
     "Memo" IS 'Memo',
     "CreateDate" IS 'Create Date',
     "CreateTime" IS 'Create Time',
     "CreateUser" IS 'Create User',
     "CreateJob" IS 'Create Job',
     "CreateJobNumber" IS 'Create              Job Number',
     "LastChangeDate" IS 'Last Change         Date',
     "LastChangeTime" IS 'Last Change         Time',
     "LastChangeUser" IS 'Last Change         User',
     "LastChangeJob" IS 'Last Change         Job',
     "LastChangeJobNumber" IS 'Last Change         Job Number'
     );

-- CREATE UNIQUE |WHERE NOT NULL| INDEX "UQ_OrderItem_#"
--    FOR SYSTEM NAME "{IndexSystemName}"
--    ON "OrderItem" (
--    {IndexColumns}
--    WHERE {FilterCondition}
--    );
-- LABEL ON INDEX "UQ_OrderItem_#"
--    IS 'Sort by {IndexColumnDescriptions}';

CREATE INDEX "IX_OrderItem_OrderInternalID_ProductInternalID_InternalID"
   FOR SYSTEM NAME "YD1IIX1"
   ON "OrderItem" (
   "OrderInternalID", "ProductInternalID", "InternalID"
   );
LABEL ON INDEX "IX_OrderItem_OrderInternalID_ProductInternalID_InternalID"
   IS 'Sort by Order and Product Internal IDs';

CREATE INDEX "IX_OrderItem_ProductInternalID_OrderInternalID_InternalID"
   FOR SYSTEM NAME "YD1IIX2"
   ON "OrderItem" (
   "ProductInternalID", "OrderInternalID", "InternalID"
   );
LABEL ON INDEX "IX_OrderItem_ProductInternalID_OrderInternalID_InternalID"
   IS 'Sort by Product and Order Internal IDs';

CREATE INDEX "IX_OrderItem_OrderInternalID_InternalID"
   FOR SYSTEM NAME "YD1IIX3"
   ON "OrderItem" (
   "OrderInternalID", "InternalID"
   );
LABEL ON INDEX "IX_OrderItem_OrderInternalID_InternalID"
   IS 'Sort by Order Internal ID';

-- Aliases
-- CREATE OR REPLACE ALIAS OrdItm FOR "OrderItem";

GRANT DELETE, INSERT, SELECT, UPDATE
ON "OrderItem" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "OrderItem" TO QPGMR WITH GRANT OPTION ;

SET SCHEMA DEFAULT;
-- =============================================================================
-- CONFIDENTIAL AND PROPRIETARY INFORMATION
--
-- The receipt or possession of this document does not convey any rights to
-- reproduce, disclose its contents or manufacture, use or sell anything it may
-- describe.  Reproduction, disclosure, or use without the specific written
-- authorization of Surround Technologies, LLC is strictly forbidden.
--
-- Copyright ....: (C) Surround Technologies, LLC 1998 - 2021
--                 9197 Estero River Circle
--                 Estero, FL 33928
--                 973-743-1277
--                 http://www.surroundtech.com/
--                  
-- =============================================================================
-- Source File...: QDBSRC
-- Source Member.: "Order"
-- Created by ...: Lee Paul
-- Created on ...: August 29, 2021
-- Object Type ..: Table Definition
-- Description ..: Order
--
-- BUILD COMMAND.:
-- RUNSQLSTM SRCFILE(EASYBUYSQL/QDBSRC) SRCMBR("Order")
--     COMMIT(*NONE) ERRLVL(20)
--
-- =============================================================================
--                              Amendment Details
--                              -----------------
-- Date       Programmer      Description
-- ---------- --------------- --------------------------------------------------
-- 
--
-- =============================================================================

SET SCHEMA "EasyBuyCyclesSQL";

-- ALTER TABLE "Order"
--    DROP CONSTRAINT "PK_Order",
--    DROP CONSTRAINT "FK_Order_{ReferenceTable}";

-- DROP INDEX IF EXISTS "UQ_Order_{IndexDescriptor}";
DROP INDEX IF EXISTS "IX_Order_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS "IX_Order_CustomerInternalID_InternalID";
DROP INDEX IF EXISTS
   "IX_Order_CustomerInternalID_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS
   "IX_Order_CustomerInternalID_ PO_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS "IX_Order_PO_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS "IX_Order_SalesPersonName_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS "IX_Order_WarehouseName_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS "IX_Order_Status_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS 
   "IX_Order_CustomerInternalID_Status_OrderDate_OrderTime_InternalID";
DROP INDEX IF EXISTS
   "IX_Order_ShippingAddressInternalID_OrderDate_OrderTime_InternalID";

-- DROP VIEW IF EXISTS "VW_{ViewName}_{ViewDescriptor}";

-- DROP ALIAS IF EXISTS "{AliasName}";

-- DROP TABLE IF EXISTS "Order";

CREATE OR REPLACE TABLE "Order"
    (
     "InternalID" FOR "YD1OIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CustomerInternalID" FOR "YD1O1CID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "OrderDate" FOR "YD1ODT" DATE NOT NULL DEFAULT CURRENT_DATE,
     "OrderTime" FOR "YD1OTM" TIME NOT NULL DEFAULT CURRENT_TIME,
     "PurchaseOrderNumber/ID" FOR "YD1OPONO" CHAR(50) NOT NULL DEFAULT ' ',
     "WarehouseInternalID" FOR "YD1O1WID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "WarehouseName" FOR "YD1O1WNM" CHAR(50) NOT NULL DEFAULT ' ',
     "DeliveryMemo" FOR "YD1ODLM1" CHAR(100) NOT NULL DEFAULT ' ',
     "ShippingAddressInternalID" FOR "YD1O1SID" NUMERIC(8,0)
          NOT NULL DEFAULT 0,
     "OrderMemo" FOR "YD1OM1" CHAR(100) NOT NULL DEFAULT ' ',
     "Status" FOR "YD1OST" CHAR(10) NOT NULL DEFAULT ' ',
     "SalesPersonInternalID" FOR "YD1O1AID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "SalesPersonName" FOR "YD1O1ANM" CHAR(50) NOT NULL DEFAULT ' ',
     "PurchasePoints" FOR "YD1CPRPT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateDate" FOR "YD1OCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime" FOR "YD1OCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser" FOR "YD1OCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob" FOR "YD1OCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber" FOR "YD1OCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate" FOR "YD1OLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime" FOR "YD1OLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser" FOR "YD1OLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob" FOR "YD1OLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber" FOR "YD1OLCJN" CHAR(6) NOT NULL DEFAULT ' ',

      -- Primary Key Constraint
      CONSTRAINT "PK_Order"
         PRIMARY KEY ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_Order_Customer"
         FOREIGN KEY ("CustomerInternalID")
         REFERENCES "Customer"("InternalID")
         ON DELETE RESTRICT ON UPDATE RESTRICT,
      CONSTRAINT "FK_Order_ShippingAddress"
         FOREIGN KEY ("ShippingAddressInternalID")
         REFERENCES "ShippingAddress"("InternalID")
         ON DELETE RESTRICT ON UPDATE RESTRICT
   )
RCDFMT YD1OPR
ON REPLACE PRESERVE ROWS;

-- Table Description
LABEL ON TABLE "Order"
   IS 'Order';

-- Column Labels
LABEL ON COLUMN "Order" (
     "InternalID" TEXT IS 'Internal ID',
     "CustomerInternalID" TEXT IS 'Customer Internal ID',
     "OrderDate" TEXT IS 'Order Date',
     "OrderTime" TEXT IS 'Order Time',
     "PurchaseOrderNumber/ID" TEXT IS 'Purchase Order Number/ID',
     "WarehouseInternalID" TEXT IS 'Warehouse Internal ID',
     "WarehouseName" TEXT IS 'Warehouse Name',
     "DeliveryMemo" TEXT IS 'Delivery Memo',
     "ShippingAddressInternalID" TEXT IS 'Shipping Address Internal ID',
     "OrderMemo" TEXT IS 'Order Memo',
     "Status" TEXT IS 'Status',
     "SalesPersonInternalID" TEXT IS 'Sales Person Internal ID',
     "SalesPersonName" TEXT IS 'Sales Person Name',
     "PurchasePoints" TEXT IS 'Purchase Points',
     "CreateDate" TEXT IS 'Create Date',
     "CreateTime" TEXT IS 'Create Time',
     "CreateUser" TEXT IS 'Create User',
     "CreateJob" TEXT IS 'Create Job',
     "CreateJobNumber" TEXT IS 'Create Job Number',
     "LastChangeDate" TEXT IS 'Last Change Date',
     "LastChangeTime" TEXT IS 'Last Change Time',
     "LastChangeUser" TEXT IS 'Last Change User',
     "LastChangeJob" TEXT IS 'Last Change Job',
     "LastChangeJobNumber" TEXT IS 'Last Change Job Number'
     );

-- Column Labels
LABEL ON COLUMN "Order" (
     "InternalID" IS 'Internal ID',
     "CustomerInternalID" IS 'Customer            Internal ID',
     "OrderDate" IS 'Order Date',
     "OrderTime" IS 'Order Time',
     "PurchaseOrderNumber/ID" IS 'Purchase Order      Number/Id',
     "WarehouseInternalID" IS 'Warehouse           Internal ID',
     "WarehouseName" IS 'Warehouse           Name',
     "DeliveryMemo" IS 'Delivery Memo',
     "ShippingAddressInternalID" IS 'Shipping Address    Internal ID',
     "OrderMemo" IS 'Order Memo',
     "Status" IS 'Status',
     "SalesPersonInternalID" IS 'Sales Person        Internal ID',
     "SalesPersonName" IS 'Sales Person        Name',
     "PurchasePoints" IS 'Purchase Points',
     "CreateDate" IS 'Create Date',
     "CreateTime" IS 'Create Time',
     "CreateUser" IS 'Create User',
     "CreateJob" IS 'Create Job',
     "CreateJobNumber" IS 'Create              Job Number',
     "LastChangeDate" IS 'Last Change         Date',
     "LastChangeTime" IS 'Last Change         Time',
     "LastChangeUser" IS 'Last Change         User',
     "LastChangeJob" IS 'Last Change         Job',
     "LastChangeJobNumber" IS 'Last Change         Job Number'
     );

-- CREATE UNIQUE |WHERE NOT NULL| INDEX "UQ_Order_#"
--    FOR SYSTEM NAME "{IndexSystemName}"
--    ON "Order" (
--    {IndexColumns}
--    WHERE {FilterCondition}
--    );
-- LABEL ON INDEX "UQ_Order_#"
--    IS 'Sort by {IndexColumnDescriptions}';


CREATE INDEX "IX_Order_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX1"
   ON "Order" (
   "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX "IX_Order_OrderDate_OrderTime_InternalID"
   IS 'Sort by Order Date and Time';

CREATE INDEX "IX_Order_CustomerInternalID_InternalID"
   FOR SYSTEM NAME "YD1OIX10"
   ON "Order" (
   "CustomerInternalID", "InternalID"
   );
LABEL ON INDEX "IX_Order_CustomerInternalID_InternalID"
   IS 'Sort by Customer Internal ID';

CREATE INDEX "IX_Order_CustomerInternalID_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX2"
   ON "Order" (
   "CustomerInternalID", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX "IX_Order_CustomerInternalID_OrderDate_OrderTime_InternalID"
   IS 'Sort by Customer Internal ID, Order Date/Time';

CREATE INDEX "IX_Order_CustomerInternalID_ PO_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX3"
   ON "Order" (
   "CustomerInternalID",  "PurchaseOrderNumber/ID", "OrderDate", "OrderTime",
   "InternalID"
   );
LABEL ON INDEX "IX_Order_CustomerInternalID_ PO_OrderDate_OrderTime_InternalID"
   IS 'Sort by Customer Internal ID and PO No.';

CREATE INDEX "IX_Order_PO_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX4"
   ON "Order" (
   "PurchaseOrderNumber/ID", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX "IX_Order_PO_OrderDate_OrderTime_InternalID"
   IS 'Sort by PO No.';

CREATE INDEX "IX_Order_SalesPersonName_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX5"
   ON "Order" (
   "SalesPersonName", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX "IX_Order_SalesPersonName_OrderDate_OrderTime_InternalID"
   IS 'Sort by Sales Agent Name';

CREATE INDEX "IX_Order_WarehouseName_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX6"
   ON "Order" (
   "WarehouseName", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX "IX_Order_WarehouseName_OrderDate_OrderTime_InternalID"
   IS 'Sort by Warehouse Name';

CREATE INDEX "IX_Order_Status_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX7"
   ON "Order" (
   "Status", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX "IX_Order_Status_OrderDate_OrderTime_InternalID"
   IS 'Sort by Status';

CREATE INDEX
   "IX_Order_CustomerInternalID_Status_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX8"
   ON "Order" (
   "CustomerInternalID", "Status", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX
   "IX_Order_CustomerInternalID_Status_OrderDate_OrderTime_InternalID"
   IS 'Sort by Customer Internal ID and Status';

CREATE INDEX
   "IX_Order_ShippingAddressInternalID_OrderDate_OrderTime_InternalID"
   FOR SYSTEM NAME "YD1OIX9"
   ON "Order" (
   "ShippingAddressInternalID", "OrderDate", "OrderTime", "InternalID"
   );
LABEL ON INDEX 
   "IX_Order_ShippingAddressInternalID_OrderDate_OrderTime_InternalID"
   IS 'Sort by Shipping Address Internal ID';

-- Aliases
-- CREATE OR REPLACE ALIAS  FOR "Order";

GRANT DELETE, INSERT, SELECT, UPDATE
ON "Order" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "Order" TO QPGMR WITH GRANT OPTION ;

SET SCHEMA DEFAULT;
-- =============================================================================
-- CONFIDENTIAL AND PROPRIETARY INFORMATION
--
-- The receipt or possession of this document does not convey any rights to
-- reproduce, disclose its contents or manufacture, use or sell anything it may
-- describe.  Reproduction, disclosure, or use without the specific written
-- authorization of Surround Technologies, LLC is strictly forbidden.
--
-- Copyright ....: (C) Surround Technologies, LLC 1998 - 2021
--                 9197 Estero River Circle
--                 Estero, FL 33928
--                 973-743-1277
--                 http://www.surroundtech.com/
--                  
-- =============================================================================
-- Source File...: QDBSRC
-- Source Member.: "Product"
-- Created by ...: Lee Paul
-- Created on ...: August 29, 2021
-- Object Type ..: Table Definition
-- Description ..: Product
--
-- BUILD COMMAND.:
-- RUNSQLSTM SRCFILE(EASYBUYSQL/QDBSRC) SRCMBR("Product")
--     COMMIT(*NONE) ERRLVL(20)
--
-- =============================================================================
--                              Amendment Details
--                              -----------------
-- Date       Programmer      Description
-- ---------- --------------- --------------------------------------------------
-- 
--
-- =============================================================================

SET SCHEMA "EasyBuyCyclesSQL";

-- ALTER TABLE "Product"
--    DROP CONSTRAINT "PK_Product",
--    DROP CONSTRAINT "FK_Product_{ReferenceTable}";

DROP INDEX IF EXISTS "UQ_Product_Name";
DROP INDEX IF EXISTS "UQ_Product_Code";
DROP INDEX IF EXISTS "IX_Product_Description_Name";
DROP INDEX IF EXISTS "IX_Product_Category_Name";
DROP INDEX IF EXISTS "IX_Product_Discontinued_Name";
DROP INDEX IF EXISTS "IX_Product_ListPrice_Name";
DROP INDEX IF EXISTS "IX_ShippingAddress_";

-- DROP VIEW IF EXISTS "VW_{ViewName}_{ViewDescriptor}";

-- DROP ALIAS IF EXISTS "{AliasName}";

-- DROP TABLE IF EXISTS "Product";

CREATE OR REPLACE TABLE "Product"
    (
     "InternalID" FOR "YD1PIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "Code" FOR "YD1PCD" CHAR(25) NOT NULL DEFAULT ' ',
     "Name" FOR "YD1PNM" CHAR(50) NOT NULL DEFAULT ' ',
     "Description" FOR "YD1PDS" CHAR(256) NOT NULL DEFAULT ' ',
     "Category" FOR "YD1PCT" CHAR(50) NOT NULL DEFAULT ' ',
     "StandardCost" FOR "YD1PSTCS" NUMERIC(10,2) NOT NULL DEFAULT 0,
     "ListPrice" FOR "YD1PLSPR" NUMERIC(10,2) NOT NULL DEFAULT 0,
     "ReorderLevel" FOR "YD1PROLV" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "TargetLevel" FOR "YD1PTGLV" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "MinimumReorderQuantity" FOR "YD1PMRQT" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "Discontinued" FOR "YD1PDC" CHAR(1) NOT NULL DEFAULT ' ',
     "Memo" FOR "YD1PM1" CHAR(100) NOT NULL DEFAULT ' ',
     "ImagePath" FOR "YD1PIMPT" CHAR(256) NOT NULL DEFAULT ' ',
     "CreateDate" FOR "YD1PCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime" FOR "YD1PCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser" FOR "YD1PCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob" FOR "YD1PCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber" FOR "YD1PCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate" FOR "YD1PLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime" FOR "YD1PLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser" FOR "YD1PLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob" FOR "YD1PLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber" FOR "YD1PLCJN" CHAR(6) NOT NULL DEFAULT ' ',

      -- Primary Key Constraint
      CONSTRAINT "PK_Product"
         PRIMARY KEY ("InternalID")

      -- Referential Constraints
      -- CONSTRAINT FK_Product_{ReferenceTable}
      --    FOREIGN KEY ({ForeignKeys})
      --    REFERENCES {ReferenceTable}({ReferenceColumns})
      --    {OnDeleteAction} {OnUpdateAction}
   )
RCDFMT YD1PPR
ON REPLACE PRESERVE ROWS;

-- Table Description
LABEL ON TABLE "Product"
   IS 'Product';

-- Column Labels
LABEL ON COLUMN "Product" (
     "InternalID" TEXT IS 'Internal ID',
     "Code" TEXT IS 'Code',
     "Name" TEXT IS 'Name',
     "Description" TEXT IS 'Description',
     "Category" TEXT IS 'Category',
     "StandardCost" TEXT IS 'Standard Cost',
     "ListPrice" TEXT IS 'List Price',
     "ReorderLevel" TEXT IS 'Reorder Level',
     "TargetLevel" TEXT IS 'Target Level',
     "MinimumReorderQuantity" TEXT IS 'Minimum Reorder Quantity',
     "Discontinued" TEXT IS 'Discontinued',
     "Memo" TEXT IS 'Memo',
     "ImagePath" TEXT IS 'Image Path',
     "CreateDate" TEXT IS 'Create Date',
     "CreateTime" TEXT IS 'Create Time',
     "CreateUser" TEXT IS 'Create User',
     "CreateJob" TEXT IS 'Create Job',
     "CreateJobNumber" TEXT IS 'Create Job Number',
     "LastChangeDate" TEXT IS 'Last Change Date',
     "LastChangeTime" TEXT IS 'Last Change Time',
     "LastChangeUser" TEXT IS 'Last Change User',
     "LastChangeJob" TEXT IS 'Last Change Job',
     "LastChangeJobNumber" TEXT IS 'Last Change Job Number'
     );

-- Column Labels
LABEL ON COLUMN "Product" (
     "InternalID" IS 'Internal ID',
     "Code" IS 'Code',
     "Name" IS 'Name',
     "Description" IS 'Description',
     "Category" IS 'Category',
     "StandardCost" IS 'Standard Cost',
     "ListPrice" IS 'List Price',
     "ReorderLevel" IS 'Reorder Level',
     "TargetLevel" IS 'Target Level',
     "MinimumReorderQuantity" IS 'Minimum             Reorder Quantity',
     "Discontinued" IS 'Discontinued',
     "Memo" IS 'Memo',
     "ImagePath" IS 'Image Path',
     "CreateDate" IS 'Create Date',
     "CreateTime" IS 'Create Time',
     "CreateUser" IS 'Create User',
     "CreateJob" IS 'Create Job',
     "CreateJobNumber" IS 'Create              Job Number',
     "LastChangeDate" IS 'Last Change         Date',
     "LastChangeTime" IS 'Last Change         Time',
     "LastChangeUser" IS 'Last Change         User',
     "LastChangeJob" IS 'Last Change         Job',
     "LastChangeJobNumber" IS 'Last Change         Job Number'
     );

CREATE UNIQUE INDEX "UQ_Product_Name"
   FOR SYSTEM NAME YD1PIXU1 ON "Product" (
   "Name"
   );
LABEL ON INDEX "UQ_Product_Name"
   IS 'Sort by Product Name';
CREATE UNIQUE INDEX "UQ_Product_Code"
   FOR SYSTEM NAME YD1PIXU2 ON "Product" (
   "Code"
   );
LABEL ON INDEX "UQ_Product_Code"
   IS 'Sort by Product Code';

CREATE INDEX "IX_Product_Description_Name"
   FOR SYSTEM NAME "YD1PIX3"
   ON "Product" (
   "Description", "Name"
   );
LABEL ON INDEX "IX_Product_Description_Name"
   IS 'Sort by Description';

CREATE INDEX "IX_Product_Category_Name"
   FOR SYSTEM NAME "YD1PIX4"
   ON "Product" (
   "Category", "Name"
   );
LABEL ON INDEX "IX_Product_Category_Name"
   IS 'Sort by Category';

CREATE INDEX "IX_Product_Discontinued_Name"
   FOR SYSTEM NAME "YD1PIX5"
   ON "Product" (
   "Discontinued", "Name"
   );
LABEL ON INDEX "IX_Product_Discontinued_Name"
   IS 'Sort by Discontinued';

CREATE INDEX "IX_Product_ListPrice_Name"
   FOR SYSTEM NAME "YD1PIX6"
   ON "Product" (
   "ListPrice", "Name"
   );
LABEL ON INDEX "IX_Product_ListPrice_Name"
   IS 'Sort by List Price';

-- Aliases
-- CREATE OR REPLACE ALIAS  FOR "Product";

GRANT DELETE, INSERT, SELECT, UPDATE
ON "Product" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "Product" TO QPGMR WITH GRANT OPTION ;

SET SCHEMA DEFAULT;
-- =============================================================================
-- CONFIDENTIAL AND PROPRIETARY INFORMATION
--
-- The receipt or possession of this document does not convey any rights to
-- reproduce, disclose its contents or manufacture, use or sell anything it may
-- describe.  Reproduction, disclosure, or use without the specific written
-- authorization of Surround Technologies, LLC is strictly forbidden.
--
-- Copyright ....: (C) Surround Technologies, LLC 1998 - 2021
--                 9197 Estero River Circle
--                 Estero, FL 33928
--                 973-743-1277
--                 http://www.surroundtech.com/
--                  
-- =============================================================================
-- Source File...: QDBSRC
-- Source Member.: "ShpAddr"
-- Created by ...: Lee Paul
-- Created on ...: August 29, 2021
-- Object Type ..: Table Definition
-- Description ..: Shipping Address
--
-- BUILD COMMAND.:
-- RUNSQLSTM SRCFILE(EASYBUYSQL/QDBSRC) SRCMBR("ShpAddr")
--     COMMIT(*NONE) ERRLVL(20)
--
-- =============================================================================
--                              Amendment Details
--                              -----------------
-- Date       Programmer      Description
-- ---------- --------------- --------------------------------------------------
-- 
--
-- =============================================================================

SET SCHEMA "EasyBuyCyclesSQL";

-- ALTER TABLE "ShippingAddress"
--    DROP CONSTRAINT "PK_ShippingAddress",
--    DROP CONSTRAINT "FK_ShippingAddress_{ReferenceTable}";

DROP INDEX IF EXISTS "UQ_ShippingAddress_Name";
DROP INDEX IF EXISTS "IX_ShippingAddress_PostalCode_Name";
DROP INDEX IF EXISTS "IX_ShippingAddress_ContactLastName_ContactFirstName_Name";
DROP INDEX IF EXISTS "IX_ShippingAddress_Telephone_Name";
DROP INDEX IF EXISTS "IX_ShippingAddress_Email_Name";
DROP INDEX IF EXISTS "IX_ShippingAddress_CustomerInternalID_Name";

-- DROP VIEW IF EXISTS "VW_{ViewName}_{ViewDescriptor}";

-- DROP ALIAS IF EXISTS "{AliasName}";

-- DROP TABLE IF EXISTS "ShippingAddress";

CREATE OR REPLACE TABLE "ShippingAddress"
    FOR SYSTEM NAME "ShpAddr"  (
     "InternalID" FOR "YD1SIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CustomerInternalID" FOR "YD1S1CID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "Name" FOR "YD1SNM" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactLastName" FOR "YD1SCNLN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactFirstName" FOR "YD1SCNFN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactMiddleName" FOR "YD1SCNMN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactNickName" FOR "YD1SCNNN" CHAR(50) NOT NULL DEFAULT ' ',
     "Address1" FOR "YD1SSHA1" CHAR(30) NOT NULL DEFAULT ' ',
     "Address2" FOR "YD1SSHA2" CHAR(30) NOT NULL DEFAULT ' ',
     "Address3" FOR "YD1SSHA3" CHAR(30) NOT NULL DEFAULT ' ',
     "PostalCode" FOR "YD1SSHPC" CHAR(10) NOT NULL DEFAULT ' ',
     "Country" FOR "YD1SSHCY" CHAR(50) NOT NULL DEFAULT ' ',
     "Telephone" FOR "YD1STL" CHAR(20) NOT NULL DEFAULT ' ',
     "Email" FOR "YD1SEM" CHAR(50) NOT NULL DEFAULT ' ',
     "Memo" FOR "YD1SM1" CHAR(100) NOT NULL DEFAULT ' ',
     "PurchasePoints" FOR "YD1SPRPT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateDate" FOR "YD1SCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime" FOR "YD1SCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser" FOR "YD1SCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob" FOR "YD1SCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber" FOR "YD1SCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate" FOR "YD1SLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime" FOR "YD1SLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser" FOR "YD1SLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob" FOR "YD1SLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber" FOR "YD1SLCJN" CHAR(6) NOT NULL DEFAULT ' ',

      -- Primary Key Constraint
      CONSTRAINT "PK_ShippingAddress"
         PRIMARY KEY ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_ShippingAddress_Customer"
         FOREIGN KEY ("CustomerInternalID")
         REFERENCES "Customer"("InternalID")
         ON DELETE RESTRICT ON UPDATE RESTRICT
   )
RCDFMT YD1SPR
ON REPLACE PRESERVE ROWS;

-- Table Description
LABEL ON TABLE "ShippingAddress"
   IS 'Shipping Address';

-- Column Labels
LABEL ON COLUMN "ShippingAddress" (
     "InternalID" TEXT IS 'Internal ID',
     "CustomerInternalID" TEXT IS 'Customer Internal ID',
     "Name" TEXT IS 'Name',
     "ContactLastName" TEXT IS 'Contact Last Name',
     "ContactFirstName" TEXT IS 'Contact First Name',
     "ContactMiddleName" TEXT IS 'Contact Middle Name',
     "ContactNickName" TEXT IS 'Contact Nick Name',
     "Address1" TEXT IS 'Address 1',
     "Address2" TEXT IS 'Address 2',
     "Address3" TEXT IS 'Address 3',
     "PostalCode" TEXT IS 'Postal Code',
     "Country" TEXT IS 'Country',
     "Telephone" TEXT IS 'Telephone',
     "Email" TEXT IS 'Email',
     "Memo" TEXT IS 'Memo',
     "PurchasePoints" TEXT IS 'Purchase Points',
     "CreateDate" TEXT IS 'Create Date',
     "CreateTime" TEXT IS 'Create Time',
     "CreateUser" TEXT IS 'Create User',
     "CreateJob" TEXT IS 'Create Job',
     "CreateJobNumber" TEXT IS 'Create Job Number',
     "LastChangeDate" TEXT IS 'Last Change Date',
     "LastChangeTime" TEXT IS 'Last Change Time',
     "LastChangeUser" TEXT IS 'Last Change User',
     "LastChangeJob" TEXT IS 'Last Change Job',
     "LastChangeJobNumber" TEXT IS 'Last Change Job Number'
     );

-- Column Labels
LABEL ON COLUMN "ShippingAddress" (
     "InternalID" IS 'Internal ID',
     "CustomerInternalID" IS 'Customer            Internal ID',
     "Name" IS 'Name',
     "ContactLastName" IS 'Contact             Last Name',
     "ContactFirstName" IS 'Contact             First Name',
     "ContactMiddleName" IS 'Contact             Middle Name',
     "ContactNickName" IS 'Contact             Nick Name',
     "Address1" IS 'Address 1',
     "Address2" IS 'Address 2',
     "Address3" IS 'Address 3',
     "PostalCode" IS 'Postal Code',
     "Country" IS 'Country',
     "Telephone" IS 'Telephone',
     "Email" IS 'Email',
     "Memo" IS 'Memo',
     "PurchasePoints" IS 'Purchase Points',
     "CreateDate" IS 'Create Date',
     "CreateTime" IS 'Create Time',
     "CreateUser" IS 'Create User',
     "CreateJob" IS 'Create Job',
     "CreateJobNumber" IS 'Create              Job Number',
     "LastChangeDate" IS 'Last Change         Date',
     "LastChangeTime" IS 'Last Change         Time',
     "LastChangeUser" IS 'Last Change         User',
     "LastChangeJob" IS 'Last Change         Job',
     "LastChangeJobNumber" IS 'Last Change         Job Number'
     );

CREATE UNIQUE INDEX "UQ_ShippingAddress_Name"
   FOR SYSTEM NAME YD1SIXU1 ON "ShippingAddress" (
   "Name"
   );
LABEL ON INDEX "UQ_ShippingAddress_Name"
   IS 'Sort by Shipping Address Name';

CREATE INDEX "IX_ShippingAddress_PostalCode_Name"
   FOR SYSTEM NAME "YD1SIX2"
   ON "ShippingAddress" (
   "PostalCode", "Name"
   );
LABEL ON INDEX "IX_ShippingAddress_PostalCode_Name"
   IS 'Sort by Postal Code';

CREATE INDEX "IX_ShippingAddress_ContactLastName_ContactFirstName_Name"
   FOR SYSTEM NAME "YD1SIX3"
   ON "ShippingAddress" (
   "ContactLastName", "ContactFirstName", "Name"
   );
LABEL ON INDEX "IX_ShippingAddress_ContactLastName_ContactFirstName_Name"
   IS 'Sort by Last and First Name';

CREATE INDEX "IX_ShippingAddress_Telephone_Name"
   FOR SYSTEM NAME "YD1SIX4"
   ON "ShippingAddress" (
   "Telephone", "Name"
   );
LABEL ON INDEX "IX_ShippingAddress_Telephone_Name"
   IS 'Sort by Telephone';

CREATE INDEX "IX_ShippingAddress_Email_Name"
   FOR SYSTEM NAME "YD1SIX5"
   ON "ShippingAddress" (
   "Email", "Name"
   );
LABEL ON INDEX "IX_ShippingAddress_Email_Name"
   IS 'Sort by Email';

CREATE INDEX "IX_ShippingAddress_CustomerInternalID_Name"
   FOR SYSTEM NAME "YD1SIX6"
   ON "ShippingAddress" (
   "CustomerInternalID", "Name"
   );
LABEL ON INDEX "IX_ShippingAddress_CustomerInternalID_Name"
   IS 'Sort by Customer ID';

-- Aliases
-- CREATE OR REPLACE ALIAS ShpAddr FOR "ShippingAddress";

GRANT DELETE, INSERT, SELECT, UPDATE
ON "ShippingAddress" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "ShippingAddress" TO QPGMR WITH GRANT OPTION ;

SET SCHEMA DEFAULT;

