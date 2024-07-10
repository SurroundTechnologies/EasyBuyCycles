//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="BOS.Module.ValidationMethods.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using A4DN.Core.BOS.Base;

namespace BOS.CustomerDataEntity
{
    //// A4DN_Tag: Validation Method Example
    // public class MyValidateMethod : AB_IValidation
    // {
    //     public AB_ValidateMethodReturnArgs Validate(AB_ValidateMethodsInputArgs inputArgs)
    //     {
    //         CustomerEntity entity = inputArgs.ap_EntityToValidate as CustomerEntity;
    //
    //         //condition logic and return true or false in AB_ValidateMethodReturnArgs
    //         if (MyConditionLogic)
    //             return new AB_ValidateMethodReturnArgs(false);
    //         else
    //             return new AB_ValidateMethodReturnArgs(true);         
    //      }
    // }

    public class ValidateParentRelationship : AB_IValidation
    {
        public AB_ValidateMethodReturnArgs Validate(AB_ValidateMethodsInputArgs InputArgs)
        {
            CustomerEntity entity = InputArgs.ap_EntityToValidate as CustomerEntity;
            if (entity.ParentInternalID != null && entity.ParentInternalID != 0 && string.IsNullOrEmpty(entity.ParentRelationship))
            {
                return new AB_ValidateMethodReturnArgs(false);
            }
            return new AB_ValidateMethodReturnArgs(true);
        }
    }

    public class ValidateLegalName : AB_IValidation
    {
        public AB_ValidateMethodReturnArgs Validate(AB_ValidateMethodsInputArgs InputArgs)
        {
            CustomerEntity entity = InputArgs.ap_EntityToValidate as CustomerEntity;

            if (entity.ParentInternalID != null && entity.ParentInternalID != 0) return new AB_ValidateMethodReturnArgs(true);

			if (string.IsNullOrEmpty(entity.LegalName)) return new AB_ValidateMethodReturnArgs(false);
            
            return new AB_ValidateMethodReturnArgs(true);
        }
    }
}