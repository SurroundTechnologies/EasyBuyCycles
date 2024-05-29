//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.System.Global.asax.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
// using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
// using System.Web.SessionState;
// using A4DN.Core.BOS.Base;
using A4DN.Core.MVC.Shared;

namespace MVC.OrderManagement
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : AB_MvcApplication
	{
		protected void Application_Start()
        {
            // Remove all view engines except Razor (we only have cshtml templates)
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Remove the default Mobile display mode, because we want to force all
            // devices to use the same responsive markup and NOT the Accelerator Core
            // *.Mobile.cshtml templates
            am_ClearNonDefaultDisplayModes();


            // var retArgs = AB_HookEnrollment.am_EnrollConfigured();
            // if (!retArgs.ap_IsSuccess)
            //    AB_LoggerFactory.am_GetLogger().am_WriteLog("Global.asax.cs", AB_LogLevel.Warn, "Application_Start", string.Join("; ", retArgs.ap_Messages.Select(m => m.Text)));


            // Set SessionState Look Polling Interval: https://www.red-gate.com/simple-talk/dotnet/asp-net/single-asp-net-client-makes-concurrent-requests-writeable-session-variables/
            //var sessionStateModuleType = typeof(SessionStateModule);

            //var pollingIntervalFieldInfo = sessionStateModuleType.GetField("LOCKED_ITEM_POLLING_INTERVAL", BindingFlags.NonPublic | BindingFlags.Static);
            //pollingIntervalFieldInfo.SetValue(null, 100);    // default 500ms

            //var pollingDeltaFieldInfo = sessionStateModuleType.GetField("LOCKED_ITEM_POLLING_DELTA", BindingFlags.NonPublic | BindingFlags.Static);
            //pollingDeltaFieldInfo.SetValue(null, TimeSpan.FromMilliseconds(50.0));  // default 250ms
        }

		protected override void am_PreRequestSessionSetup(object sender, EventArgs e)
        {
             base.am_PreRequestSessionSetup(sender, e);

			// HINT: Any code that must be run at the start of each HTTP request, before
			// the Action method in the Controller is called, can be placed here. 
			// This method is called at the end of the MVC Application_PreRequestHandlerExecute
			// event handler in AB_MvcApplication.cs.
        }
	}
}