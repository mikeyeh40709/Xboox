using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Services;
using XbooxLibrary.Models.DataTable;

namespace Xboox.Controllers
{
    public class BookController : Controller
    {
        FindBookDetailService FindBook = new FindBookDetailService();
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Duration = 60)]
        public ActionResult Books(string CategoryName, string min_price, string max_price)
        {
            ViewBag.Tags = FindBook.FindTag();
            ViewBag.Category = FindBook.FindCategory(CategoryName).Name;
            ViewBag.CategoryID = FindBook.FindCategory(CategoryName).CategoryId;
            return View(FindBook.FindBookByRange(CategoryName, min_price, max_price));
        }
        public ActionResult BooksByName(string Name)
        {
            ViewBag.Tags = FindBook.FindTag();
            ViewBag.Category = FindBook.FindCategory("All").Name;
            ViewBag.CategoryID = FindBook.FindCategory("All").CategoryId;
            //ViewBag.SideSuggestion = FindBook.FindBookDetail("All");
            return View("Books", FindBook.FindBookByName(Name));
        }
    }
}