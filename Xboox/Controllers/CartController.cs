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

            //List<ProductDetailViewModel> item = new List<ProductDetailViewModel>();
            //var query = from p in context.Product
            //            join pi in context.ProductImgs
            //            on p.ProductId equals pi.ProductId
            //            select new ProductDetailViewModel
            //            {
            //                Name = p.Name,
            //                Quantity = p.Quantity,
            //                Price = p.Price,
            //                imgLink = pi.imgLink
            //            };
            //foreach (var product in query)
            //{
            //    item.Add(product);
            //}
            //return View(item);


           var cart= ShoppingCartManage.GetCart(this.HttpContext);

            // Set up our ViewModel

            var viewModel = new CartViewModel()
            {
                CartItems = cart.GetCartItems(),
               
            };
            return View(viewModel);





        }

        public ActionResult Bill()
        {
            return View();
        }
    }
}