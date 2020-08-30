using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.ViewModels;
using Xboox.ViewModels;


namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        private XbooxContext context;
        private CartViewModel cartviewmodel;
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
        public ActionResult GetCart()
        {
            var cart = Models.Services.CartOperator.GetCurrentCart();
            
        
            var productDb = context.Product;
            var productImgDb = context.ProductImgs;
            //var cartviewmodelDb = cartviewmodel;
            List<CartViewModel> CartViewItem = new List<CartViewModel>();
            if (CartViewItem.Count == 0)
            {
                CartViewItem.Add(
                    new CartViewModel { Name = "A", Amount = 1, Total = 100 }

                    );
                //var query = from p in productDb
                //        join img in productImgDb
                //        on p.ProductImgId equals img.ProductImgId
                //        select new CartViewModel
                //        {
                //            Name = p.Name,
                //            ProductImg=img.imgLink,
                //            Amount=p.Quantity,
                //            Total = p.Price
                //        };

                //foreach (var item in query)
                //{
                //    CartViewItem.Add(item);
                //}


            }
            else
            {
                CartViewItem.First().Amount += 1;
            }
            return View(CartViewItem);
         
        }
    }
}