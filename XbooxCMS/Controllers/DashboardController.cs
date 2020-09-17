using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Controllers
{
    public class DashboardController : Controller
    {
        private XbooxContext context = new XbooxContext();
        // GET: Dashboard
        public ActionResult GetSalesRevenue()
        {
            //要做年份限制 多加year(orderdata) 的where 選擇年份做動態選項
            var temp = from od in context.OrderDetails
                       join o in context.Order
                       on od.OrderId equals o.OrderId
                       where o.StateId == 2  && o.OrderDate.Year == 2020
                       select new
                       {
                           Month = o.OrderDate.Month,
                           Total = od.UnitPrice * od.Quantity
                       };
            var Revenue = from t in temp
                          group t by t.Month into g
                          select new SalesRevenueViewModel
                          {
                              Month = g.Key,
                              Revenue = g.Sum(x=>x.Total)
                          };
           

            return Json(Revenue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTop5Products()
        {
            var temp = from od in context.OrderDetails
                       join o in context.Order
                       on od.OrderId equals o.OrderId
                       where o.StateId == 2
                       select new
                       {
                           Month = o.OrderDate.Month,
                           Name = od.ProductName,
                           Quantity = od.Quantity
                       };

            var topproducts = from t in temp
                             group t by new { t.Month, t.Name } into g
                             select new TopProducts
                             {
                                 Month = g.Key.Month,
                                 Name = g.Key.Name,
                                 Quantity = g.Sum(x => x.Quantity)
                             };

            return Json(topproducts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJSON()
        {
            return View();
        }
    }
}