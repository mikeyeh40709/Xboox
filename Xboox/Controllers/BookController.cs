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
            ViewBag.CateOrName = FindBook.FindCategoryName(CategoryName);
            ViewBag.TotalResult = FindBook.TotolBooksByCateAndRange(CategoryName, min_price, max_price);
            ViewBag.Active = ActivePageNum;
            ViewBag.Pages = FindBook.CountTotalPagesWithCate(CategoryName, min_price, max_price);

            return View(FindBook.PagingTotalBooksWithCate(CategoryName, min_price, max_price, ActivePageNum));
        }

        [OutputCache(Duration = 60)]
        public ActionResult BooksByName(string Name, string min_price, string max_price, int ActivePageNum = 1)
        {
            ViewBag.Tags = FindBook.FindTag();
            ViewBag.CateOrName = Name;
            ViewBag.TotalResult = FindBook.TotolBooksByNameAndRange(Name, min_price, max_price);
            ViewBag.Active = ActivePageNum;
            ViewBag.Pages = FindBook.CountTotalPagesWithName(Name, min_price, max_price);

            return View("Books", FindBook.PagingTotalBooksWithName(Name, min_price, max_price, ActivePageNum));
        }
    }
}