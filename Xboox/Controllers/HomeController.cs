using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class HomeController : Controller
    {
        private XbooxContext context = new XbooxContext();
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult ProductDetail()
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
                            ISBN = p.ISBN,
                            Price = p.Price,
                            Publisher = p.Publisher,
                            Intro = p.Intro,
                            PublishedDate = p.PublishedDate.ToString(),
                            imgLink = pi.imgLink
                        };
            foreach(var product in query)
            {
                item.Add(product);
            }
            return View(item);
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}