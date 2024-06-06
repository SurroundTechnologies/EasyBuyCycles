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
Source Member.: IBM i DDL EasyBuyCyclesModern Schema.sql
Created by ...: Lee Paul
Created on ...: September 14, 2022
Object Type ..: Schema Definition
Description ..: Customer

BUILD COMMAND.:
RUNSQLSTM SRCFILE(EASYBUYDM3/QDBSRC) SRCMBR("Customer")
    COMMIT(*NONE) ERRLVL(20)

================================================================================
                             Amendment Details
                             -----------------
Date       Programmer      Description
---------- --------------- -----------------------------------------------------


*******************************************************************************/

CREATE SCHEMA "EasyBuyCycles" FOR EASYBUYDM3;
-- Set the Library Description on the IBM i
CALL QSYS2.QCMDEXC ('CHGLIB LIB(EASYBUYDM3) TYPE(*PROD) TEXT(''EasyBuy Cycles'')');


CREATE SCHEMA "EasyBuyCyclesDev" FOR EASYBUYDV3;
-- Set the Library Description on the IBM i
CALL QSYS2.QCMDEXC ('CHGLIB LIB(EASYBUYDV3) TYPE(*PROD) TEXT(''EasyBuy Cycles Developmnet'')');