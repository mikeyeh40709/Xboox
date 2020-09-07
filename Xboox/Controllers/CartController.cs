using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.Models.ViewModels;
using Xboox.ViewModels;

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
        // GET: Carts


        public ActionResult AddToCart(string id)
        {
            // Retrieve the album from the database
            var addItem = context.Product
                 .Single(p => p.ProductId.ToString() == id);
            // Add it to the shopping cart
            var manage = new ShoppingCartManage();
            //ShoppingCartManage.GetCart(this.HttpContext);
            manage.AddToCart(addItem, this.HttpContext);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index","Home");
        }
        public ActionResult ShopCart()
        {       
            var cart = new ShoppingCartManage();
            return View(cart.GetCartItems(this.HttpContext));
        }
        //public ActionResult EmptyCart()
        //{
        //    var GetUserKey = HttpContext.Request.Cookies["VisitorKey"].Value;
        //    var cartItems = context.CartItems.Where(
        //        cart => cart.CartId == Guid.Parse(GetUserKey)).ToList();

        //    foreach (var cartItem in cartItems)
        //    {
        //        context.CartItems.Remove(cartItem);
        //    }
        //    // Save changes
        //    context.SaveChanges();
        //    return RedirectToAction("ShopCart");

        //}



        public ActionResult Bill()
        {
            return View();
        }
    }
}

//var query = from p in context.Product.ToList()
//            join pi in context.ProductImgs.ToList()
//            on p.ProductId equals pi.ProductId
//            select new ProductDetailViewModel
//            {
//                Name = p.Name,
//                UnitInStock = p.UnitInStock,
//                Price = p.Price,
//                //imgLink = pi.imgLink.FirstOrDefault()
//            };
//var cartproduct = context.Product.ToList().Select(x => new ProductDetailViewModel
//{
//    Name = x.Name,
//    UnitInStock = x.UnitInStock,
//    Price = x.Price,
//    imgLink = context.ProductImgs.FirstOrDefault(y => y.ProductId == x.ProductId).imgLink
//});
//return View(cartproduct);