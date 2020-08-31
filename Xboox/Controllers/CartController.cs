using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.DataTable;
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
            ////var cartItems = _context.CartItmes.ToList();
            //List<CartViewModel> carts = new List<CartViewModel>()
            //{
            //    new CartViewModel{Name="Wellness And Paradise",ProductImg="Wellnes.png",Total=67},
            //    new CartViewModel{Name="Wellness And Paradise",ProductImg="Wellnes.png",Total=67}
            //};

            //return View(carts);

            List<ProductDetailViewModel> item = new List<ProductDetailViewModel>();
            var query = from p in context.Product
                        join pi in context.ProductImgs
                        on p.ProductId equals pi.ProductId
                        select new ProductDetailViewModel
                        {
                            Name = p.Name,
                            Quantity = p.UnitInStock,
                            Price = p.Price,
                            imgLink = pi.imgLink
                        };
            foreach (var product in query)
            {
                item.Add(product);
            }
            return View(item);
        }

        public ActionResult Bill()
        {
            return View();
        }
    }
}