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
        public CartController()
        {
            if (context == null)
            {
                context = new XbooxContext();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocaltoSQL(string values)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Json(new { redirectToUrl = Url.Action("Login", "Account") });
               
            }
            else
            {
                var CartItems = JsonConvert.DeserializeObject<List<TempCartItems>>(values);
                var GetUserKey = HttpContext.Request.Cookies["VisitorKey"].Value;

                var cart = context.Cart.SingleOrDefault(ca => ca.CartId.ToString() == GetUserKey);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        CartId = Guid.Parse(GetUserKey),
                        UserId = HttpContext.User.Identity.Name
                    };
                    context.Cart.Add(cart);
                    context.SaveChanges();
                }

                foreach (var item in CartItems)
                {
                    var ProductCheck = context.CartItems.SingleOrDefault(
                   c => c.ProductId.ToString() == item.ProductId && c.CartId.ToString() == GetUserKey);
                    if (ProductCheck == null)
                    {
                        Guid randomId = Guid.NewGuid();
                        CartItems cartItem = new CartItems
                        {
                            CartId = Guid.Parse(GetUserKey),
                            ProductId = Guid.Parse(item.ProductId),
                            Quantity = item.Count,
                            Id = randomId
                        };
                        context.CartItems.Add(cartItem);
                    }
                }
                context.SaveChanges();
                return Json(new { redirectToUrl = Url.Action("ShopCart") });
            }
          
        }


        //public ActionResult AddToCart(string id)
        //{
        //    var addItem = context.Product
        //         .Single(p => p.ProductId.ToString() == id);
        //    var manage = new ShoppingCartManage();
        //    manage.AddToCart(addItem, this.HttpContext);
        //    return PartialView("_HomePageProductPartial");
        //}
       
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
