﻿using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CommercialGateAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            //{
            //    HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization, RemoteKey, CrowdSSOKey");
            //    HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            //    HttpContext.Current.Response.AddHeader("Content-Length", "0");
            //    HttpContext.Current.Response.End();
            //}
        }
    }
}
