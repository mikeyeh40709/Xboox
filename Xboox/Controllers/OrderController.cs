using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.Models.ViewModels;
using Xboox.Services;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class OrderController : Controller
    {
        OrderService service = new OrderService();
        public ActionResult UserView()
        {
            ViewBag.Unpaid = 0;
            ViewBag.paid = 1;
            ViewBag.Cancel = 2;
            if (User.Identity.IsAuthenticated == true)
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
        public ActionResult ChangeState(string stateId)
        {
            if (service.EditState(stateId).isSuccessful)
            {
                return RedirectToAction("ManagerView");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // 刪除某筆訂單(使用者和後台)
        [HttpPost]
        public ActionResult CancelOrder(string orderId)
        {
            if (service.CancelOrder(orderId).isSuccessful)
            {
                if(User.Identity.IsAuthenticated == true)
                {
                    return Json(new { redirectToUrl = Url.Action("UserView", "Order") });
                }
                else
                {
                    return Json(new { redirectToUrl = Url.Action("ManagerView", "Order") });
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [Authorize]
        public ActionResult CreateOrder()
        {
            ShoppingCartManage shopCart = new ShoppingCartManage();
            var cartItems = shopCart.GetCartItems(this.HttpContext);
            return View(cartItems);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "PurchaserName,PurchaserAddress,PurchaserEmail,PurchaserPhone,StateId")] OrderViewModel order)
        {
            if(order.StateId == 1)
            {
                return RedirectToAction("CreditDetail");
            }
            else
            {
                var createOrder = service.CreateOrder(this.HttpContext, order);
                if (createOrder.isSuccessful)
                {
                    ViewBag.Success = "訂單建立成功";
                    return View("CreateOrderSuccess");
                }
                else
                {
                    var Error = createOrder.exception;
                    ViewBag.Error = Error.ToString();
                    return View("CreateOrderFail");
                }
            }
        }
        [Authorize]
        public ActionResult CreditDetail()
        {
            return View();
        }
        public ActionResult CreateOrderSuccess()
        {
            return View();
        }
    }
}