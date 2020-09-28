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
        public ActionResult Books(string CategoryName, string min_price, string max_price, int ActivePageNum = 1)
        {
            ViewBag.Tags = FindBook.FindTag();
            ViewBag.CateOrName = FindBook.FindCategory(CategoryName).Name;
            var TotalResult = FindBook.FindBookByCateAndRange(CategoryName, min_price, max_price);
            ViewBag.TotalResult = TotalResult.Count();

            int pageRows = 12;
            int totalRows = TotalResult.Count();
            int TotalPages = totalRows % pageRows == 0 ? totalRows / pageRows : totalRows / pageRows + 1;
            int startRow = (ActivePageNum - 1) * pageRows;
            var ResultWithPaging = TotalResult.OrderBy(x => x.Name.Substring(0, 1)).Skip(startRow).Take(pageRows);

            ViewBag.Active = ActivePageNum;
            ViewBag.Pages = TotalPages;
            return View(ResultWithPaging);
        }

        [OutputCache(Duration = 60)]
        public ActionResult BooksByName(string Name, string min_price, string max_price, int ActivePageNum = 1)
        {
            ViewBag.Tags = FindBook.FindTag();
            ViewBag.CateOrName = Name;
            var TotalResult = FindBook.FindBookByNameAndRange(Name, min_price, max_price);
            ViewBag.TotalResult = TotalResult.Count();

            int pageRows = 12;
            int totalRows = TotalResult.Count();
            int TotalPages = totalRows % pageRows == 0 ? totalRows / pageRows : totalRows / pageRows + 1;
            int startRow = (ActivePageNum - 1) * pageRows;
            var ResultWithPaging = TotalResult.OrderBy(x => x.Name.Substring(0, 1)).Skip(startRow).Take(pageRows);

            ViewBag.Active = ActivePageNum;
            ViewBag.Pages = TotalPages;
            return View("Books", ResultWithPaging);
        }
    }
}