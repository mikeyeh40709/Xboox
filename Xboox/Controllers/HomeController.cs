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
    public class HomeController : Controller
    {
        private XbooxContext context = new XbooxContext();
        public ActionResult Index()
        {
            //List<ProductDetailViewModel> result = new List<ProductDetailViewModel>();
            //Where(x => x.PublishedDate.Year >= 2019).
            var products = context.Product.ToList().Select(y => new ProductDetailViewModel
            {
                Name = y.Name,
                Price = y.Price,
                Tag = context.ProductTags.FirstOrDefault(z => z.ProductId == y.ProductId).TagId.ToString(),
                imgLink = context.ProductImgs.FirstOrDefault(k => k.ProductId == y.ProductId).imgLink,
                ProductId = y.ProductId.ToString()
            });
            return View(products);
        }
        public ActionResult ProductDetail(string id)
        {

            //var products = (from p in context.Product
            //                join pi in context.ProductImgs
            //               on p.ProductId equals pi.ProductId
            //                select new ProductDetailViewModel
            //                {
            //                    Name = p.Name,
            //                    UnitInStock = p.UnitInStock,
            //                    ISBN = p.ISBN,
            //                    Price = p.Price,
            //                    Publisher = p.Publisher,
            //                    Description = p.Description,
            //                    Specification = p.Specification,
            //                    Intro = p.Intro,
            //                    Author = p.Author,
            //                    Language = p.Language,
            //                    ProductId = p.ProductId.ToString(),
            //                    PublishedDate = p.PublishedDate.ToString("yyyy/MM/dd"),
            //                    imgLink = pi.imgLink
            //                }).ToList();
            var products = context.Product.ToList().Select(x => new ProductDetailViewModel
            {
                Name = x.Name,
                UnitInStock = x.UnitInStock,
                ISBN = x.ISBN,
                Price = x.Price,
                Publisher = x.Publisher,
                Description = x.Description,
                Specification = x.Specification,
                Author = x.Author,
                Intro = x.Intro,
                Language = x.Language,
                ProductId = x.ProductId.ToString(),
                PublishedDate = x.PublishedDate.ToString("yyyy/MM/dd"),
                imgLinks = context.ProductImgs.Where(z => z.ProductId == x.ProductId).Select(k => k.imgLink).ToList(),
                imgLink = context.ProductImgs.FirstOrDefault(y => y.ProductId == x.ProductId).imgLink
            });
            var productID = products.FirstOrDefault(x => x.ProductId == id);
            return View(productID);
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