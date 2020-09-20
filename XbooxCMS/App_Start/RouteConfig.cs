using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XbooxCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //  name: "TagsIndex",
            //  url: "api/{controller}/{action}/{id}",
            //  defaults: new { controller = "Tags", action = "Index", id = UrlParameter.Optional });


            //routes.MapRoute(
            //  name: "TagsCreateAndEdit",
            //  url: "api/{controller}/{action}/{id}",
            //  defaults: new { controller = "Tags", id = UrlParameter.Optional },
            //  constraints: new { action = "SaveEditTag|CreateTags" }
            //  );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );





        }
    }
}
