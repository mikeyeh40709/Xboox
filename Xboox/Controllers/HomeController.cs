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
    public class HomeController : Controller
    {
        private XbooxContext context = new XbooxContext();
        public ActionResult Index()
        {
            List<ProductDetailViewModel> item = new List<ProductDetailViewModel>();
            var query = (from p in context.Product
                         join pi in context.ProductImgs
                        on p.ProductId equals pi.ProductId
                        join t in context.ProductTags
                        on p.ProductId equals t.ProductId
                         select new ProductDetailViewModel
                         {
                             Name = p.Name,
                             Quantity = p.UnitInStock,
                             ISBN = p.ISBN,
                             Price = p.Price,
                             Publisher = p.Publisher,
                             Description = p.Description,
                             Specification = p.Specification,
                             Intro = p.Intro,
                             Author = p.Author,
                             Language = p.Language,
                             Tag = t.TagId.ToString(),
                             ProductId = p.ProductId.ToString(),
                             PublishedDate = p.PublishedDate.ToString().Remove(11),
                             imgLink = pi.imgLink
                         }).ToList();
            foreach (var product in query)
            {
                item.Add(product);
            }
            return View(item);
        }
        public ActionResult ProductDetail(string id)
        {
            List<ProductDetailViewModel> item = new List<ProductDetailViewModel>();
            var query = (from p in context.Product
                         join pi in context.ProductImgs
                        on p.ProductId equals pi.ProductId
                         select new ProductDetailViewModel
                         {
                             Name = p.Name,
                             Quantity = p.UnitInStock,
                             ISBN = p.ISBN,
                             Price = p.Price,
                             Publisher = p.Publisher,
                             Description = p.Description,
                             Specification = p.Specification,
                             Intro = p.Intro,
                             Author = p.Author,
                             Language = p.Language,
                             ProductId = p.ProductId.ToString(),
                             PublishedDate = p.PublishedDate.ToString().Remove(11),
                             imgLink = pi.imgLink
                         }).ToList();
            //foreach (var product in query)
            //{
            //    item.Add(product);
            //}
            var productID = query.Find(x => x.ProductId == id);
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