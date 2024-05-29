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
Source Member.: "Customer"
Created by ...: Lee Paul
Created on ...: August 29, 2021
Object Type ..: Table Definition
Description ..: Customer

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM2/QDBSRC) SRCMBR("Customer")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

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
-- Warnng: Dropping a table will lose the data within it.

CREATE OR REPLACE TABLE "Customer"
    (
     "InternalID"           FOR "YD1CIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "ParentInternalID"     FOR "YD1CPTID" NUMERIC(8,0),
     "ParentRelationship"   FOR "YD1CPTRL" CHAR(20),
     "Name"                 FOR "YD1CNM" CHAR(50) NOT NULL DEFAULT ' ',
     "LegalName"            FOR "YD1CNMLG" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactLastName"      FOR "YD1CCNLN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactFirstName"     FOR "YD1CCNFN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactMiddleName"    FOR "YD1CCNMN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactNickName"      FOR "YD1CCNNN" CHAR(50) NOT NULL DEFAULT ' ',
     "BillingAddress1"      FOR "YD1CBLA1" CHAR(30) NOT NULL DEFAULT ' ',
     "BillingAddress2"      FOR "YD1CBLA2" CHAR(30) NOT NULL DEFAULT ' ',
     "BillingAddress3"      FOR "YD1CBLA3" CHAR(30) NOT NULL DEFAULT ' ',
     "BillingPostalCode"    FOR "YD1CBLPC" CHAR(10) NOT NULL DEFAULT ' ',
     "BillingCountry"       FOR "YD1CBLCY" CHAR(50) NOT NULL DEFAULT ' ',
     "Telephone"            FOR "YD1CTL" CHAR(20) NOT NULL DEFAULT ' ',
     "Email"                FOR "YD1CEM" CHAR(50) NOT NULL DEFAULT ' ',
     "Memo"                 FOR "YD1CM1" CHAR(100) NOT NULL DEFAULT ' ',
     "PurchasePoints"       FOR "YD1CPRPT" NUMERIC(8,0) NOT NULL DEFAULT 0,
    
     -- Audit Stamps
     "CreateDate"           FOR "YD1CCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime"           FOR "YD1CCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser"           FOR "YD1CCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob"            FOR "YD1CCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber"      FOR "YD1CCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate"       FOR "YD1CLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime"       FOR "YD1CLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser"       FOR "YD1CLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob"        FOR "YD1CLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber"  FOR "YD1CLCJN" CHAR(6) NOT NULL DEFAULT ' ',

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

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "Customer" (
     "InternalID"           TEXT IS 'Internal ID',
     "ParentInternalID"     TEXT IS 'Parent Internal ID',
     "ParentRelationship"   TEXT IS 'Parent Relationship',
     "Name"                 TEXT IS 'Name',
     "LegalName"            TEXT IS 'Legal Name',
     "ContactLastName"      TEXT IS 'Contact Last Name',
     "ContactFirstName"     TEXT IS 'Contact First Name',
     "ContactMiddleName"    TEXT IS 'Contact Middle Name',
     "ContactNickName"      TEXT IS 'Contact Nick Name',
     "BillingAddress1"      TEXT IS 'Billing Address 1',
     "BillingAddress2"      TEXT IS 'Billing Address 2',
     "BillingAddress3"      TEXT IS 'Billing Address 3',
     "BillingPostalCode"    TEXT IS 'Billing Postal Code',
     "BillingCountry"       TEXT IS 'Billing Country',
     "Telephone"            TEXT IS 'Telephone',
     "Email"                TEXT IS 'Email',
     "Memo"                 TEXT IS 'Memo',
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
LABEL ON COLUMN "Customer" (
--                             |1-------------------2-------------------3-------------------|
     "InternalID"           IS 'Internal ID',
     "ParentInternalID"     IS 'Parent              Internal ID',
     "ParentRelationship"   IS 'Parent              Relationship',
     "Name"                 IS 'Name',
     "LegalName"            IS 'Legal Name',
     "ContactLastName"      IS 'Contact             Last Name',
     "ContactFirstName"     IS 'Contact             First Name',
     "ContactMiddleName"    IS 'Contact             Middle Name',
     "ContactNickName"      IS 'Contact             Nick Name',
     "BillingAddress1"      IS 'Billing             Address 1',
     "BillingAddress2"      IS 'Billing             Address 2',
     "BillingAddress3"      IS 'Billing             Address 3',
     "BillingPostalCode"    IS 'Billing             Postal Code',
     "BillingCountry"       IS 'Billing             Country',
     "Telephone"            IS 'Telephone',
     "Email"                IS 'Email',
     "Memo"                 IS 'Memo',
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
ON "Customer" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "Customer" TO QPGMR WITH GRANT OPTION ;

-- Unique Indexes
CREATE UNIQUE INDEX "UQ_Customer_Name"
   FOR SYSTEM NAME YD1CIXU1 ON "Customer" (
   "Name"
   );
LABEL ON INDEX "UQ_Customer_Name"
   IS 'Sort by Customer Name';

-- Indexes
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