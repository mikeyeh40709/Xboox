using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Controllers
{
    public class HomeController : Controller
    {
        private XbooxContext db = new XbooxContext();

        public ActionResult Index()
        {

            return View();
        }
        //public CreateGuid()
        //{
        // 
        //    return View();
        //}

        Guid productId = Guid.NewGuid();
        Guid productId2 = Guid.NewGuid();
        public ActionResult ProductIndex()
        {
            //List<ProductListViewModel> products = new List<ProductListViewModel>
            //{
            //   new ProductListViewModel{ProductId =  Guid.NewGuid() ,Name = "人間失格",Quantity = 236, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 236, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId =  Guid.NewGuid() ,Name = "人間失格",Quantity = 236, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId =  Guid.NewGuid() ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
            //   new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"}
            //};
            return View();
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

        public ActionResult Create()
        {
            //var tag = db.ProductTags.ToList();
          //  var products = db.Product.Include(p => p.ProductTags);

            //List<Product> products = new List<Product>()
            //{

            //};
            db.SaveChanges();
            return View();
        }
        public ActionResult Index_Clone()
        {

            return View();
        }

    }
}