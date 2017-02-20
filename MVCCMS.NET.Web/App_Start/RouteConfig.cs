using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCCMS.NET.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               "index.html" ,
               "index.html" ,
               new { controller = "Home" , action = "Index" , id = UrlParameter.Optional } ,
               new string[] { "MVCCMS.NET.Web.Controllers" }
           );

            routes.MapRoute(
                "Default" ,
                "{controller}/{action}/{id}" ,
                new { controller = "Home" , action = "Index" , id = UrlParameter.Optional } ,
                new string[] { "MVCCMS.NET.Web.Controllers" }
            );
        }
    }
}
