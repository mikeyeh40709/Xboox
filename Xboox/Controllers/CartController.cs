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
                ShoppingCartManage.AddToCart(values, this.HttpContext);
                return Json(new { redirectToUrl = Url.Action("ShopCart") });
            }
          
        }
        public ActionResult ShopCart()
        {
            var cart = new ShoppingCartManage();
            return View(cart.GetCartItems(this.HttpContext));
        }
        public ActionResult EmptyCart(string id)
        {
            var GetUserKey = HttpContext.Request.Cookies["VisitorKey"].Value;
            var cartItems = context.CartItems.Where(
                cart => cart.CartId.ToString() == GetUserKey && cart.ProductId.ToString() == id ).ToList();

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
