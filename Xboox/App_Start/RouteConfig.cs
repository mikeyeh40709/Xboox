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
            
            //Books/All/1
            routes.MapRoute(
                name: "Books",
                url: "Books/{CategoryName}/{ActivePageNum}",
                defaults: new { controller = "Book", action = "Books", CategoryName = "All", ActivePageNum = 1 }
                );
            //Books/All/300-600/1
            routes.MapRoute(
                name: "BooksByRange",
                url: "Books/{CategoryName}/{min_price}-{max_price}/{ActivePageNum}",
                defaults: new { controller = "Book", action = "Books", CategoryName = "All", min_price = 0, max_price = 9999, ActivePageNum = 1 }
                );
            //Find/MVC/1
            routes.MapRoute(
                name: "BooksByName",
                url: "Find/{Name}/{ActivePageNum}",
                defaults: new { controller = "Book", action = "BooksByName", Name = "MVC", ActivePageNum = 1 }
                );
            //Find/MVC/300-600/1
            routes.MapRoute(
                name: "BooksByNameAndRange",
                url: "Find/{Name}/{min_price}-{max_price}/{ActivePageNum}",
                defaults: new { controller = "Book", action = "BooksByName", Name = "MVC", min_price = 0, max_price = 9999, ActivePageNum = 1 }
                );
            //Detail/GUID
            routes.MapRoute(
                name: "BooksById",
                url: "Details/{id}",
                defaults: new { controller = "Home", action = "ProductDetail" }
                );
            //會員中心Route
            routes.MapRoute(
               name: "OrderUserView",
               url: "Member/Order",
               defaults: new { controller = "Order", action = "UserView" }
               );
            routes.MapRoute(
               name: "UserData",
               url: "Member/UserData",
               defaults: new { controller = "Account", action = "UserDataDetails"}
               );
            routes.MapRoute(
               name: "UserEdit",
               url: "Member/UserEdit",
               defaults: new { controller = "Account", action = "UserDataEdit"}
               );
            routes.MapRoute(
               name: "ChangePassword",
               url: "Member/ChangePassword",
               defaults: new { controller = "Manage", action = "ChangePassword" }
               );
            //訂單Route
            routes.MapRoute(
               name: "OrderByDateNum",
               url: "Member/Order/Time/{DateType}/{Num}",
               defaults: new { controller = "Order", action = "FilterDataByNum",DataType = "" , Num =""}
               );
            routes.MapRoute(
               name: "OrderByDateRange",
               url: "Member/Order/Range/{startDate}/{endDate}",
               defaults: new { controller = "Order", action = "FilterDataByRange" }
               );
            routes.MapRoute(
               name: "OrderByOrderId",
               url: "Member/Order/Id/{orderId}",
               defaults: new { controller = "Order", action = "FilterDataByOrderId" ,orderId=""}
               );
            //Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
