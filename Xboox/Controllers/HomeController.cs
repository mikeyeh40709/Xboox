﻿using System;
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
    [RequireHttps]
    public class HomeController : Controller
    {
        private XbooxContext context = new XbooxContext();
        
        public ActionResult Index()
        {
            //ViewBag.UserState = User.Identity.IsAuthenticated;
            //ViewBag.UserName = User.Identity.Name;
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
                imgLink = context.ProductImgs.FirstOrDefault(y => y.ProductId == x.ProductId).imgLink,
                Category = context.Category.FirstOrDefault(j=> j.CategoryId == x.CategoryId).Name.ToString(),
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