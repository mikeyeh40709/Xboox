using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xboox
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Books",
                url: "Books/{CategoryName}",
                defaults: new { controller = "Book", action = "Books" }
                );
            routes.MapRoute(
                name: "BooksByRange",
                url: "Books/{CategoryName}/{min_price}-{max_price}",
                defaults: new { controller = "Book", action = "Books", CategoryName = "All" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
