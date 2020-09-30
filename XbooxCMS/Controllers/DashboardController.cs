using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.Service;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Controllers
{
   
    public class DashboardController : Controller
    {
        
        // GET: Dashboard
        public ActionResult GetSalesRevenue()
        {
            DashboardService ds = new DashboardService();
            var Revenue = ds.GetSalesRevenue();

            return Json(Revenue, JsonRequestBehavior.AllowGet);
        }         

        public ActionResult GetJSON()
        {
            return View();
        }
    }
    public class TitleDataController : Controller 
    {
        public ActionResult GetTitleData()
        {
            DashboardService ds = new DashboardService();
            var titledata = ds.GetTitleData();
            return Json(titledata, JsonRequestBehavior.AllowGet);
        }
    }

    public class PieChajsController : Controller
    {
        public ActionResult GetTop5Products()
        {
            DashboardService ds = new DashboardService();
            var topproducts = ds.GetTopProducts();

            return Json(topproducts, JsonRequestBehavior.AllowGet);
        }
    }
}