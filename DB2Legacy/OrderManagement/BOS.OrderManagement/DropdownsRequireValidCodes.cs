using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DataController;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOS.OrderManagement
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public abstract class RequireValidCodesAttribute : AB_ValidationAttribute
	{
		public RequireValidCodesAttribute()
		{
			ap_ValidationLocation = AB_ValidationLocations.Server;
			ap_ValidationTriggers = AB_ValidationTrigger.OnSave;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			am_InitializePropertiesFromValidationContext(validationContext);

			if (ap_ContextEntity == null)
				return ValidationResult.Success;

			if (am_SkipValidationBasedOnTriggerOrLocation())
				return ValidationResult.Success;

			if (am_SkipValidationBasedOnPropertiesToValidate(validationContext))
				return ValidationResult.Success;

			if (value == null)
				return ValidationResult.Success;    // nulls are handled by [AB_RequiredField] if not allowed

			if (IsModuleSpecificCodeValid(value))
				return ValidationResult.Success;
			else
				return new ValidationResult(validationContext.DisplayName);
		}

		protected abstract bool IsModuleSpecificCodeValid(object value);
	}

	public static class RequireValidCodesExtensions
	{
		public static bool AreDropdownCodesValid<Tentity, TinputArgs, TreturnArgs>(this AB_IBusinessObjectProcessBaseServiceContract<Tentity> bp, TinputArgs inputArgs, out TreturnArgs returnArgs)
			where Tentity : AB_BusinessObjectEntityBase
			where TinputArgs : AB_InputArgsBase
			where TreturnArgs : AB_ReturnArgsBase, new()
		{
			returnArgs = null;

			var entity = inputArgs.ap_InputEntity as Tentity;
			var properties = entity.ap_PropertyMetaDataObjects.Values
				.Where(pi => pi.ap_AttributesForProperty.Any(attr => attr is RequireValidCodesAttribute))
				.ToArray();

			if (properties == null || properties.Length == 0)
				return true;

			if (!entity.am_ValidateEntity(out ObservableCollection<AB_Message> errorMessages, properties))
			{
				returnArgs = new TreturnArgs
				{
					ap_ReturnCode = AB_ReturnCodes.ER.ToString(),
					ap_Messages = errorMessages.ToList()
				};
				return false;
			}

			return true;
		}
	}
}
