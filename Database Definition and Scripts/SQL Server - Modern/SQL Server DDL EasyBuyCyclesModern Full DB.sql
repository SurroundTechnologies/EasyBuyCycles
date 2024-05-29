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
-- Source File...: EasyBuyCycles_SQL_FullDBDDL.sql
-- Created by ...: Lee Paul
-- Created on ...: May 18, 2023
-- Description ..: Create the Full DB for EasyBuy Cycles
--
-- =============================================================================
--                              Amendment Details
--                              -----------------
-- Date       Programmer      Description
-- ---------- --------------- --------------------------------------------------
-- 
--
-- =============================================================================

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'EasyBuyCycles')
  BEGIN
    CREATE DATABASE [EasyBuyCycles]
  END
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'EasyBuyCyclesDev')
  BEGIN
    CREATE DATABASE [EasyBuyCyclesDev]
  END
GO
USE "EasyBuyCycles";
-- USE "EasyBuyCyclesDev";

-- =============================================================================
-- Customer Table
-- =============================================================================
CREATE TABLE "dbo"."Customer"
    (
     "InternalID"            INT IDENTITY(5000,1) NOT NULL,
     "ParentInternalID"      INT,
     "ParentRelationship"    NVARCHAR(20),
     "Name"                  NVARCHAR(50) NOT NULL,
     "LegalName"             NVARCHAR(50),
     "ContactLastName"       NVARCHAR(50) NOT NULL,
     "ContactFirstName"      NVARCHAR(50) NOT NULL,
     "ContactMiddleName"     NVARCHAR(50),
     "ContactNickName"       NVARCHAR(50),
     "BillingAddress1"       NVARCHAR(30) NOT NULL,
     "BillingAddress2"       NVARCHAR(30) NOT NULL,
     "BillingAddress3"       NVARCHAR(30) NOT NULL,
     "BillingPostalCode"     NVARCHAR(10) NOT NULL,
     "BillingCountry"        NVARCHAR(50) NOT NULL,
     "Telephone"             NVARCHAR(20),
     "Email"                 NVARCHAR(50),
     "Memo"                  NVARCHAR(100),
     "PurchasePoints"        NUMERIC(8,0) DEFAULT 0,
     "CreatedAt"             DATETIME2 NOT NULL DEFAULT GETDATE(),
     "CreatedBy"             NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "CreatedWith"           NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),
     "LastModifiedAt"        DATETIME2 NOT NULL DEFAULT GETDATE(),
     "LastModifiedBy"        NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "LastModifiedWith"      NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),

      -- Primary Key Constraint
      CONSTRAINT "PK_Customer"
         PRIMARY KEY CLUSTERED ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_Customer_ParentCustomer"
         FOREIGN KEY ("ParentInternalID")
         REFERENCES "Customer"("InternalID")
         ON DELETE NO ACTION
		 ON UPDATE NO ACTION
   );

-- Table Description
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@name=N'MS_Description', @value=N'Customer';

-- Column Descriptions
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'InternalID',
    @name=N'MS_Description', @value=N'Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'ParentInternalID',
	@name=N'MS_Description', @value=N'Parent Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'ParentRelationship',
	@name=N'MS_Description', @value=N'Parent Relationship';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'Name',
    @name=N'MS_Description', @value=N'Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'LegalName',
    @name=N'MS_Description', @value=N'Legal Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'ContactLastName',
    @name=N'MS_Description', @value=N'Contact Last Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'ContactFirstName',
    @name=N'MS_Description', @value=N'Contact First Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'ContactMiddleName',
    @name=N'MS_Description', @value=N'Contact Middle Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'ContactNickName',
    @name=N'MS_Description', @value=N'Contact Nick Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'BillingAddress1',
    @name=N'MS_Description', @value=N'Billing Address 1';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'BillingAddress2',
    @name=N'MS_Description', @value=N'Billing Address 2';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'BillingAddress3',
    @name=N'MS_Description', @value=N'Billing Address 3';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'BillingPostalCode',
    @name=N'MS_Description', @value=N'Billing Postal Code';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'BillingCountry',
    @name=N'MS_Description', @value=N'Billing Country';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'Telephone',
    @name=N'MS_Description', @value=N'Telephone';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'Email',
    @name=N'MS_Description', @value=N'Email';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'Memo',
    @name=N'MS_Description', @value=N'Memo';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'PurchasePoints',
    @name=N'MS_Description', @value=N'Purchase Points';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'CreatedAt',
    @name=N'MS_Description', @value=N'Created At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'CreatedBy',
    @name=N'MS_Description', @value=N'Created By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'CreatedWith',
    @name=N'MS_Description', @value=N'Created With';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedAt',
    @name=N'MS_Description', @value=N'Last Modified At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedBy',
    @name=N'MS_Description', @value=N'Last Modified By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedWith',
    @name=N'MS_Description', @value=N'Last Modified With';

