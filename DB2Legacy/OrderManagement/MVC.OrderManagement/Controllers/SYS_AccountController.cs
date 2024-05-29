//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.System.Global.asax.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================

using MVC.OrderManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Configuration;
using A4DN.Core.MVC.Shared.Models;
using A4DN.Core.MVC.Shared;
using A4DN.Core.MVC.Shared.Infrastructure.Attributes.Action;
using A4DN.Core.MVC.Shared.Infrastructure;
using A4DN.Core.MVC.Shared.Controllers;

namespace MVC.OrderManagement.Controllers
{
    /// <summary>
    /// Class SYS_AccountController.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.Shared.Controllers.AB_AccountController" />
    [Authorize]
    public class SYS_AccountController : AB_AccountController
    {
        // GET: /sys_account/login
        /// <summary>
        /// Logins the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="username">The username.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl, string username, string culture)
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            // Store the originating URL so we can attach it to a form field
            var accountLoginModel = new AB_LoginModel { ReturnUrl = returnUrl, UserName = username, DefaultCulture = string.IsNullOrWhiteSpace(culture) ? null : new System.Globalization.CultureInfo(culture) };

            _EnrollCultures(accountLoginModel);

            // Set Culture
            am_SetCulture(accountLoginModel.DefaultCulture);

            // Show Language Dropdown 
            // accountLoginModel.IsLanguageDropdownVisible = true;

            return View(accountLoginModel);
        }

        /// <summary>
        /// Enrolls the cultures.
        /// </summary>
        /// <param name="accountLoginModel">The account login model.</param>
        private static void _EnrollCultures(AB_LoginModel accountLoginModel)
        {
            //accountLoginModel.EnrolledCultures.AddRange(new List<System.Globalization.CultureInfo>() {
            //    new System.Globalization.CultureInfo("en-US"),
            //    new System.Globalization.CultureInfo("es-ES"),
            //    new System.Globalization.CultureInfo("fr-FR"),
            //    new System.Globalization.CultureInfo("ja-JP"),
            //    new System.Globalization.CultureInfo("pt-BR"),
            //    new System.Globalization.CultureInfo("zh-CN"),
            //});
        }

        // POST: /sys_account/login
        /// <summary>
        /// Logins the specified account login model.
        /// </summary>
        /// <param name="accountLoginModel">The account login model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AB_RequestThrottle]
        public async Task<ActionResult> Login(AB_LoginModel accountLoginModel)
        {
            _EnrollCultures(accountLoginModel);

            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(accountLoginModel);

            int systemNumber = Convert.ToInt32(ConfigurationManager.AppSettings["SystemNumber"]);

            var user = am_ValidateUser(accountLoginModel, systemNumber);
            if (user.ap_IsValidated)
            {
                // If the user came from a specific page, redirect back to it
                return RedirectToLocal(accountLoginModel.ReturnUrl);
            }

            // No existing user was found that matched the given criteria
            ModelState.AddModelError("", Resources.DescriptionResource.InvalidUsernameOrPassword);

            // If we got this far, something failed, redisplay form
            return View("Login", accountLoginModel);
        }

