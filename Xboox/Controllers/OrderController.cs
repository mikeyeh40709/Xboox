using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.ViewModels;
using Xboox.Services;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class OrderController : Controller
    {
        OrderService service = new OrderService();
        // GET: Order
        public ActionResult UserView()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                var UserId = User.Identity.GetUserId();
                var result = service.GetOrder(UserId);
                return View(result);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }

        }
        public ActionResult ManagerView()
        {
            var result = service.GetOrder();
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetOrderDetails(string id)
        {
            var result = service.GetOrderDetails(id);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        // 編輯付款狀態(後台)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeState(string id)
        {
            return View();
        }
        // 刪除某筆訂單(使用者和後台)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder()
        {
            return View();
        }
    }
}