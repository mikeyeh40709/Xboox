﻿using Microsoft.AspNet.Identity;
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
        public ActionResult DeleteOrder(string orderId)
        {
            if (service.Delete(orderId).isSuccessful)
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

        public ActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateOrder(Order order)
        {
           if(service.CreateOrder(this.HttpContext, order).isSuccessful)
           {
                
                return RedirectToAction("CreateOrderSuccess");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult CreateOrderSuccess()
        {
            ViewBag.success = "訂單建立成功";
            return View();
        }
    }
}