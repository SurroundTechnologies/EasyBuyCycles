//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.System.Global.asax.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================

using A4DN.Core.MVC.AccountRegistration;

namespace MVC.OrderManagement.Models
{
    // ***********************************************************************
    // All models inherit from the accelerator core models. Refer to the surround support portal 
    // for information on how to implement your own custom models. 
    // ***********************************************************************

    /// <summary>
    /// Class AccountRegistrationModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_AccountRegistrationModel" />
    public class AccountRegistrationModel : AB_AccountRegistrationModel
    {
    }

    /// <summary>
    /// Class AccountForgotPasswordModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_AccountForgotPasswordModel" />
    public class AccountForgotPasswordModel : AB_AccountForgotPasswordModel
    {
    }

    /// <summary>
    /// Class AccountForgotIDModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_AccountForgotIDModel" />
    public class AccountForgotIDModel : AB_AccountForgotIDModel
    {
    }

    /// <summary>
    /// Class AccountSettingsModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_AccountSettingsModel" />
    public class AccountSettingsModel : AB_AccountSettingsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountSettingsModel"/> class.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        public AccountSettingsModel(string userID) : base(userID)
        {
        }
    }

    /// <summary>
    /// Class UpdateUserIDViewModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_UpdateUserIDViewModel" />
    public class UpdateUserIDViewModel : AB_UpdateUserIDViewModel
    {
    }

    /// <summary>
    /// Class UpdatePasswordViewModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_UpdatePasswordViewModel" />
    public class UpdatePasswordViewModel : AB_UpdatePasswordViewModel
    { 
	}

    /// <summary>
    /// Class UpdateSecurityQandAViewModel.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.AccountRegistration.AB_UpdateSecurityQandAViewModel" />
    public class UpdateSecurityQandAViewModel : AB_UpdateSecurityQandAViewModel
    { 
	}
}