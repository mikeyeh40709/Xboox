using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Xboox.Models;
using Xboox.Models.DataTable;
using Xboox.Models.Services;
using Xboox.ViewModels;
using Xboox.Repositories;

namespace Xboox.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        FindBookDetailRepository books = new FindBookDetailRepository();
       
        public ActionResult Index()
        {
            ShoppingCartManage cartManage = new ShoppingCartManage();
            GetKey getKey = new GetKey();
            var allKey = getKey.GetAllKey(HttpContext);
            ViewBag.XbooxKey = allKey;

            var products = books.FindBookDetail();
            return View(products);
            
        }
        public ActionResult ProductDetail(string id)
        {
            var products = books.FindBookDetail().FirstOrDefault(x=>x.ProductId == id);
            return View(products);
        }

    }
}