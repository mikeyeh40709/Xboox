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


            var result = context.Product;
            var productimgs = context.ProductImgs;
            List<ProductDetailViewModel> item = new List<ProductDetailViewModel>();
            var query = from p in result
                        join pi in productimgs
                        on p.ProductImgId equals pi.ProductImgId
                        select new ProductDetailViewModel
                        {
                            Name = p.Name,
                            Quantity = p.Quantity,
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