GO

-- Unique Indexes
CREATE UNIQUE NONCLUSTERED INDEX "UQ_Customer_Name" ON "Customer" ("Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'UQ_Customer_Name',
    @name=N'MS_Description', @value=N'Name';

-- Indexes
CREATE NONCLUSTERED INDEX "IX_Customer_ParentInternalID_Name" ON "Customer" ("ParentInternalID", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_ParentInternalID_Name',
    @name=N'MS_Description', @value=N'Parent Internal ID, Name';

CREATE NONCLUSTERED INDEX "IX_Customer_LegalName_Name" ON "Customer" ("LegalName", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_LegalName_Name',
    @name=N'MS_Description', @value=N'Legal Name, Name';

CREATE NONCLUSTERED INDEX "IX_Customer_ContactLastName_ContactFirstName_Name" ON "Customer" ("ContactLastName", "ContactFirstName", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_ContactLastName_ContactFirstName_Name',
    @name=N'MS_Description', @value=N'Contact Last Name, Contact First Name, Name';

CREATE NONCLUSTERED INDEX "IX_Customer_BillingPostalCode_Name" ON "Customer" ("BillingPostalCode", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_BillingPostalCode_Name',
    @name=N'MS_Description', @value=N'Billing Postal Code, Name';

CREATE NONCLUSTERED INDEX "IX_Customer_Telephone_Name" ON "Customer" ("Telephone", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_Telephone_Name',
    @name=N'MS_Description', @value=N'Telephone, Name';

CREATE NONCLUSTERED INDEX "IX_Customer_PurchasePoints_Name" ON "Customer" ("PurchasePoints", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_PurchasePoints_Name',
    @name=N'MS_Description', @value=N'Purchase Points, Name';

CREATE NONCLUSTERED INDEX "IX_Customer_LastModifiedAt_Name" ON "Customer" ("LastModifiedAt", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Customer',
	@level2type=N'INDEX',    @level2name=N'IX_Customer_LastModifiedAt_Name',
    @name=N'MS_Description', @value=N'Last Modified At, Name';

GO

-- Triggers
CREATE TRIGGER "Customer_LastModified"
    ON "dbo"."Customer"
    AFTER UPDATE
    AS
    BEGIN
        UPDATE "dbo"."Customer"
		        SET "LastModifiedAt" = GETDATE(),
		            "LastModifiedBy" = SYSTEM_USER,
                    "LastModifiedWith" = APP_NAME()
		    FROM "dbo"."Customer"
			INNER JOIN inserted ON "Customer"."InternalID" = inserted."InternalID"
    END;
GO

-- =============================================================================
-- Shipping Address Table
-- =============================================================================

CREATE TABLE "dbo"."ShippingAddress"
    (
     "InternalID"            INT IDENTITY(5000,1) NOT NULL,
     "CustomerInternalID"    INT NOT NULL,
     "Name"                  NVARCHAR(50) NOT NULL,
     "ContactLastName"       NVARCHAR(50) NOT NULL,
     "ContactFirstName"      NVARCHAR(50) NOT NULL,
     "ContactMiddleName"     NVARCHAR(50),
     "ContactNickName"       NVARCHAR(50),
     "Address1"              NVARCHAR(30) NOT NULL,
     "Address2"              NVARCHAR(30) NOT NULL,
     "Address3"              NVARCHAR(30) NOT NULL,
     "PostalCode"            NVARCHAR(10) NOT NULL,
     "Country"               NVARCHAR(50) NOT NULL,
     "Telephone"             NVARCHAR(20),
     "Email"                 NVARCHAR(50),
     "Memo"                  NVARCHAR(100),
     "PurchasePoints"        NUMERIC(8,0),
     "CreatedAt"             DATETIME2 NOT NULL DEFAULT GETDATE(),
     "CreatedBy"             NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "CreatedWith"           NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),
     "LastModifiedAt"        DATETIME2 NOT NULL DEFAULT GETDATE(),
     "LastModifiedBy"        NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "LastModifiedWith"      NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),

      -- Primary Key Constraint
      CONSTRAINT "PK_ShippingAddress"
         PRIMARY KEY CLUSTERED ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_ShippingAddress_Customer"
         FOREIGN KEY ("CustomerInternalID")
         REFERENCES "Customer"("InternalID")
         ON DELETE NO ACTION
		 ON UPDATE NO ACTION
   );

-- Table Description
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@name=N'MS_Description', @value=N'Shipping Address';

-- Column Descriptions
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'InternalID',
    @name=N'MS_Description', @value=N'Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'CustomerInternalID',
    @name=N'MS_Description', @value=N'Customer Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Name',
    @name=N'MS_Description', @value=N'Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'ContactLastName',
    @name=N'MS_Description', @value=N'Contact Last Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'ContactFirstName',
    @name=N'MS_Description', @value=N'Contact First Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'ContactMiddleName',
    @name=N'MS_Description', @value=N'Contact Middle Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'ContactNickName',
    @name=N'MS_Description', @value=N'Contact Nick Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Address1',
    @name=N'MS_Description', @value=N'Address Line 1';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Address2',
    @name=N'MS_Description', @value=N'Address Line 2';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Address3',
    @name=N'MS_Description', @value=N'Address Line 3';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'PostalCode',
    @name=N'MS_Description', @value=N'Postal Code';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Country',
    @name=N'MS_Description', @value=N'Country';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Telephone',
    @name=N'MS_Description', @value=N'Telephone';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Email',
    @name=N'MS_Description', @value=N'Email';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'Memo',
    @name=N'MS_Description', @value=N'Memo';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'PurchasePoints',
    @name=N'MS_Description', @value=N'Purchase Points';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'CreatedAt',
    @name=N'MS_Description', @value=N'Created At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'CreatedBy',
    @name=N'MS_Description', @value=N'Created By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'CreatedWith',
    @name=N'MS_Description', @value=N'Created With';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedAt',
    @name=N'MS_Description', @value=N'Last Modified At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedBy',
    @name=N'MS_Description', @value=N'Last Modified By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedWith',
    @name=N'MS_Description', @value=N'Last Modified With';
GO

-- Unique Indexes
CREATE UNIQUE NONCLUSTERED INDEX "UQ_ShippingAddress_Name" ON "ShippingAddress" ("Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'INDEX',    @level2name=N'UQ_ShippingAddress_Name',
    @name=N'MS_Description', @value=N'Name';

-- Indexes
CREATE NONCLUSTERED INDEX "IX_ShippingAddress_PostalCode_Name" ON "ShippingAddress" ("PostalCode", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'INDEX',    @level2name=N'IX_ShippingAddress_PostalCode_Name',
    @name=N'MS_Description', @value=N'Postal Code, Name';

CREATE NONCLUSTERED INDEX "IX_ShippingAddress_ContactLastName_ContactFirstName_Name" ON "ShippingAddress" ("ContactLastName", "ContactFirstName", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'INDEX',    @level2name=N'IX_ShippingAddress_ContactLastName_ContactFirstName_Name',
    @name=N'MS_Description', @value=N'Contact Last Name, Contact First Name, Name';


CREATE NONCLUSTERED INDEX "IX_ShippingAddress_Telephone_Name" ON "ShippingAddress" ("Telephone", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'INDEX',    @level2name=N'IX_ShippingAddress_Telephone_Name',
    @name=N'MS_Description', @value=N'Telephone, Name';

CREATE NONCLUSTERED INDEX "IX_ShippingAddress_Email_Name" ON "ShippingAddress" ("Email", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'INDEX',    @level2name=N'IX_ShippingAddress_Email_Name',
    @name=N'MS_Description', @value=N'Email, Name';

CREATE NONCLUSTERED INDEX "IX_ShippingAddress_CustomerInternalID_Name" ON "ShippingAddress" ("CustomerInternalID", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'ShippingAddress',
	@level2type=N'INDEX',    @level2name=N'IX_ShippingAddress_CustomerInternalID_Name',
    @name=N'MS_Description', @value=N'Customer Internal ID, Name';

GO

-- Triggers
CREATE TRIGGER "ShippingAddress_LastModified"
    ON "dbo"."ShippingAddress"
    AFTER UPDATE
    AS
    BEGIN
        UPDATE "dbo"."ShippingAddress"
		        SET "LastModifiedAt" = GETDATE(),
		            "LastModifiedBy" = SYSTEM_USER,
                    "LastModifiedWith" = APP_NAME()
		    FROM "dbo"."ShippingAddress"
			INNER JOIN inserted ON "ShippingAddress"."InternalID" = inserted."InternalID"
    END;
GO

-- =============================================================================
-- Product
-- =============================================================================
CREATE TABLE "dbo"."Product"
    (
     "InternalID"            INT IDENTITY(5000,1) NOT NULL,
     "Code"                  NVARCHAR(25) NOT NULL,
     "Name"                  NVARCHAR(50) NOT NULL,
     "Description"           NVARCHAR(256),
     "Category"              NVARCHAR(50),
     "StandardCost"          NUMERIC(10,2),
     "ListPrice"             NUMERIC(10,2) NOT NULL,
     "ReorderLevel"          NUMERIC(4,0),
     "TargetLevel"           NUMERIC(4,0),
     "MinimumReorderQuantity" NUMERIC(4,0),
     "Discontinued"          BIT,
     "Memo"                  NVARCHAR(100),
     "ImagePath"             NVARCHAR(256),
     "CreatedAt"             DATETIME2 NOT NULL DEFAULT GETDATE(),
     "CreatedBy"             NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "CreatedWith"           NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),
     "LastModifiedAt"        DATETIME2 NOT NULL DEFAULT GETDATE(),
     "LastModifiedBy"        NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "LastModifiedWith"      NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),

      -- Primary Key Constraint
      CONSTRAINT "PK_Product"
         PRIMARY KEY CLUSTERED ("InternalID"),

   );

-- Table Description
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@name=N'MS_Description', @value=N'Product';

-- Column Descriptions
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'InternalID',
    @name=N'MS_Description', @value=N'Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'Code',
    @name=N'MS_Description', @value=N'Code';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'Name',
    @name=N'MS_Description', @value=N'Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'Description',
    @name=N'MS_Description', @value=N'Description';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'Category',
    @name=N'MS_Description', @value=N'Category';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'StandardCost',
    @name=N'MS_Description', @value=N'Standard Cost';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'ListPrice',
    @name=N'MS_Description', @value=N'List Price';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'ReorderLevel',
    @name=N'MS_Description', @value=N'Reorder Level';


EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'TargetLevel',
    @name=N'MS_Description', @value=N'Target Level';


EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'MinimumReorderQuantity',
    @name=N'MS_Description', @value=N'Minimum Reorder Quantity';

	
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'Discontinued',
    @name=N'MS_Description', @value=N'Discontinued';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'Memo',
    @name=N'MS_Description', @value=N'Memo';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'ImagePath',
    @name=N'MS_Description', @value=N'Image Path';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'CreatedAt',
    @name=N'MS_Description', @value=N'Created At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'CreatedBy',
    @name=N'MS_Description', @value=N'Created By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'CreatedWith',
    @name=N'MS_Description', @value=N'Created With';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedAt',
    @name=N'MS_Description', @value=N'Last Modified At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedBy',
    @name=N'MS_Description', @value=N'Last Modified By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedWith',
    @name=N'MS_Description', @value=N'Last Modified With';
GO

-- Unique Indexes
CREATE UNIQUE NONCLUSTERED INDEX "UQ_Product_Name" ON "Product" ("Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'INDEX',    @level2name=N'UQ_Product_Name',
    @name=N'MS_Description', @value=N'Name';

CREATE UNIQUE NONCLUSTERED INDEX "IX_Product_Code" ON "Product" ("Code");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'INDEX',    @level2name=N'IX_Product_Code',
    @name=N'MS_Description', @value=N'Code';

-- Indexes
CREATE NONCLUSTERED INDEX "IX_Product_Description_Name" ON "Product" ("Description", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'INDEX',    @level2name=N'IX_Product_Description_Name',
    @name=N'MS_Description', @value=N'Description, Name';

CREATE NONCLUSTERED INDEX "IX_Product_Category_Name" ON "Product" ("Category", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'INDEX',    @level2name=N'IX_Product_Category_Name',
    @name=N'MS_Description', @value=N'Category, Name';

CREATE NONCLUSTERED INDEX "IX_Product_Discontinued_Name" ON "Product" ("Discontinued", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'INDEX',    @level2name=N'IX_Product_Discontinued_Name',
    @name=N'MS_Description', @value=N'Discontinued, Name';

CREATE NONCLUSTERED INDEX "IX_Product_ListPrice_Name" ON "Product" ("ListPrice", "Name");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Product',
	@level2type=N'INDEX',    @level2name=N'IX_Product_ListPrice_Name',
    @name=N'MS_Description', @value=N'ListPrice, Name';
GO

-- Triggers
CREATE TRIGGER "Product_LastModified"
    ON "dbo"."Product"
    AFTER UPDATE
    AS
    BEGIN
        UPDATE "dbo"."Product"
		        SET "LastModifiedAt" = GETDATE(),
		            "LastModifiedBy" = SYSTEM_USER,
                    "LastModifiedWith" = APP_NAME()
		    FROM "dbo"."Product"
			INNER JOIN inserted ON "Product"."InternalID" = inserted."InternalID"
    END;
GO

-- =============================================================================
-- Order
-- =============================================================================
CREATE TABLE "dbo"."Order"
    (
     "InternalID"           INT IDENTITY(5000,1) NOT NULL,
     "CustomerInternalID"    INT NOT NULL,
     "OrderDateTime"         DATETIME2(0) NOT NULL DEFAULT GETDATE(),
     "PurchaseOrderNumber"   NVARCHAR(50) NOT NULL,
     "WarehouseInternalID"   INT,
     "WarehouseName"         NVARCHAR(50) NOT NULL,
     "DeliveryMemo"          NVARCHAR(100),
     "ShippingAddressInternalID" INT,
     "OrderMemo"             NVARCHAR(100),
     "Status"                NVARCHAR(10),
     "SalesPersonInternalID" INT,
     "SalesPersonName"       NVARCHAR(50) NOT NULL,
     "PurchasePoints"        NUMERIC(8,0),
     "CreatedAt"             DATETIME2 NOT NULL DEFAULT GETDATE(),
     "CreatedBy"             NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "CreatedWith"           NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),
     "LastModifiedAt"        DATETIME2 NOT NULL DEFAULT GETDATE(),
     "LastModifiedBy"        NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "LastModifiedWith"      NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),

      -- Primary Key Constraint
      CONSTRAINT "PK_Order"
         PRIMARY KEY CLUSTERED ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_Order_Customer"
         FOREIGN KEY ("CustomerInternalID")
         REFERENCES "Customer"("InternalID")
         ON DELETE NO ACTION
		 ON UPDATE NO ACTION,

      CONSTRAINT "FK_Order_ShippingAddress"
         FOREIGN KEY ("ShippingAddressInternalID")
         REFERENCES "ShippingAddress"("InternalID")
         ON DELETE NO ACTION
		 ON UPDATE NO ACTION
   );

-- Table Description
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@name=N'MS_Description', @value=N'Order';

-- Column Descriptions
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'InternalID',
    @name=N'MS_Description', @value=N'Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'CustomerInternalID',
    @name=N'MS_Description', @value=N'Customer Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'OrderDateTime',
    @name=N'MS_Description', @value=N'Order Date and Time';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'PurchaseOrderNumber',
    @name=N'MS_Description', @value=N'Purchase Order Number';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'WarehouseInternalID',
    @name=N'MS_Description', @value=N'Warehouse Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'WarehouseName',
    @name=N'MS_Description', @value=N'Warehouse Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'DeliveryMemo',
    @name=N'MS_Description', @value=N'Delivery Memo';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'ShippingAddressInternalID',
    @name=N'MS_Description', @value=N'Shipping Address Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'OrderMemo',
    @name=N'MS_Description', @value=N'Order Memo';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'Status',
    @name=N'MS_Description', @value=N'Status';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'SalesPersonInternalID',
    @name=N'MS_Description', @value=N'Sales Person Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'SalesPersonName',
    @name=N'MS_Description', @value=N'Sales Person Name';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'PurchasePoints',
    @name=N'MS_Description', @value=N'Purchase Points';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'CreatedAt',
    @name=N'MS_Description', @value=N'Created At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'CreatedBy',
    @name=N'MS_Description', @value=N'Created By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'CreatedWith',
    @name=N'MS_Description', @value=N'Created With';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedAt',
    @name=N'MS_Description', @value=N'Last Modified At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedBy',
    @name=N'MS_Description', @value=N'Last Modified By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedWith',
    @name=N'MS_Description', @value=N'Last Modified With';
GO

-- Unique Indexes

-- Indexes
CREATE NONCLUSTERED INDEX "IX_Order_OrderDateTime_InternalID" ON "Order" ("OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_CustomerInternalID_InternalID" ON "Order" ("CustomerInternalID", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_CustomerInternalID_InternalID',
    @name=N'MS_Description', @value=N'Customer Internal ID, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_CustomerInternalID_OrderDateTime_InternalID" ON "Order" ("CustomerInternalID", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_CustomerInternalID_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Customer Internal ID, Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_CustomerInternalID_PurchaseOrderNumber_OrderDateTime_InternalID" ON "Order" ("CustomerInternalID", "PurchaseOrderNumber", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_CustomerInternalID_PurchaseOrderNumber_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Customer Internal ID, Purchase Order Number, Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_PurchaseOrderNumber_OrderDateTime_InternalID" ON "Order" ("PurchaseOrderNumber", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_PurchaseOrderNumber_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Purchase Order Number, Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_SalesPersonName_OrderDateTime_InternalID" ON "Order" ("SalesPersonName", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_SalesPersonName_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Sales Person Name, Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_WarehouseName_OrderDateTime_InternalID" ON "Order" ("WarehouseName", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_WarehouseName_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Warehouse Name Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_Status_OrderDateTime_InternalID" ON "Order" ("Status", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_Status_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Status, Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_CustomerInternalID_Status_OrderDateTime_InternalID" ON "Order" ("CustomerInternalID", "Status", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_CustomerInternalID_Status_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'CustomerInternalID, Status, Order Date Time, Internal ID';

CREATE NONCLUSTERED INDEX "IX_Order_ShippingAddressInternalID_OrderDateTime_InternalID" ON "Order" ("ShippingAddressInternalID", "OrderDateTime", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'Order',
	@level2type=N'INDEX',    @level2name=N'IX_Order_ShippingAddressInternalID_OrderDateTime_InternalID',
    @name=N'MS_Description', @value=N'Shipping Address Internal ID, Order Date Time, Internal ID';

GO

-- Triggers
CREATE TRIGGER "Order_LastModified"
    ON "dbo"."Product"
    AFTER UPDATE
    AS
    BEGIN
        UPDATE "dbo"."Order"
		        SET "LastModifiedAt" = GETDATE(),
		            "LastModifiedBy" = SYSTEM_USER,
                    "LastModifiedWith" = APP_NAME()
		    FROM "dbo"."Order"
			INNER JOIN inserted ON "Order"."InternalID" = inserted."InternalID"
    END;
GO

-- =============================================================================
-- Order Item
-- =============================================================================
CREATE TABLE "dbo"."OrderItem"
    (
     "InternalID"            INT IDENTITY(5000,1) NOT NULL,
     "OrderInternalID"       INT NOT NULL,
     "ProductInternalID"     INT NOT NULL,
     "Quantity"              NUMERIC(4,0) NOT NULL,
     "UnitPrice"             NUMERIC(8,2),
     "DiscountPercent"       NUMERIC(4,1),
     "Memo"                  NVARCHAR(100),
     "CreatedAt"             DATETIME2 NOT NULL DEFAULT GETDATE(),
     "CreatedBy"             NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "CreatedWith"           NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),
     "LastModifiedAt"        DATETIME2 NOT NULL DEFAULT GETDATE(),
     "LastModifiedBy"        NVARCHAR(128) NOT NULL DEFAULT SYSTEM_USER,
     "LastModifiedWith"      NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),

      -- Primary Key Constraint
      CONSTRAINT "PK_OrderItem"
         PRIMARY KEY CLUSTERED ("InternalID"),

      -- Referential Foreign Key Constraints
      CONSTRAINT "FK_OrderItem_Order"
         FOREIGN KEY ("OrderInternalID")
         REFERENCES "Order"("InternalID")
         ON DELETE NO ACTION
		 ON UPDATE NO ACTION,

      CONSTRAINT "FK_OrderItem_Product"
         FOREIGN KEY ("ProductInternalID")
         REFERENCES "Product"("InternalID")
         ON DELETE NO ACTION
		 ON UPDATE NO ACTION
   );

-- Table Description
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@name=N'MS_Description', @value=N'Order Item';

-- Column Descriptions
EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'InternalID',
    @name=N'MS_Description', @value=N'Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'OrderInternalID',
    @name=N'MS_Description', @value=N'Order Interna lID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'ProductInternalID',
    @name=N'MS_Description', @value=N'Product Internal ID';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'Quantity',
    @name=N'MS_Description', @value=N'Quantity';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'UnitPrice',
    @name=N'MS_Description', @value=N'Unit Price';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'DiscountPercent',
    @name=N'MS_Description', @value=N'Discount (Percent)';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'Memo',
    @name=N'MS_Description', @value=N'Memo';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'CreatedAt',
    @name=N'MS_Description', @value=N'Created At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'CreatedBy',
    @name=N'MS_Description', @value=N'Created By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'CreatedWith',
    @name=N'MS_Description', @value=N'Created With';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedAt',
    @name=N'MS_Description', @value=N'Last Modified At';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedBy',
    @name=N'MS_Description', @value=N'Last Modified By';

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'COLUMN',   @level2name=N'LastModifiedWith',
    @name=N'MS_Description', @value=N'Last Modified With';

-- Unique Indexes

-- Indexes
CREATE NONCLUSTERED INDEX "IX_OrderItem_OrderInternalID_ProductInternalID_InternalID" ON "OrderItem" ("OrderInternalID", "ProductInternalID", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'INDEX',    @level2name=N'IX_OrderItem_OrderInternalID_ProductInternalID_InternalID',
    @name=N'MS_Description', @value=N'Order and Product Internal IDs, Internal ID';

CREATE NONCLUSTERED INDEX "IX_OrderItem_ProductInternalID_OrderInternalID_InternalID" ON "OrderItem" ("ProductInternalID", "OrderInternalID", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'INDEX',    @level2name=N'IX_OrderItem_ProductInternalID_OrderInternalID_InternalID',
    @name=N'MS_Description', @value=N'Product and Order Internal IDs, Internal ID';

CREATE NONCLUSTERED INDEX "IX_OrderItem_OrderInternalID_InternalID" ON "OrderItem" ("OrderInternalID", "InternalID");

EXEC sys.sp_addextendedproperty
	@level0type=N'SCHEMA',   @level0name=N'dbo',
	@level1type=N'TABLE',    @level1name=N'OrderItem',
	@level2type=N'INDEX',    @level2name=N'IX_OrderItem_OrderInternalID_InternalID',
    @name=N'MS_Description', @value=N'Order Internal ID, Internal ID';
GO

-- Triggers
CREATE TRIGGER "OrderItem_LastModified"
    ON "dbo"."OrderItem"
    AFTER UPDATE
    AS
    BEGIN
        UPDATE "dbo"."OrderItem"
		        SET "LastModifiedAt" = GETDATE(),
		            "LastModifiedBy" = SYSTEM_USER,
                    "LastModifiedWith" = APP_NAME()
		    FROM "dbo"."OrderItem"
			INNER JOIN inserted ON "OrderItem"."InternalID" = inserted."InternalID"
    END;
GO
