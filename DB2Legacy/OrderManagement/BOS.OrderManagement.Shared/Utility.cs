using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.DALBaseForSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOS.OrderManagement.Shared
{
	public static class Utility
	{
		/// <summary>
		/// Sets inputArgs.ap_UserId and ap_Password to default configuration from ConnectionParms.config
		/// </summary>
		/// <param name="inputArgs"></param>
		public static void SetDatabaseCredentialsFromDefault<TDAL, TEntity>(AB_InputArgsBase inputArgs) 
			where TDAL : AB_BusinessObjectDALBaseForSQL<TEntity>, new () 
			where TEntity : AB_BusinessObjectEntityBase
		{
			// This seems a little backward, but we have to do this when the System.UseLogonDBCredentials setting is true
			// if we want to use the UserID and Password that are in ConnectionParms. The inputArgs properties have to be
			// set otherwise an error is returned before the database connection is even attempted.
			var dal = new TDAL();
			inputArgs.ap_UserID = dal.ap_ConnectionParms.UserID;
			inputArgs.ap_Password = dal.ap_ConnectionParms.Password;
		}
	}
}
