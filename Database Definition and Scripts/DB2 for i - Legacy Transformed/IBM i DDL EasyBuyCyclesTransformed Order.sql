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
Source Member.: "Order"
Created by ...: Lee Paul
Created on ...: August 29, 2021
Object Type ..: Table Definition
Description ..: Order

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM2/QDBSRC) SRCMBR("Order")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCyclesTransformed";
-- SET SCHEMA "EasyBuyCyclesTransformedDev";

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
-- Warnng: Dropping a table will lose the data within it.

CREATE OR REPLACE TABLE "Order"
    (
     "InternalID"           FOR "YD1OIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CustomerInternalID"   FOR "YD1O1CID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "OrderDate"            FOR "YD1ODT" DATE NOT NULL DEFAULT CURRENT_DATE,
     "OrderTime"            FOR "YD1OTM" TIME NOT NULL DEFAULT CURRENT_TIME,
     "PurchaseOrderNumber/ID" FOR "YD1OPONO" CHAR(50) NOT NULL DEFAULT ' ',
     "WarehouseInternalID"  FOR "YD1O1WID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "WarehouseName"        FOR "YD1O1WNM" CHAR(50) NOT NULL DEFAULT ' ',
     "DeliveryMemo"         FOR "YD1ODLM1" CHAR(100) NOT NULL DEFAULT ' ',
     "ShippingAddressInternalID" FOR "YD1O1SID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "OrderMemo"            FOR "YD1OM1" CHAR(100) NOT NULL DEFAULT ' ',
     "Status"               FOR "YD1OST" CHAR(10) NOT NULL DEFAULT ' ',
     "SalesPersonInternalID" FOR "YD1O1AID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "SalesPersonName"      FOR "YD1O1ANM" CHAR(50) NOT NULL DEFAULT ' ',
     "PurchasePoints"       FOR "YD1CPRPT" NUMERIC(8,0) NOT NULL DEFAULT 0,
    
     -- Audit Stamps
     "CreateDate"           FOR "YD1OCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime"           FOR "YD1OCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser"           FOR "YD1OCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob"            FOR "YD1OCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber"      FOR "YD1OCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate"       FOR "YD1OLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime"       FOR "YD1OLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser"       FOR "YD1OLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob"        FOR "YD1OLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber"  FOR "YD1OLCJN" CHAR(6) NOT NULL DEFAULT ' ',

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

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "Order" (
     "InternalID"           TEXT IS 'Internal ID',
     "CustomerInternalID"   TEXT IS 'Customer Internal ID',
     "OrderDate"            TEXT IS 'Order Date',
     "OrderTime"            TEXT IS 'Order Time',
     "PurchaseOrderNumber/ID" TEXT IS 'Purchase Order Number/ID',
     "WarehouseInternalID"  TEXT IS 'Warehouse Internal ID',
     "WarehouseName"        TEXT IS 'Warehouse Name',
     "DeliveryMemo"         TEXT IS 'Delivery Memo',
     "ShippingAddressInternalID" TEXT IS 'Shipping Address Internal ID',
     "OrderMemo"            TEXT IS 'Order Memo',
     "Status"               TEXT IS 'Status',
     "SalesPersonInternalID" TEXT IS 'Sales Person Internal ID',
     "SalesPersonName"      TEXT IS 'Sales Person Name',
     "PurchasePoints"       TEXT IS 'Purchase Points',
     "CreateDate"           TEXT IS 'Create Date',
     "CreateTime"           TEXT IS 'Create Time',
     "CreateUser"           TEXT IS 'Create User',
     "CreateJob"            TEXT IS 'Create Job',
     "CreateJobNumber"      TEXT IS 'Create Job Number',
     "LastChangeDate"       TEXT IS 'Last Change Date',
     "LastChangeTime"       TEXT IS 'Last Change Time',
     "LastChangeUser"       TEXT IS 'Last Change User',
     "LastChangeJob"        TEXT IS 'Last Change Job',
     "LastChangeJobNumber"  TEXT IS 'Last Change Job Number'
     );

-- Column Labels - Column Headings (3 x 20 Chars)
LABEL ON COLUMN "Order" (
--                             |1-------------------2-------------------3-------------------|
     "InternalID"           IS 'Internal ID',
     "CustomerInternalID"   IS 'Customer            Internal ID',
     "OrderDate"            IS 'Order Date',
     "OrderTime"            IS 'Order Time',
     "PurchaseOrderNumber/ID" IS 'Purchase Order      Number/Id',
     "WarehouseInternalID"  IS 'Warehouse           Internal ID',
     "WarehouseName"        IS 'Warehouse           Name',
     "DeliveryMemo"         IS 'Delivery Memo',
     "ShippingAddressInternalID" IS 'Shipping Address    Internal ID',
     "OrderMemo"            IS 'Order Memo',
     "Status"               IS 'Status',
     "SalesPersonInternalID" IS 'Sales Person        Internal ID',
     "SalesPersonName"      IS 'Sales Person        Name',
     "PurchasePoints"       IS 'Purchase Points',
     "CreateDate"           IS 'Create Date',
     "CreateTime"           IS 'Create Time',
     "CreateUser"           IS 'Create User',
     "CreateJob"            IS 'Create Job',
     "CreateJobNumber"      IS 'Create              Job Number',
     "LastChangeDate"       IS 'Last Change         Date',
     "LastChangeTime"       IS 'Last Change         Time',
     "LastChangeUser"       IS 'Last Change         User',
     "LastChangeJob"        IS 'Last Change         Job',
     "LastChangeJobNumber"  IS 'Last Change         Job Number'
     );

-- Grant Table Access
GRANT DELETE, INSERT, SELECT, UPDATE
ON "Order" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "Order" TO QPGMR WITH GRANT OPTION ;

-- Unique Indexes
-- CREATE UNIQUE |WHERE NOT NULL| INDEX "UQ_Order_#"
--    FOR SYSTEM NAME "{IndexSystemName}"
--    ON "Order" (
--    {IndexColumns}
--    WHERE {FilterCondition}
--    );
-- LABEL ON INDEX "UQ_Order_#"
--    IS 'Sort by {IndexColumnDescriptions}';


-- Indexes
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

-- Views
-- CREATE OR REPLACE VIEW "VW_AccountTable_<Descriptors>"
--     FOR SYSTEM NAME "FACCTV01" AS
--          < View Definition >
-- ;
-- LABEL ON TABLE "VVW_AccountTable_<Descriptors>"
--    IS 'View Description';

-- LABEL ON COLUMN "VW_AccountTable_<Descriptors>" (
--      <ViewColumnName> TEXT IS 'View Column Description'
--      );

-- LABEL ON COLUMN "VW_AccountTable_<Descriptors>" (
--      <ViewColumnName> IS 'View Column Description'
--      );

-- GRANT SELECT
-- ON "VW_AccountTable_<Descriptors>" TO PUBLIC WITH GRANT OPTION ;

-- GRANT ALTER, REFERENCES, SELECT
-- ON "VW_AccountTable_<Descriptors>" TO QPGMR WITH GRANT OPTION ;
   
-- Aliases
-- CREATE OR REPLACE ALIAS {AliasName} FOR "AccountTable";

-- Triggers

SET SCHEMA DEFAULT;