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
        public ActionResult Index()
        {

            var temp = from od in context.OrderDetails
                       join o in context.Order
                       on od.OrderId equals o.OrderId
                       where o.StateId == 2
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

        public ActionResult GetJSON()
        {
            return View();
        }
    }
}