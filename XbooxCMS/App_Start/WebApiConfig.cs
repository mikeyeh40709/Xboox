﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace XbooxCMS.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();


           // config.Routes.MapHttpRoute(
           //    name: "OrderApi",
           //    routeTemplate: "api/{controller}/{action}/{id}",
           //    defaults: new { controller = "Order",id = RouteParameter.Optional }
           //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}