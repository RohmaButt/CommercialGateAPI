//using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CommercialGateAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();http://10.32.5.80
            config.EnableCors(new EnableCorsAttribute("*", "*", "*") /*{ SupportsCredentials = true }*/);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { controller = "Echo", action = "Get", id = RouteParameter.Optional }
        );
        }
    }
}
