//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.Module.Controller.cs.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================

using A4DN.Core.BOS.Base;
using A4DN.Core.MVC.LOB.Controllers;
using A4DN.Core.MVC.LOB.Models;
using A4DN.Core.MVC.Shared.Infrastructure.Attributes.Action;
using A4DN.Core.MVC.Shared.Infrastructure.Extensions;
using BOS.ProductDataEntity;
using BOS.ProductDataMaps;
using BOS.ProductViewModel;
using System.Web.Mvc;

namespace MVC.OrderManagement.Controllers
{
	/// <summary>
    /// Class ProductController.
    /// </summary>
    [Authorize]
    [AB_CompressContent]
    public class ProductController : AB_ModuleController<ProductEntity, ProductVM, ProductMaps>
    {
        /// <summary>
        /// Gets the module explorer HTML view.
        /// </summary>
        /// <param name="moduleExplorerModel">The module explorer model.</param>
        /// <returns>JsonResult.</returns>
        [HttpGet]
        public JsonResult GetModuleExplorerHtmlView(AB_ModuleExplorerModel moduleExplorerModel)
        {
			// Setting to control how the am_SetUpVisualModelForContent method is called in the viewmodel. By default, this method will not be called.
			// moduleExplorerModel.ap_SelectionChangedAjaxTrigger = AB_SelectionChangedAjaxTrigger.OnSelectionChanged;
            am_SetModuleExplorerModelProperties(moduleExplorerModel);

            var markup = this.am_RenderView(moduleExplorerModel.ap_PartialView_ModuleExplorer, moduleExplorerModel);
            return this.AB_JsonResult(resultCode: moduleExplorerModel.ap_ReturnCode, message: moduleExplorerModel.ap_MessageString, markup: markup);
        }

        /// <summary>
        /// Gets the module detail HTML view.
        /// </summary>
        /// <param name="moduleDetailModel">The module detail model.</param>
        /// <returns>JsonResult.</returns>
        [HttpGet]
        public JsonResult GetModuleDetailHtmlView(AB_ModuleDetailModel moduleDetailModel)
        {
			// Setting to control how the am_OnCurrentEntityPropertyChanged method is called in the viewmodel. By default, this method will not be called. 
			// moduleDetailModel.ap_PropertyChangedAjaxTrigger = AB_PropertyChangedAjaxTrigger.OnInputControlChangedDelayedWait;
			 
            am_SetModuleDetailModelProperties(moduleDetailModel);

			if (moduleDetailModel.ap_ReturnCode == "OK")
            {
                var markup = this.am_RenderView(moduleDetailModel.ap_PartialView_ModuleDetail, moduleDetailModel);
                return this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString, markup: markup);
            }

