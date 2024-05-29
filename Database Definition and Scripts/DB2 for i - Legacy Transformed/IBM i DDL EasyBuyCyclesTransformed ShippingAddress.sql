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
Source Member.: "ShpAddr"
Created by ...: Lee Paul
Created on ...: August 29, 2021
Object Type ..: Table Definition
Description ..: Shipping Address

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM2/QDBSRC) SRCMBR("ShpAddr")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

SET SCHEMA "EasyBuyCyclesTransformed";
-- SET SCHEMA "EasyBuyCyclesTransformedDev";

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
-- Warnng: Dropping a table will lose the data within it.

CREATE OR REPLACE TABLE "ShippingAddress"
    FOR SYSTEM NAME "ShpAddr"  (
     "InternalID"           FOR "YD1SIID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CustomerInternalID"   FOR "YD1S1CID" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "Name"                 FOR "YD1SNM" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactLastName"      FOR "YD1SCNLN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactFirstName"     FOR "YD1SCNFN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactMiddleName"    FOR "YD1SCNMN" CHAR(50) NOT NULL DEFAULT ' ',
     "ContactNickName"      FOR "YD1SCNNN" CHAR(50) NOT NULL DEFAULT ' ',
     "Address1"             FOR "YD1SSHA1" CHAR(30) NOT NULL DEFAULT ' ',
     "Address2"             FOR "YD1SSHA2" CHAR(30) NOT NULL DEFAULT ' ',
     "Address3"             FOR "YD1SSHA3" CHAR(30) NOT NULL DEFAULT ' ',
     "PostalCode"           FOR "YD1SSHPC" CHAR(10) NOT NULL DEFAULT ' ',
     "Country"              FOR "YD1SSHCY" CHAR(50) NOT NULL DEFAULT ' ',
     "Telephone"            FOR "YD1STL" CHAR(20) NOT NULL DEFAULT ' ',
     "Email"                FOR "YD1SEM" CHAR(50) NOT NULL DEFAULT ' ',
     "Memo"                 FOR "YD1SM1" CHAR(100) NOT NULL DEFAULT ' ',
     "PurchasePoints"       FOR "YD1SPRPT" NUMERIC(8,0) NOT NULL DEFAULT 0,
    
     -- Audit Stamps
     "CreateDate"           FOR "YD1SCRDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "CreateTime"           FOR "YD1SCRTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "CreateUser"           FOR "YD1SCRUS" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJob"            FOR "YD1SCRJB" CHAR(10) NOT NULL DEFAULT ' ',
     "CreateJobNumber"      FOR "YD1SCRJN" CHAR(6) NOT NULL DEFAULT ' ',
     "LastChangeDate"       FOR "YD1SLCDT" NUMERIC(8,0) NOT NULL DEFAULT 0,
     "LastChangeTime"       FOR "YD1SLCTM" NUMERIC(6,0) NOT NULL DEFAULT 0,
     "LastChangeUser"       FOR "YD1SLCUS" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJob"        FOR "YD1SLCJB" CHAR(10) NOT NULL DEFAULT ' ',
     "LastChangeJobNumber"  FOR "YD1SLCJN" CHAR(6) NOT NULL DEFAULT ' ',

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

-- Column Labels - Description (50 Chars)
LABEL ON COLUMN "ShippingAddress" (
     "InternalID"           TEXT IS 'Internal ID',
     "CustomerInternalID"   TEXT IS 'Customer Internal ID',
     "Name"                 TEXT IS 'Name',
     "ContactLastName"      TEXT IS 'Contact Last Name',
     "ContactFirstName"     TEXT IS 'Contact First Name',
     "ContactMiddleName"    TEXT IS 'Contact Middle Name',
     "ContactNickName"      TEXT IS 'Contact Nick Name',
     "Address1"             TEXT IS 'Address 1',
     "Address2"             TEXT IS 'Address 2',
     "Address3"             TEXT IS 'Address 3',
     "PostalCode"           TEXT IS 'Postal Code',
     "Country"              TEXT IS 'Country',
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
LABEL ON COLUMN "ShippingAddress" (
--                             |1-------------------2-------------------3-------------------|
     "InternalID"           IS 'Internal ID',
     "CustomerInternalID"   IS 'Customer            Internal ID',
     "Name"                 IS 'Name',
     "ContactLastName"      IS 'Contact             Last Name',
     "ContactFirstName"     IS 'Contact             First Name',
     "ContactMiddleName"    IS 'Contact             Middle Name',
     "ContactNickName"      IS 'Contact             Nick Name',
     "Address1"             IS 'Address 1',
     "Address2"             IS 'Address 2',
     "Address3"             IS 'Address 3',
     "PostalCode"           IS 'Postal Code',
     "Country"              IS 'Country',
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
ON "ShippingAddress" TO PUBLIC WITH GRANT OPTION ;

GRANT ALTER, DELETE, INDEX, INSERT, REFERENCES, SELECT, UPDATE
ON "ShippingAddress" TO QPGMR WITH GRANT OPTION ;

-- Unique Indexes
CREATE UNIQUE INDEX "UQ_ShippingAddress_Name"
   FOR SYSTEM NAME YD1SIXU1 ON "ShippingAddress" (
   "Name"
   );
LABEL ON INDEX "UQ_ShippingAddress_Name"
   IS 'Sort by Shipping Address Name';

-- Indexes
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