using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SabraMedicalCenter
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{area}/{id}",
                defaults: new { controller = "McHomeHandle", action = "Index", area="McHome", id = UrlParameter.Optional }
            ).DataTokens = new RouteValueDictionary(new { area = "McHome" });
        }
    }
}
