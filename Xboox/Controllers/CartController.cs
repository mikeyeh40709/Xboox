using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.DataTable;
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
            var cartproduct = context.Product.ToList().Select(x => new ProductDetailViewModel
            {
                Name = x.Name,
                UnitInStock = x.UnitInStock,
                Price = x.Price,
                imgLink = context.ProductImgs.FirstOrDefault(y=>y.ProductId == x.ProductId).imgLink
            });
            return View(cartproduct);
        }

        public ActionResult Bill()
        {
            return View();
        }
    }
}