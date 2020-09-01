using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.Models.ViewModels;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        private XbooxContext context;
        public CartController()
        {
            if (context == null)
            {
                context = new XbooxContext();
            }
        }
        // GET: Carts

        public ActionResult ShopCart()
        {
            var cart = ShoppingCartManage.GetCart(this.HttpContext);

            // Set up our ViewModel

            var viewModel = new CartViewModel()
            {
                CartItems = cart.GetCartItems(),

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(string id)
        {
            // Retrieve the album from the database
            var addItem = context.Product
                .Single(p => p.ProductId.ToString() == id);

            // Add it to the shopping cart
            var cart =  ShoppingCartManage.GetCart(this.HttpContext);

            cart.AddToCart(addItem);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        // AJAX: /ShoppingCart/RemoveFromCart/5

        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        //public ActionResult RemoveFromCart(int id)
        //{
        //    // Remove the item from the cart
        //    var cart = ShoppingCartManage.GetCart(this.HttpContext);

        //    // Get the name of the album to display confirmation
        //    string bookName = context.Carts
        //        .Single(item => item.RecordId == id).Album.Title;

        //    // Remove from cart
        //    int itemCount = cart.RemoveFromCart(id);

        //    // Display the confirmation message
        //    var results = new ShoppingCartRemoveViewModel
        //    {
        //        Message = Server.HtmlEncode(albumName) +
        //            " has been removed from your shopping cart.",
        //        CartTotal = cart.GetTotal(),
        //        CartCount = cart.GetCount(),
        //        ItemCount = itemCount,
        //        DeleteId = id
        //    };
        //    return Json(results);
        //}



        public ActionResult Bill()
        {
            return View();
        }
    }
}