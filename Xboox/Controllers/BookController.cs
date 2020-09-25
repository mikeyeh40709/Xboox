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
            var result = FindBook.FindBookByRange(CategoryName, min_price, max_price);
            ViewBag.Result = result.Count();
            return View(result);
            //Todo changePage
            //int activePage = id;
            //int pageRows = 15;
            //int totalRows = products.Count();
            //int Pages = totalRows % pageRows == 0 ? Pages = totalRows / pageRows : Pages = totalRows / pageRows + 1;
            //int startRow = (activePage - 1) * pageRows;
            //products = products.OrderBy(x => x.Name.Substring(0, 1)).Skip(startRow).Take(pageRows);
            //ViewData["Active"] = id;
            //ViewData["Pages"] = Pages;
        }
        [OutputCache(Duration = 60)]
        public ActionResult BooksByName(string Name)
        {
            ViewBag.Tags = FindBook.FindTag();
            ViewBag.Category = Name;
            var result = FindBook.FindBookByName(Name);
            ViewBag.Result = result.Count();
            return View("Books", result);
        }
    }
}