        // GET: /sys_account/error
        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Error()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }

        // POST: /sys_account/Logout
        /// <summary>
        /// Logout this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            am_SignOut();

            // Try to redirect to the configured default url
            var section = WebConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection;
            if (section != null)
            {
                var url = section.Forms.DefaultUrl;
                return Redirect(url);
            }

            // Otherwise we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
            return RedirectToLocal();
        }

        // GET: /sys_account/lock
        /// <summary>
        /// Locks this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
        }

        /// <summary>
        /// Redirects to local.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("index", "home");
        }

        /// <summary>
        /// Ensure logged out.
        /// </summary>
        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }

        #region Registration

        /// <summary>
        /// Instantiate and return an AB_RegistrationModel object or a subclass, initialized for
        /// starting a new registration process.
        /// </summary>
        /// <returns>AB_RegistrationModel.</returns>
        protected override AB_RegistrationModel am_GetEmptyRegistrationModel()
        {
            return new AccountRegistrationModel
            {
                ap_StageNumber = 0
            };
        }

        /// <summary>
        /// Display user registration form, all stages
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <remarks>Base method steps:
        /// 1. If Request.IsAuthenticated, call am_SignOut() and then redirect back to Register() method.
        /// 2. If passed model is null, call am_GetEmptyRegistrationModel() to get a new one.
        /// 3. Save the model
        /// 4. Display the "Register" view, passing in the model. The Register.cshtml view must handle
        /// rendering different forms for different values of ap_StageNumber.</remarks>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return am_Register(AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountRegistrationModel>(this));
        }

        /// <summary>
        /// Perform any additional validation the low-level needs, which isn't covered by the model's properties
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void am_AdditionalRegistrationValidation(AB_RegistrationModel model)
        {
            // Perform some additional server-side validation.

            // Email is checked at each stage, to guard against someone else registering with the email address
            // before we complete the registration process.
            if (AB_FrameworkHelper.ap_AdminFrameworkHelper.am_IsEmailAlreadyRegistered(((AccountRegistrationModel)model).Email))
            {
                ModelState.AddModelError("Email", Resources.DescriptionResource.EmailAddressAlreadyRegistered);
            }
            else
            {
                // If the email is ok, we have to clear any errors about previous failures

                ModelState state = null;
                if (ModelState.TryGetValue("Email", out state))
                {
                    ModelError errorItem = null;

                    state.Errors.ForEach(err =>
                    {
                        if (err.ErrorMessage == Resources.DescriptionResource.EmailAddressAlreadyRegistered)
                        {
                            errorItem = err;
                        }
                    });

                    if (errorItem != null)
                    {
                        state.Errors.Remove(errorItem);
                    }
                }
            }

            switch (model.ap_StageNumber)
            {
                // Stage-specific validation can be performed in cases here.
                default: break;
            };
        }

        /// <summary>
        /// Handle the form submission for each registration stage
        /// </summary>
        /// <param name="submittedModel">The submitted model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        /// <remarks>Base method steps:
        /// 1. Updated saved model with stage-specific values from submitted model
        /// 2. Call am_AdditionalRegistrationValidation(), which can update ModelState
        /// 3. Save the updated model
        /// 4. If ModelState.IsValid == false, redirect to Register() method to redisplay form
        /// 5. Call savedModel.am_ExecuteCurrentStage(), which updates ap_StageNumber, ap_ErrorLevel, and ap_ErrorMessage
        /// 6. If ap_StageNumber is null, call am_HandleSuccessfulRegistration()
        /// 7. Otherwise redirect to Register() method to display form for ap_StageNumber.</remarks>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountRegistrationModel submittedModel)
        {
            var savedModel = AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountRegistrationModel>(this);
            return await am_Register(submittedModel, savedModel);
        }

        /// <summary>
        /// Handle verification link included in verification email, which pre-fills a value for ap_UserSuppliedCode
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>ActionResult.</returns>
        /// <remarks>Base method steps:
        /// 1. Verify the model is still saved. If not, redirect to Register() method with a new model (from am_GetEmptyRegistrationModel) and an error about the code expiring.
        /// 2. Set the code in the model if it is not null/whitespace.
        /// 3. Save the model and redirect to Register() method to display the form for ap_StageNumber.</remarks>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registration(string code)
        {
            var savedModel = AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountRegistrationModel>(this);
            return am_Registration(savedModel, code);
        }

        /// <summary>
        /// Called at the end of the registration process, after ap_StageNumber has been set to null.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        /// <remarks>When this method is called, the user should be fully registered, and model will contain all of the registration
        /// information including the password the user selected, so that the user can be logged in automatically.</remarks>
        protected override Task<ActionResult> am_HandleSuccessfulRegistration(AB_RegistrationModel model)
        {
            var savedModel = model as AccountRegistrationModel;

            // Log the user in
            var loginModel = new AB_LoginModel
            {
                UserName = savedModel.UserID,
                Password = savedModel.Password,
                RememberMe = true
            };

            return Login(loginModel);
        }

        #endregion Registration

        #region Forgot Password

        // These methods work the same way the Registration methods work, displaying a sequence of forms with an emailed verification code linkback

        /// <summary>
        /// Empties forgot password model.
        /// </summary>
        /// <returns>AB_ForgotPasswordModel.</returns>
        protected override AB_ForgotPasswordModel am_GetEmptyForgotPasswordModel()
        {
            return new AccountForgotPasswordModel
            {
                ap_StageNumber = 0
            };
        }

        /// <summary>
        /// Forgot password.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return am_ForgotPassword(AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountForgotPasswordModel>(this));
        }

        /// <summary>
        /// Additional forgot password validation.
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void am_AdditionalForgotPasswordValidation(AB_ForgotPasswordModel model)
        {
            // Perform some additional server-side validation.

            switch (model.ap_StageNumber)
            {
                // Stage-specific validation can be performed in cases here.
                default: break;
            };
        }

        /// <summary>
        /// Forgot password.
        /// </summary>
        /// <param name="submittedModel">The submitted model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(AccountForgotPasswordModel submittedModel)
        {
            var savedModel = AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountForgotPasswordModel>(this);
            return await am_ForgotPassword(submittedModel, savedModel);
        }

        /// <summary>
        /// Recovers the password.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecoverPassword(string code)
        {
            var savedModel = AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountForgotPasswordModel>(this);
            return am_ForgotPassword(savedModel, code);
        }

        /// <summary>
        /// Handles successful password recovery.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        protected override Task<ActionResult> am_HandleSuccessfulPasswordRecovery(AB_ForgotPasswordModel model)
        {
            var savedModel = model as AccountForgotPasswordModel;

            // Log the user in
            var loginModel = new AB_LoginModel
            {
                UserName = savedModel.UserID,
                Password = savedModel.Password,
                RememberMe = true
            };

            return Login(loginModel);
        }

        #endregion Forgot Password

        #region Forgot ID

        // These methods work the same way the Registration methods work, displaying a sequence of forms with an emailed verification code linkback

        // NOTE: Forgot ID functionality is not implemented because the first step is to request the user's email
        // address to look up their Accelerator User, but we're using the email address as the user id.

        /// <summary>
        /// Gets empty forgot identifier model.
        /// </summary>
        /// <returns>AB_ForgotIDModel.</returns>
        protected override AB_ForgotIDModel am_GetEmptyForgotIDModel()
        {
            return new AccountForgotIDModel
            {
                ap_StageNumber = 0
            };
        }

        /// <summary>
        /// Forgot identifier.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotID()
        {
            return am_ForgotID(AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountForgotIDModel>(this));
        }

        /// <summary>
        /// Additional forgot identifier validation.
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void am_AdditionalForgotIDValidation(AB_ForgotIDModel model)
        {
            // Perform some additional server-side validation.

            switch (model.ap_StageNumber)
            {
                // Stage-specific validation can be performed in cases here.
                default: break;
            };
        }

        /// <summary>
        /// Forgot identifier.
        /// </summary>
        /// <param name="submittedModel">The submitted model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotID(AccountForgotIDModel submittedModel)
        {
            var savedModel = AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountForgotIDModel>(this);
            return await am_ForgotID(submittedModel, savedModel);
        }

        /// <summary>
        /// Recovers the identifier.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecoverID(string code)
        {
            var savedModel = AB_StagedFormModelExtensions.am_RetrieveSavedModel<AccountForgotIDModel>(this);
            return am_ForgotID(savedModel, code);
        }

        /// <summary>
        /// Handles successful identifier recovery.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        protected override Task<ActionResult> am_HandleSuccessfulIDRecovery(AB_ForgotIDModel model)
        {
            var savedModel = model as AccountForgotIDModel;

            // Send the user to the login page with their userid pre-filled and a message to supply their password

            // Since we're returning a RedirectToRouteResult instead of ActionResult, we have to use ContinueWith
            // to convert it. This wouldn't be necessary directly inside the ForgotID handler method, but since
            // we're several method calls deep from there, MVC can't figure out the conversion on its own.
            return Task.Factory.StartNew(ForgotID).ContinueWith<ActionResult>(
                t =>
                {
                    return RedirectToAction("Login", "SYS_Account", new { username = savedModel.ap_UserEntity.UserID });
                });
        }

        #endregion Forgot ID
    }
}