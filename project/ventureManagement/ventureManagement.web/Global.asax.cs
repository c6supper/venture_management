﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VentureManagement.BLL;
using VentureManagement.Web.Areas.LucenceEngine.Controllers;
using VentureManagement.Web.Filters;

//using ventureManagement.web.Filters;

namespace VentureManagement.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorize());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{exclude}/{extnet}/ext.axd");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            DatabaseInitilization.Initilization();

            AreaRegistration.RegisterAllAreas();

            AlarmController.GetAllIndex();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // If the user is logged-in, make sure his cache details are still available, otherwise redirect to login page.
            if (Request.IsAuthenticated && Membership.GetUser() == null)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}