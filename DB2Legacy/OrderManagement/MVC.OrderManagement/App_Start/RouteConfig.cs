//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.System.Global.asax.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using A4DN.Core.MVC.Shared;

namespace MVC.OrderManagement
{
    public class RouteConfig : AB_RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            AB_RouteConfig.RegisterRoutes(routes, RouteFlags.AB_Content | RouteFlags.MainContentBrowser);

            #region Custom Routes

            // Add any routes.MapHttpRoute calls here that are needed above and beyond 
            // the routes defined by AB_RouteConfig.RegisterRoutes(). Typical uses would
            // be for AJAX API requests and non-module-based displays.
            //
            // For route debugging, turn on the RouteDebugger:Enabled setting in web.config

			routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "SYS_Account", action = "Login", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { controller = "SYS_Account", action = "Register", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Registration",
                url: "registration/{code}",
                defaults: new { controller = "SYS_Account", action = "Registration", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Forgot Password",
                url: "forgot-password",
                defaults: new { controller = "SYS_Account", action = "ForgotPassword", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Recover Password",
                url: "recover-password/{code}",
                defaults: new { controller = "SYS_Account", action = "RecoverPassword", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Forgot ID",
                url: "forgot-id",
                defaults: new { controller = "SYS_Account", action = "ForgotID", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Recover ID",
                url: "recover-id/{code}",
                defaults: new { controller = "SYS_Account", action = "RecoverID", code = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "SYS_Account", action = "Logout", code = UrlParameter.Optional }
                );

            #endregion

            AB_RouteConfig.RegisterGenericRoutes(routes, RouteFlags.MainGeneric | RouteFlags.AppModuleGeneric);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}