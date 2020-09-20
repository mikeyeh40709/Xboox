﻿using Microsoft.AspNet.Identity;
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
using Xboox.Services;
using System.Net;

namespace Xboox.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        FindBookDetailService books = new FindBookDetailService();
       
        public ActionResult Index()
        {
            Response.Cookies.Add(SetCookieService.SetCookie());
            var products = books.FindBookDetail("All");
            return View(products);
            
        }
        public ActionResult ProductDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var products = books.FindBookDetail("All").FirstOrDefault(x=>x.ProductId == id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

    }
}