            return this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString);
        }

        /// <summary>
        /// Search.
        /// </summary>
        /// <param name="moduleSearchModel">The module search model.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>JsonResult.</returns>
        [AB_AjaxOnly]
        public JsonResult Search(AB_ModuleSearchModel moduleSearchModel, ProductEntity entity)
        {
            var retArgs = am_Select(moduleSearchModel, entity);

            if (moduleSearchModel.ap_ReturnSearchCountOnly)
            {
                return this.AB_JsonResult(output: new { RecordCount = retArgs.ap_RecordCount });
            }

			if (moduleSearchModel.ap_ReturnExcelUrlOnly)
            {
                return this.AB_JsonResult(output: new { ExcelUrl = am_ExportToExcel(moduleSearchModel.ap_ViewColumns, moduleSearchModel.ap_ModuleNumber, retArgs.ap_OutputRecords) });
            }

            // Create Json Data Output
            var jsonResult = Json(am_CreateTableDataFromOutputUsingViewDisplayColumns(moduleSearchModel.ap_ViewName, moduleSearchModel.ap_ModuleNumber, moduleSearchModel.ap_OutputRecords, Request.Params["search[value]"], moduleSearchModel.ap_DrawCount, moduleSearchModel.ap_TotalCount, moduleSearchModel.ap_ReturnCode, moduleSearchModel.ap_MessageString), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        /// <summary>
        /// Fetches using the unique key.
        /// </summary>
        /// <param name="moduleDetailModel">The module detail model.</param>
        /// <returns>JsonResult.</returns>
        [HttpGet]
        public JsonResult FetchWithUniqueKey(AB_ModuleDetailModel moduleDetailModel)
        {
            var retArgs = am_FetchWithUniqueKey(moduleDetailModel);

            if (moduleDetailModel.ap_ReturnTableData)
            {  // Return Table Data format
                return this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString, output: am_CreateTableDataFromOutputUsingViewDisplayColumns(moduleDetailModel.ap_ViewName, moduleDetailModel.ap_ModuleNumber, retArgs.ap_OutputEntity, moduleDetailModel.ap_ReturnCode, moduleDetailModel.ap_MessageString).data);
            }
            var test = am_SerializeEntitytoJsonString(retArgs.ap_OutputEntity, moduleDetailModel.ap_TitleMode, moduleDetailModel.ap_ModuleName);
            // Return entity as a json object
            return this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString, output: am_SerializeEntitytoJsonString(retArgs.ap_OutputEntity, moduleDetailModel.ap_TitleMode, moduleDetailModel.ap_ModuleName));
        }

        /// <summary>
        /// Updates the specified entity model.
        /// </summary>
        /// <param name="moduleDetailModel">The module detail model.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(AB_ModuleDetailModel moduleDetailModel, ProductEntity entity)
        {
            am_SetModuleDetailModelUpdateProperties(moduleDetailModel, entity);

            if (ModelState.IsValid)
            {
                var retArgs = am_Update(moduleDetailModel, entity);

                if (retArgs.ap_IsSuccess)
                {
                    ModelState.Clear();
                    return GetModuleDetailHtmlView(moduleDetailModel);
                }
            }

            return moduleDetailModel.ap_ReturnCode == AB_ReturnCodes.OK.ToString()
                ? this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString, markup: this.am_RenderView(moduleDetailModel.ap_PartialView_ModuleDetail, moduleDetailModel))
                : this.AB_JsonErrorResult(moduleDetailModel.ap_MessageString);
        }

        /// <summary>
        /// Inserts the specified entity model.
        /// </summary>
        /// <param name="moduleDetailModel">The module detail model.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Insert(AB_ModuleDetailModel moduleDetailModel, ProductEntity entity)
        {
            am_SetModuleDetailModelInsertProperties(moduleDetailModel, entity);

            if (ModelState.IsValid)
            {
                var retArgs = am_Insert(moduleDetailModel, entity);

                if (retArgs.ap_IsSuccess)
                {
                    return GetModuleDetailHtmlView(moduleDetailModel);
                }
            }

            return moduleDetailModel.ap_ReturnCode == AB_ReturnCodes.OK.ToString()
                ? this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString, markup: this.am_RenderView(moduleDetailModel.ap_PartialView_ModuleDetail, moduleDetailModel))
                : this.AB_JsonErrorResult(moduleDetailModel.ap_MessageString);
        }

        /// <summary>
        /// Deletes using the unique key.
        /// </summary>
        /// <param name="moduleDetailModel">The module detail model.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult DeleteWithUniqueKey(AB_ModuleDetailModel moduleDetailModel)
        {
            var retArgs = am_DeleteWithUniqueKey(moduleDetailModel);

            return retArgs.ap_IsSuccess 
                ?  this.AB_JsonResult(resultCode: moduleDetailModel.ap_ReturnCode, message: moduleDetailModel.ap_MessageString, output: new { UniqueKey = moduleDetailModel.ap_UniqueKey })
                : this.AB_JsonErrorResult(moduleDetailModel.ap_MessageString);
        }
    }
}