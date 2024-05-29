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
Source Member.: "OrdItm"
Created by ...: Lee Paul
Created on ...: August 29, 2021
Object Type ..: Table Definition
Description ..: Order Item

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM2/QDBSRC) SRCMBR("OrdItm")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCyclesTransformed";
-- SET SCHEMA "EasyBuyCyclesTransformedDev";

-- ALTER TABLE "OrderItem"
--    DROP CONSTRAINT "PK_OrderItem";
--    DROP CONSTRAINT "FK_OrderItem_{ReferenceTable}";

-- DROP INDEX IF EXISTS "UQ_OrderItem_{IndexDescriptor}";
DROP INDEX IF EXISTS
   "IX_OrderItem_OrderInternalID_ProductInternalID_InternalID";
DROP INDEX IF EXISTS
   "IX_OrderItem_ProductInternalID_OrderInternalID_InternalID";
DROP INDEX IF EXISTS "IX_OrderItem_OrderInternalID_InternalID";

-- DROP VIEW IF EXISTS "VW_{ViewName}_{ViewDescriptor}";

-- DROP ALIAS IF EXISTS "{AliasName}";

-- DROP TABLE IF EXISTS "OrderItem";
-- Warnng: Dropping a table will lose the data within it.

CREATE OR REPLACE TABLE "OrderItem"
    FOR SYSTEM NAME "OrdItm"  (
     "InternalID"           FOR "YD1IIID" NUMERIC(12,0) NOT NULL DEFAULT 0,
     "OrderInternalID"      FOR "YD1I1OID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "ProductInternalID"    FOR "YD1I1PID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "Quantity"             FOR "YD1IQT" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "UnitPrice"            FOR "YD1IPRUN" NUMERIC(8,2) NOT NULL DEFAULT 0,
     "Discount(Percent)"    FOR "YD1IDSPC" NUMERIC(4,1) NOT NULL DEFAULT 0,
     "Memo"                 FOR "YD1IM1" CHAR(100) NOT NULL DEFAULT ' ',
    
     -- Audit Stamps
     "CreateDate"           FOR "YD1ICRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime"           FOR "YD1ICRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser"           FOR "YD1ICRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob"            FOR "YD1ICRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber"      FOR "YD1ICRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate"       FOR "YD1ILCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime"       FOR "YD1ILCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser"       FOR "YD1ILCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob"        FOR "YD1ILCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber"  FOR "YD1ILCJN" CHAR(6) NOT NULL DEFAULT ' ',

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

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "OrderItem" (
     "InternalID"           TEXT IS 'Internal ID',
     "OrderInternalID"      TEXT IS 'Order Internal ID',
     "ProductInternalID"    TEXT IS 'Product Internal ID',
     "Quantity"             TEXT IS 'Quantity',
     "UnitPrice"            TEXT IS 'Unit Price',
     "Discount(Percent)"    TEXT IS 'Discount (Percent)',
     "Memo"                 TEXT IS 'Memo',
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
LABEL ON COLUMN "OrderItem" (
--                             |1-------------------2-------------------3-------------------|
     "InternalID"           IS 'Internal ID',
     "OrderInternalID"      IS 'Order               Internal ID',
     "ProductInternalID"    IS 'Product             Internal ID',
     "Quantity"             IS 'Quantity',
     "UnitPrice"            IS 'Unit Price',
     "Discount(Percent)"    IS 'Discount (Percent)',
     "Memo"                 IS 'Memo',
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
ON "OrderItem" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "OrderItem" TO QPGMR WITH GRANT OPTION ;

-- Unique Indexes
-- CREATE UNIQUE |WHERE NOT NULL| INDEX "UQ_OrderItem_#"
--    FOR SYSTEM NAME "{IndexSystemName}"
--    ON "OrderItem" (
--    {IndexColumns}
--    WHERE {FilterCondition}
--    );
-- LABEL ON INDEX "UQ_OrderItem_#"
--    IS 'Sort by {IndexColumnDescriptions}';

-- Indexes
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