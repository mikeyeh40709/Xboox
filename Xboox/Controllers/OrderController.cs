using ECPay.Payment.Integration;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
        private OrderService orderservice;
        private ShoppingCartService shoppingCartService;
        private XbooxLibraryDBContext _context;
        private FilterOrderDataService filterOrderService;
        public OrderController()
        {
            orderservice = new OrderService();
            shoppingCartService = new ShoppingCartService();
            _context = new XbooxLibraryDBContext();
            filterOrderService = new FilterOrderDataService();
        }
        /// <summary>
        /// 我的訂單
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 訂單細節
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOrderDetails(string id)
        {
            var result = orderservice.GetOrderDetails(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取消訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelOrder(string orderId)
        {
            if (orderservice.CancelOrder(orderId).isSuccessful)
            {
                return Json(new { redirectToUrl = Url.Action("UserView", "Order") });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// 建立訂單View
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 建立訂單View
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 重新結帳到綠界
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult PostToECPay(string orderId)
        {
            if (orderId != null)
            {
                ECPayService ecpayService = new ECPayService();
                string PostURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/v5";
                var ecpayNumber = DateTime.Now.ToString("yyyyMMddHHmmss");

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>").AppendLine();
                sb.Append("<form name='ECPayAIO'  id='ECPayAIO' action='" + PostURL + "' method='POST'>").AppendLine();
                // 重新結帳
                var order = orderservice.GetOrder(this.HttpContext,orderId).FirstOrDefault();
                var orderDetails = orderservice.GetOrderDetails(orderId.ToString());
                if (orderDetails.FirstOrDefault().Coupon != null)
                {
                    var discount = orderDetails.FirstOrDefault().Coupon.CouponCode;
                    order.Discount = discount;
                }
                var postCollection = ecpayService.GetPostCollection(orderDetails, order, ecpayNumber);
                foreach (var aa in postCollection)
                {
                    sb.Append("<input type='hidden' name='" + aa.Key + "' value='" + aa.Value + "'>").AppendLine();
                }
                sb.Append("</form>").AppendLine();
                sb.Append("<script> var theForm = document.forms['ECPayAIO'];  if (!theForm) { theForm = document.ECPayAIO; } theForm.submit(); </script>").AppendLine();
                sb.Append("<html><body>").AppendLine();
                TempData["PostForm"] = sb.ToString();
                return View();
            }
            return View("Index","Home");
        }
        /// <summary>
        /// 付款到綠界
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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
                sb.Append("</form>").AppendLine();
                sb.Append("<script> var theForm = document.forms['ECPayAIO'];  if (!theForm) { theForm = document.ECPayAIO; } theForm.submit(); </script>").AppendLine();
                sb.Append("<html><body>").AppendLine();
                TempData["PostForm"] = sb.ToString();
                return View();
            }
            return View();
        }
        /// <summary>
        /// 接收綠界參數
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ECPayResult()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in Request.Form.AllKeys)
            {
                result.Add(item, Request.Form.Get(item));
            }
            string RtnCode = result.FirstOrDefault(item => item.Key == "RtnCode").Value;
            string merchantTradeNo = result.FirstOrDefault(item => item.Key == "MerchantTradeNo").Value;
            string orderId = result.FirstOrDefault(item => item.Key == "CustomField1").Value;
            if (RtnCode == "1")
            {
                if (orderId != "")
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
        public ActionResult FilterDataByNum(string DateType, string Num)
        {
            var orderList = filterOrderService.filterOrders.FirstOrDefault(item => item.Key == DateType.ToUpper()).Value(this.HttpContext.User.Identity.GetUserId(),Convert.ToInt32(Num));
            return View("UserView",orderList);
        }
        public ActionResult FilterDataByRange(string startDate,string endDate)
        {
            var orderList = filterOrderService.GetOrderList(this.HttpContext.User.Identity.GetUserId(), startDate, endDate);
            return View("UserView", orderList);
        }
        public ActionResult FilterDataByOrderId(string orderId)
        {
            var orderList = filterOrderService.GetOrderList(this.HttpContext.User.Identity.GetUserId(), orderId);
            return View("UserView", orderList);
        }
    }
}