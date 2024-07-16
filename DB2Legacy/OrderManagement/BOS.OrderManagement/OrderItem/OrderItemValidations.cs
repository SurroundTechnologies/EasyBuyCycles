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

namespace BOS.OrderItemDataEntity
{
	public class QuantityValidationMethod : AB_IValidation
	{
		public AB_ValidateMethodReturnArgs Validate(AB_ValidateMethodsInputArgs inputArgs)
		{
			OrderItemEntity entity = inputArgs.ap_EntityToValidate as OrderItemEntity;

			if (entity.Quantity < 1)
				return new AB_ValidateMethodReturnArgs(false);
			else
				return new AB_ValidateMethodReturnArgs(true);
		}
	}
}