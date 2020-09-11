using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Xboox.Models.DataTable;
using Xboox.ViewModels;
using Xboox.Repositories;
using System.Net;

namespace Xboox.Controllers
{
    public class BookCategoryController : Controller
    {
        FindBookDetailRepository findBook = new FindBookDetailRepository();
        private XbooxContext db = new XbooxContext();

        public ActionResult ShowBooks(string id)
        {
            ViewBag.Tags = db.Tags.Select(x => x.TagName).OrderBy(y => y.Substring(0, 1)).ToList();
            ViewBag.Category = db.Category.FirstOrDefault(x => x.CategoryId.ToString() == id).Name;
            ViewBag.CategoryID = db.Category.FirstOrDefault(x => x.CategoryId.ToString() == id).CategoryId;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == "dc5c22d1-ff3e-45fe-87e3-e577ee771551")
            {
                return View(findBook.FindBookDetail());
            }
            return View(findBook.FindBookDetail(id));
        }
    }
}