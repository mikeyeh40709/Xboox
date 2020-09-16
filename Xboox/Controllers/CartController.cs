using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Xboox.Models.Services;
using Xboox.Models.ViewModels;
using Xboox.ViewModels;
using Newtonsoft.Json;
using XbooxLibrary.Models.DataTable;

namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        private XbooxLibraryDBContext context = new XbooxLibraryDBContext();
        private ShoppingCartService shoppingCartService = new ShoppingCartService();
        public CartController()
        {
            if (context == null)
            {
                context = new XbooxLibraryDBContext();
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
                var AddToCart = shoppingCartService.AddToCart();
                var AddToCartItems = shoppingCartService.AddToCartItems(values);
                if (AddToCart.isSuccessful && AddToCartItems.isSuccessful)
                {
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
            var cart = new ShoppingCartManage();
            return View(cart.GetCartItems(this.HttpContext));
        }
        public ActionResult EmptyCart(string id)
        {
            if (shoppingCartService.EmptyCart(id).isSuccessful)
            {
                return RedirectToAction("ShopCart");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }        
        }
        public ActionResult Bill()
        {
            return View();
        }
    }

}

