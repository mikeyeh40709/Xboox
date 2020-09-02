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

        public ActionResult ShopCart()
        {
            //var carts = new ShoppingCartManage();

            var cart = ShoppingCartManage.GetCart(this.HttpContext);

            //List<CartViewModel> item = new List<CartViewModel>();
            // Set up our ViewModel

            //public List<CartItems> GetCartItems()
            //{
            //    return xbooxDb.CartItems.Where(item => item.CartId.ToString() == ShoppingCartId).ToList();
            //}
            ////////////
            ////////////
            ///Mike
            //List<CartViewModel> item = new List<CartViewModel>();

            //var query = context.Product.Select(x => new CartViewModel
            //{
            //    Price = x.Price,
            //    Name = x.Name,
            //    ProductImgLink = context.ProductImgs.FirstOrDefault(y => y.ProductId == x.ProductId).imgLink,
            //    Count = context.CartItems.FirstOrDefault(k => k.ProductId == x.ProductId).Quantity
            //}).ToList();
            //foreach (var product in query)
            //{
            //    item.Add(product);
            //}
            ////////Mike
            //var viewModel = new CartViewModel
            //{
            //   CartList = cart.GetCartItems(),       
            //};
            return View(cart.GetCartItems());

            //var  viewM = cart.GetCartItems();

            //foreach (var item in viewM)
            //{
            //    item.
            //}


            // Return the view

        }
        //List<CartViewModel> item = new List<CartViewModel>();
        //var query = (from p in context.Product
        //             join pi in context.ProductImgs
        //             on p.ProductId equals pi.ProductId
        //             select new CartViewModel
        //             {
        //                 Name = p.Name,
        //                 Price=p.Price,
        //                 ProductImgLink=pi.imgLink

        //             }).ToList();
        //foreach (var product in query)
        //{
        //    item.Add(product);
        //}
        //return View(item);


        public ActionResult AddToCart(string id)
        {
            // Retrieve the album from the database
            var addItem = context.Product
                 .Single(p => p.ProductId.ToString() == id);
            // Add it to the shopping cart
            var cart = ShoppingCartManage.GetCart(this.HttpContext);
            //ShoppingCartManage.GetCart(this.HttpContext);
            cart.AddToCart(addItem, this.HttpContext);

            // Go back to the main store page for more shopping
            return RedirectToAction("ShopCart");
        }
        // AJAX: /ShoppingCart/RemoveFromCart/5

        // AJAX: /ShoppingCart/RemoveFromCart/5

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