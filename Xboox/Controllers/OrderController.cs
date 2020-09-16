using ECPay.Payment.Integration;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Xboox.Models.Services;
using Xboox.Services;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class OrderController : Controller
    {
        OrderService service = new OrderService();
        public ActionResult UserView()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var UserId = User.Identity.GetUserId();
                var result = service.GetOrder(UserId);
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
            var result = service.GetOrderDetails(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // 編輯付款狀態(後台)
        //[HttpPost]
        //public ActionResult ChangeState(string stateId)
        //{
        //    if (service.EditState(stateId).isSuccessful)
        //    {
        //        return RedirectToAction("ManagerView");
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //}
        // 刪除某筆訂單(使用者和後台)
        [HttpPost]
        public ActionResult CancelOrder(string orderId)
        {
            if (service.CancelOrder(orderId).isSuccessful)
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
            ShoppingCartService shopCart = new ShoppingCartService();
            var cartItems = shopCart.GetCartItems(this.HttpContext);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(OrderViewModel order)
        {
            var createOrder = service.CreateOrder(this.HttpContext, order);
            if (createOrder.isSuccessful)
            {
                ViewBag.Success = "訂單建立成功";
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
        public ActionResult PostToECPay([Bind(Include = "Payment")] OrderViewModel order)
        {
            //### 廠商應做基本的Molde檢查(自行撰寫)
            ShoppingCartService shopCart = new ShoppingCartService();
            var cartItems = shopCart.GetCartItems(this.HttpContext);

            //### 建立Service
            CommonService _CommonService = new CommonService();

            //### 組合檢查碼
            string PostURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut";
            string MerchantID = "2000132";
            string HashKey = "5294y06JbISpM5x9";
            string HashIV = "v77hoKGq4kWxNNIS";
            //AllPay
            AllInOne opay = new AllInOne();
            opay.Send.MerchantTradeNo = DateTime.Now.ToString("yyyyMMddHHmmss"); //廠商訂單編號
            opay.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //廠商訂單日期           
            opay.Send.TotalAmount = cartItems.Sum(item => item.Price * item.Quantity);
            opay.Send.TradeDesc = "ECPay訂單測試";
            foreach (var item in cartItems)
            {
                opay.Send.Items.Add(new Item()
                {
                    Name = item.Name,//商品名稱
                    Price = Convert.ToInt32(item.Price),//商品單價
                    Currency = "元",//幣別單位
                    Quantity = Convert.ToInt32(item.Quantity),//購買數量
                });
            }
            var payment = service.payment.FirstOrDefault(x => x.Key == order.Payment).Value;

            // 形成字串
            SortedDictionary<string, string> PostCollection = new SortedDictionary<string, string>();
            PostCollection.Add("MerchantID", MerchantID);
            PostCollection.Add("MerchantTradeNo", opay.Send.MerchantTradeNo);
            PostCollection.Add("MerchantTradeDate", opay.Send.MerchantTradeDate);
            PostCollection.Add("PaymentType", "aio");//固定帶aio
            PostCollection.Add("TotalAmount", Convert.ToInt32(opay.Send.TotalAmount).ToString());
            PostCollection.Add("TradeDesc", opay.Send.TradeDesc);
            PostCollection.Add("ItemName", opay.Send.ItemName);
            PostCollection.Add("ReturnURL", "https://54ac7b1502d0.ngrok.io/Order/ECPayResult");//廠商通知付款結果API
            PostCollection.Add("ChoosePayment", payment);
            PostCollection.Add("EncryptType", "1");//固定

            //壓碼
            string str = string.Empty;
            string str_pre = string.Empty;
            foreach (var item in PostCollection)
            {
                str += string.Format("&{0}={1}", item.Key, item.Value);
            }

            str_pre += string.Format("HashKey={0}" + str + "&HashIV={1}", HashKey, HashIV);

            string urlEncodeStrPost = HttpUtility.UrlEncode(str_pre);
            string ToLower = urlEncodeStrPost.ToLower();
            string sCheckMacValue = _CommonService.GetSHA256(ToLower);
            PostCollection.Add("CheckMacValue", sCheckMacValue);

            //### Form Post To ECPay

            string ParameterString = string.Join("&", PostCollection.Select(p => p.Key + "=" + p.Value));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<html><body>").AppendLine();
            sb.Append("<form name='ECPayAIO'  id='ECPayAIO' action='" + PostURL + "' method='POST'>").AppendLine();
            foreach (var aa in PostCollection)
            {
                sb.Append("<input type='hidden' name='" + aa.Key + "' value='" + aa.Value + "'>").AppendLine();
            }

            sb.Append("</form>").AppendLine();
            sb.Append("<script> var theForm = document.forms['ECPayAIO'];  if (!theForm) { theForm = document.ECPayAIO; } theForm.submit(); </script>").AppendLine();
            sb.Append("<html><body>").AppendLine();

            TempData["PostForm"] = sb.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult ECPayResult(AllInOne.SendArguments result)
        {
            //service.GetOrder().FirstOrDefault(item => item.OrderId == );
            ViewBag.result = result;
            return View();
        }
    }
}