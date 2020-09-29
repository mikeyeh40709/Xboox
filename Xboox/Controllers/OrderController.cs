using ECPay.Payment.Integration;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.Services;
using Xboox.ViewModels;
using XbooxLibrary.Models.DataTable;

namespace Xboox.Controllers
{
    public class OrderController : Controller
    {
        private XbooxLibraryDBContext _context = new XbooxLibraryDBContext();
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
        public ActionResult Checkout(string orderId)
        {
            // 拿到優惠券list
            var CouponDetails = _context.Coupons.ToList();
            ViewBag.CouponCode = CouponDetails.Select(x => x.CouponCode).ToList();
            ViewBag.Discount = CouponDetails.Select(x => Convert.ToDouble(x.Discount)).ToList();
            ViewBag.StartTime = CouponDetails.Select(x => x.StartTime.ToString("yyyy/MM/dd")).ToList();
            ViewBag.EndTime = CouponDetails.Select(x => x.EndTime.ToString("yyyy/MM/dd")).ToList();
            // 取得訂單詳細項目
            var orderInfo = orderservice.GetOrder(this.HttpContext, orderId).FirstOrDefault();
            var orderDetails = orderservice.GetOrderDetails(orderId);
            ViewBag.Items = orderDetails;
            if (orderDetails.FirstOrDefault().Coupon != null)
            {
                ViewBag.Coupon = orderDetails.FirstOrDefault().Coupon;
            }
            return View("CreateOrder", orderInfo);
        }
        public ActionResult CreateOrder()
        {
            // 拿到Coupon 資料
            var CouponDetails = _context.Coupons.ToList();
            ViewBag.CouponCode = CouponDetails.Select(x => x.CouponCode).ToList();
            ViewBag.Discount = CouponDetails.Select(x => Convert.ToDouble(x.Discount)).ToList();
            ViewBag.StartTime = CouponDetails.Select(x => x.StartTime.ToString("yyyy/MM/dd")).ToList();
            ViewBag.EndTime = CouponDetails.Select(x => x.EndTime.ToString("yyyy/MM/dd")).ToList();
            OrderViewModel orderInfo = new OrderViewModel();
            // 拿到cartItems
            var cartItems = shoppingCartService.GetCartItems(this.HttpContext);
            ViewBag.Items = cartItems;
            if (User.Identity.IsAuthenticated)
            {
                orderInfo = orderservice.GetRecordInfo(this.HttpContext);
            }
            return View(orderInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "PurchaserName,City,District,Road,PurchaserEmail,PurchaserPhone,Discount,Payment,Remember")] OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var createOrder = orderservice.CreateOrder(this.HttpContext, order, null);
                if (createOrder.isSuccessful)
                {
                    return View("Success");
                }
                else
                {
                    var Error = createOrder.exception;
                    ViewBag.Error = Error.ToString();
                    return View("Fail");
                }
            }
            return View("CreateOrder");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostToECPay([Bind(Include = "OrderId, EcpayOrderNumber,PurchaserName,City,District,Road,PurchaserEmail,PurchaserPhone,Discount,Payment,Remember")] OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                ECPayService ecpayService = new ECPayService();
                string PostURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/v5";
                var ecpayNumber = DateTime.Now.ToString("yyyyMMddHHmmss");

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>").AppendLine();
                sb.Append("<form name='ECPayAIO'  id='ECPayAIO' action='" + PostURL + "' method='POST'>").AppendLine();
                // 重新結帳
                if (order.OrderId != Guid.Empty)
                {
                    var orderDetails = orderservice.GetOrderDetails(order.OrderId.ToString());
                    var postCollection = ecpayService.GetPostCollection(orderDetails, order, ecpayNumber);
                    foreach (var aa in postCollection)
                    {
                        sb.Append("<input type='hidden' name='" + aa.Key + "' value='" + aa.Value + "'>").AppendLine();
                    }
                }
                // 新訂單
                else
                {
                    var cartItems = shoppingCartService.GetCartItems(this.HttpContext);
                    var createOrder = orderservice.CreateOrder(this.HttpContext, order, ecpayNumber);
                    var postCollection = ecpayService.GetPostCollection(cartItems, order, ecpayNumber);
                    foreach (var aa in postCollection)
                    {
                        sb.Append("<input type='hidden' name='" + aa.Key + "' value='" + aa.Value + "'>").AppendLine();
                    }
                    if (!createOrder.isSuccessful)
                    {
                        var Error = createOrder.exception;
                        ViewBag.Error = Error.ToString();
                        return View("Fail");
                    }
                }
                sb.Append("</form>").AppendLine();
                sb.Append("<script> var theForm = document.forms['ECPayAIO'];  if (!theForm) { theForm = document.ECPayAIO; } theForm.submit(); </script>").AppendLine();
                sb.Append("<html><body>").AppendLine();
                TempData["PostForm"] = sb.ToString();
                return View();
            }
            return View("CreateOrder");
        }
        [HttpPost]
        public ActionResult ECPayResult()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in Request.Form.AllKeys)
            {
                result.Add(item, Request.Form.Get(item));
            }
            string RtnCode = result.FirstOrDefault(item => item.Key == "RtnCode").Value;
            string ReturnCheckMacValue = result.FirstOrDefault(item => item.Key == "CheckMacValue").Value;
            string merchantTradeNo = result.FirstOrDefault(item => item.Key == "MerchantTradeNo").Value;
            string orderId = result.FirstOrDefault(item => item.Key == "CustomField1").Value;
            if (RtnCode == "1")
            {
                if(orderId != "")
                {
                    orderservice.EditPaidState(orderId);
                }
                orderservice.EditPaidStateByEcNumber(merchantTradeNo);
            }
            Response.Write("1|OK");
            this.Response.Flush();
            this.Response.End();
            return View();
        }
    }
}