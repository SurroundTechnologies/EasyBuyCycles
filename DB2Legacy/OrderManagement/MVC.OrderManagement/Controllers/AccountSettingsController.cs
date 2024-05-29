//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.System.Global.asax.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================

using MVC.OrderManagement.Models;
using A4DN.Core.MVC.LOB.Models;
using A4DN.Core.MVC.Shared;
using A4DN.Core.MVC.Shared.Controllers;
using A4DN.Core.MVC.Shared.Infrastructure.Attributes.Action;
using A4DN.Core.MVC.Shared.Infrastructure.Extensions;
using System.Web.Mvc;

namespace MVC.OrderManagement.Controllers
{
	/// <summary>
    /// Class AccountSettingsController.
    /// </summary>
    /// <seealso cref="A4DN.Core.MVC.Shared.Controllers.AB_BaseController" />
    [Authorize]
    [AB_CompressContent]
    public class AccountSettingsController : AB_BaseController
    {
        /// <summary>
        /// Gets the module explorer HTML view.
        /// </summary>
        /// <param name="moduleExplorerModel">The module explorer model.</param>
        /// <returns>JsonResult.</returns>
        [HttpGet]
        public JsonResult GetModuleExplorerHtmlView(AB_ModuleExplorerModel moduleExplorerModel)
        {
            var model = new AccountSettingsModel(User.Identity.Name);

            if (model.ap_AccountLookupErrorLevel != AB_ERRORLEVEL.SUCCESS || model.ap_UserEntity == null)
            {
                return this.AB_JsonErrorResult(Resources.DescriptionResource.AccountNotFound + ": " + model.ap_AccountLookupErrorMessage);
            }

            moduleExplorerModel.ap_PartialView_CustomExplorer = "_AccountSettings";
            moduleExplorerModel.ap_CustomModel = model;
            var markup = this.am_RenderView(moduleExplorerModel.ap_PartialView_ModuleExplorer, moduleExplorerModel);

            return this.AB_JsonResult(resultCode: moduleExplorerModel.ap_ReturnCode, message: moduleExplorerModel.ap_MessageString, markup: markup);
        }

        /// <summary>
        /// Updates the username.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        [AB_RequestThrottle]
        public JsonResult UpdateUsername(UpdateUserIDViewModel model)
        {
            var result = model.am_UpdateUserEntity(User.Identity.Name);

            if (result == null)
            {
                return this.AB_JsonResult(message: (Resources.DescriptionResource.InformationSaved));
            }
            else
            {
                return this.AB_JsonErrorResult(result);
            }
        }

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        [AB_RequestThrottle]
        public JsonResult UpdatePassword(UpdatePasswordViewModel model)
        {
            var result = model.am_UpdateUserEntity(User.Identity.Name);
            if (result == null)
            {
                return this.AB_JsonResult(message: (Resources.DescriptionResource.PasswordUpdated));
            }
            else
            {
                return this.AB_JsonErrorResult(result);
            }
        }

        /// <summary>
        /// Updates the security question and answer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        [AB_RequestThrottle]
        public JsonResult UpdateSecurityQA(UpdateSecurityQandAViewModel model)
        {
            var result = model.am_UpdateUserEntity(User.Identity.Name);
            if (result == null)
            {
                return this.AB_JsonResult(message: Resources.DescriptionResource.SecurityQuestionAnswerUpdated);
            }
            else
            {
                return this.AB_JsonErrorResult(result);
            }
        }
    }
}