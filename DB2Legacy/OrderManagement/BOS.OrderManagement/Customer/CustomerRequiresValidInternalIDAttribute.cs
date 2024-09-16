using A4DN.Core.BOS.Base;
using BOS.CustomerBusinessProcess;
using BOS.CustomerDataAccessLayer;
using BOS.CustomerDataEntity;
using BOS.OrderManagement.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOS.OrderManagement.Customer
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class CustomerRequiresValidInternalIDAttribute : RequireValidCodesAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			=> base.IsValid(value, validationContext);

		protected override bool IsModuleSpecificCodeValid(object value)
		{
			using (var bp = new CustomerBP())
			{
				var searchEntity = new CustomerEntity
				{
					CustomerInternalID = value as int?
				};

				var inArgs = new AB_FetchInputArgs<CustomerEntity>(searchEntity);
				inArgs.ap_Query = inArgs.ap_InputEntity.am_BuildDefaultQuery(AB_BuildQueryOptions.UseEqualsOperatorForStrings);

				// No inputArgs to get credentials from; use default credentials in ConnectionParms.config
				Utility.SetDatabaseCredentialsFromDefault<CustomerDALForSQL, CustomerEntity>(inArgs);

				var retArgs = bp.am_Fetch(inArgs).am_Cast<CustomerEntity>();

				if (!retArgs.ap_IsSuccess || retArgs.ap_FetchedEntity == null)
					return false;
			}

			return true;
		}
	}
}
