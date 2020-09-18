using ECPay.Payment.Integration;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.Services;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class OrderController : Controller
    {
        private XbooxContext _context = new XbooxContext();
        OrderService orderservice = new OrderService();
        ShoppingCartService shoppingCartService = new ShoppingCartService();
        public ActionResult UserView()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var UserId = User.Identity.GetUserId();
                var result = orderservice.GetOrder(UserId);
                return View(result);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpGet]
        public ActionResult GetOrderDetails(string id)
        {
            var result = orderservice.GetOrderDetails(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CancelOrder(string orderId)
        {
            if (orderservice.CancelOrder(orderId).isSuccessful)
            {
                if (User.Identity.IsAuthenticated == true)
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
            var CouponDetails = _context.Coupons.ToList();
            ViewBag.CouponCode = CouponDetails.Select(x => x.CouponCode).ToList();
            ViewBag.Discount = CouponDetails.Select(x => Convert.ToDouble(x.Discount)).ToList();
            ViewBag.StartTime = CouponDetails.Select(x => x.StartTime.ToString("yyyy/MM/dd")).ToList();
            ViewBag.EndTime = CouponDetails.Select(x => x.EndTime.ToString("yyyy/MM/dd")).ToList();
            var cartItems = shoppingCartService.GetCartItems(this.HttpContext);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(OrderViewModel order)
        {
            var createOrder = orderservice.CreateOrder(this.HttpContext, order, null);
            if (createOrder.isSuccessful)
            {
                ViewBag.Success = "訂單建立成功";
                shoppingCartService.EmptyCart(User.Identity.GetUserId());
                return View("Success");
            }
            else
            {
                var Error = createOrder.exception;
                ViewBag.Error = Error.ToString();
                return View("Fail");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostToECPay(OrderViewModel order)
        {
            //### 廠商應做基本的Molde檢查(自行撰寫)
            var cartItems = shoppingCartService.GetCartItems(this.HttpContext);
            var ecpayNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            var createOrder = orderservice.CreateOrder(this.HttpContext, order, ecpayNumber);
            if (createOrder.isSuccessful)
            {
                ECPayService ecpayService = new ECPayService();

                //### 組合檢查碼
                string PostURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut";
                var postCollection = ecpayService.GetPostCollection(cartItems, order, ecpayNumber);
                //### Form Post To ECPay
                string ParameterString = string.Join("&", postCollection.Select(p => p.Key + "=" + p.Value));
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>").AppendLine();
                sb.Append("<form name='ECPayAIO'  id='ECPayAIO' action='" + PostURL + "' method='POST'>").AppendLine();
                foreach (var aa in postCollection)
                {
                    sb.Append("<input type='hidden' name='" + aa.Key + "' value='" + aa.Value + "'>").AppendLine();
                }

                sb.Append("</form>").AppendLine();
                sb.Append("<script> var theForm = document.forms['ECPayAIO'];  if (!theForm) { theForm = document.ECPayAIO; } theForm.submit(); </script>").AppendLine();
                sb.Append("<html><body>").AppendLine();
                Session.Add("CheckMacValue",postCollection["CheckMacValue"]);
                TempData["PostForm"] = sb.ToString();
                return View();
            }
            else
            {
                var Error = createOrder.exception;
                ViewBag.Error = Error.ToString();
                return View("Fail");
            }
        }
        [HttpPost]
        public ActionResult ECPayResult(FormCollection form)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in form.AllKeys)
            {
                result.Add(item, Request.Form.Get(item));
            }
            var RtnCode = result.FirstOrDefault(item => item.Key == "RtnCode").Value;
            var ReturnCheckMacValue = result.FirstOrDefault(item => item.Key == "CheckMacValue").Value;
            var MerchantTradeNo = result.FirstOrDefault(item => item.Key == "MerchantTradeNo").Value;
            string session = (string)Session["CheckMacValue"];
            if (RtnCode == "1")
            {
                orderservice.EditPaidState(MerchantTradeNo);
            }
            Response.Write("1|OK");
            this.Response.Flush();
            this.Response.End();
            return View();
        }
    }
}