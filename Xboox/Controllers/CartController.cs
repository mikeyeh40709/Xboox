using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.Models.ViewModels;
using Xboox.ViewModels;
using Newtonsoft.Json;


namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        private XbooxContext context = new XbooxContext();
        private ShoppingCartManage ShoppingCartManage = new ShoppingCartManage();
        public CartController()
        {
            if (context == null)
            {
                context = new XbooxContext();
            }
        }
        [HttpPost]
        public ActionResult AddToCart(string values)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Json(new { redirectToUrl = Url.Action("Login", "Account") });
            }
            else
            {
                var AddToCart = ShoppingCartManage.AddToCart();
                var AddToCartItems = ShoppingCartManage.AddToCartItems(values);
                if (AddToCart.isSuccessful && AddToCartItems.isSuccessful)
                {
                    ViewBag.SucssesAddToCart = "成功加入購物車";
                    return Json(new { redirectToUrl = Url.Action("ShopCart") });
                }
                else
                {
                    var Error = AddToCart.exception;
                    ViewBag.ErrorToAddCart = Error.ToString();
                    return Json(new { redirectToUrl = Url.Action("ShopCart") });
                }

            }

        }
        public ActionResult ShopCart()
        {
            var CouponDetails = context.Coupons.ToList();
            ViewBag.CouponCode = CouponDetails.Select(x => x.CouponCode).ToList();
            ViewBag.Discount = CouponDetails.Select(x => Convert.ToDouble(x.Discount)).ToList();
            ViewBag.StartTime = CouponDetails.Select(x => x.StartTime.ToString("yyyy/MM/dd")).ToList();
            ViewBag.EndTime = CouponDetails.Select(x => x.EndTime.ToString("yyyy/MM/dd")).ToList();
            var cart = new ShoppingCartManage();
            return View(cart.GetCartItems(this.HttpContext));
        }
        public ActionResult EmptyCart(string id)
        {
            var GetUserKey = HttpContext.Request.Cookies["VisitorKey"].Value;
            var cartItems = context.CartItems.Where(
                cart => cart.CartId.ToString() == GetUserKey && cart.ProductId.ToString() == id).ToList();

            foreach (var cartItem in cartItems)
            {
                context.CartItems.Remove(cartItem);

            }
            context.SaveChanges();



            return RedirectToAction("ShopCart");
        }
        public ActionResult Bill()
        {
            return View();
        }
    }

}

