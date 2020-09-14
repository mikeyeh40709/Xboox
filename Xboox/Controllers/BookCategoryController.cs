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
            var CategoryDetail = db.Category.FirstOrDefault(x => x.CategoryId.ToString() == id);
            ViewBag.Category = CategoryDetail.Name;
            ViewBag.CategoryID = CategoryDetail.CategoryId;
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

        public ActionResult FilterBooks(string maxPrice, string minPrice, string Id)
        {
            ViewBag.Tags = db.Tags.Select(x => x.TagName).OrderBy(y => y.Substring(0, 1)).ToList();
            var CategoryDetail = db.Category.FirstOrDefault(x => x.CategoryId.ToString() == Id);
            ViewBag.Category = CategoryDetail.Name;
            ViewBag.CategoryID = CategoryDetail.CategoryId;
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Id == "dc5c22d1-ff3e-45fe-87e3-e577ee771551")
            {
                if (maxPrice == null || minPrice == null)
                {
                    return View(findBook.FindBookDetail());
                }
                else
                {
                    return View(findBook.FindBookDetail(int.Parse(maxPrice), int.Parse(minPrice)));
                }
            }
            else if (maxPrice == null || minPrice == null)
            {
                return View(findBook.FindBookDetail(Id));
            }
            else
            {
                return View(findBook.FindBookDetail(Id, int.Parse(maxPrice), int.Parse(minPrice)));
            }
        }


        //public ActionResult Books(string maxPrice, string minPrice, string Id)
        //{

        //    ViewBag.Tags = db.Tags.Select(x => x.TagName).OrderBy(y => y.Substring(0, 1)).ToList();
        //    var category = db.Category.FirstOrDefault(x => x.CategoryId.ToString() == Id);
        //    ViewBag.Category = category.Name;
        //    ViewBag.CategoryID = category.CategoryId;
        //    if (Id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (Id == "dc5c22d1-ff3e-45fe-87e3-e577ee771551")
        //    {

        //        var a = findBook.FindBookDetail().Where(x => x.Price >= int.Parse(minPrice) && x.Price <= int.Parse(maxPrice)).ToList();

        //        return View(a);
        //    }

        //    return View(findBook.FindBookDetail(Id).Where(x => x.Price >= int.Parse(minPrice) && x.Price <= int.Parse(maxPrice)));

        //}

    }

}