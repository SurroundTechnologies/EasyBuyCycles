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
Source Member.: "Product"
Created by ...: Lee Paul
Created on ...: August 29, 2021
Object Type ..: Table Definition
Description ..: Product

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM2/QDBSRC) SRCMBR("Product")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCyclesTransformed";
-- SET SCHEMA "EasyBuyCyclesTransformedDev";

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
-- Warnng: Dropping a table will lose the data within it.

CREATE OR REPLACE TABLE "Product"
    (
     "InternalID"           FOR "YD1PIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "Code"                 FOR "YD1PCD" CHAR(25) NOT NULL DEFAULT ' ',
     "Name"                 FOR "YD1PNM" CHAR(50) NOT NULL DEFAULT ' ',
     "Description"          FOR "YD1PDS" CHAR(256) NOT NULL DEFAULT ' ',
     "Category"             FOR "YD1PCT" CHAR(50) NOT NULL DEFAULT ' ',
     "StandardCost"         FOR "YD1PSTCS" NUMERIC(10,2) NOT NULL DEFAULT 0,
     "ListPrice"            FOR "YD1PLSPR" NUMERIC(10,2) NOT NULL DEFAULT 0,
     "ReorderLevel"         FOR "YD1PROLV" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "TargetLevel"          FOR "YD1PTGLV" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "MinimumReorderQuantity" FOR "YD1PMRQT" NUMERIC(4,0) NOT NULL DEFAULT 0,
     "Discontinued"         FOR "YD1PDC" CHAR(1) NOT NULL DEFAULT ' ',
     "Memo"                 FOR "YD1PM1" CHAR(100) NOT NULL DEFAULT ' ',
     "ImagePath"            FOR "YD1PIMPT" CHAR(256) NOT NULL DEFAULT ' ',
    
     -- Audit Stamps
     "CreateDate"           FOR "YD1PCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime"           FOR "YD1PCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser"           FOR "YD1PCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob"            FOR "YD1PCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber"      FOR "YD1PCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate"       FOR "YD1PLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime"       FOR "YD1PLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser"       FOR "YD1PLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob"        FOR "YD1PLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber"  FOR "YD1PLCJN" CHAR(6) NOT NULL DEFAULT ' ',

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

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "Product" (
     "InternalID"           TEXT IS 'Internal ID',
     "Code"                 TEXT IS 'Code',
     "Name"                 TEXT IS 'Name',
     "Description"          TEXT IS 'Description',
     "Category"             TEXT IS 'Category',
     "StandardCost"         TEXT IS 'Standard Cost',
     "ListPrice"            TEXT IS 'List Price',
     "ReorderLevel"         TEXT IS 'Reorder Level',
     "TargetLevel"          TEXT IS 'Target Level',
     "MinimumReorderQuantity" TEXT IS 'Minimum Reorder Quantity',
     "Discontinued"         TEXT IS 'Discontinued',
     "Memo"                 TEXT IS 'Memo',
     "ImagePath"            TEXT IS 'Image Path',
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
LABEL ON COLUMN "Product" (
--                             |1-------------------2-------------------3-------------------|
     "InternalID"           IS 'Internal ID',
     "Code"                 IS 'Code',
     "Name"                 IS 'Name',
     "Description"          IS 'Description',
     "Category"             IS 'Category',
     "StandardCost"         IS 'Standard Cost',
     "ListPrice"            IS 'List Price',
     "ReorderLevel"         IS 'Reorder Level',
     "TargetLevel"          IS 'Target Level',
     "MinimumReorderQuantity" IS 'Minimum             Reorder Quantity',
     "Discontinued"         IS 'Discontinued',
     "Memo"                 IS 'Memo',
     "ImagePath"            IS 'Image Path',
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
ON "Product" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "Product" TO QPGMR WITH GRANT OPTION ;

-- Unique Indexes
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

-- Indexes
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