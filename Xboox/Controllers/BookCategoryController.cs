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

namespace Xboox.Controllers
{
    public class BookCategoryController : Controller
    {
        FindBookCategoryRepository findBook = new FindBookCategoryRepository();

        // GET: CnBookPage
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult ShowAllAllBooks(Guid? id)
        //{
        //    if(id.HasValue)
        //    {
        //        return View(findBook.FindBookCategory(id.ToString()));
        //    }
        //    return View(findBook.FindBookCategory());
        //}
        public ActionResult ShowAllAllBooks()
        {
            var AllBooks = findBook.FindBookCategory();
            return View(AllBooks);
        }
        public ActionResult ShowAllNovelBooks()
        {
            var Novel = findBook.FindBookCategory("a26d7886-3439-409a-adf9-45b47734bfae");
            return View(Novel);
        }
        public ActionResult ShowAllChineseBooks()
        {
            var ChineseBooks = findBook.FindBookCategory("761e6b0f-b8fd-45d2-bf35-9f09c7cc25b8");
            return View(ChineseBooks);
        }
        public ActionResult ShowAllComicsBooks()
        {
            var Comics = findBook.FindBookCategory("66ab9b96-6d3e-457f-b487-77ce7ab35d1c");
            return View(Comics);
        }
        public ActionResult ShowAllMagazineBooks()
        {
            var Magazine = findBook.FindBookCategory("8443c1ae-ab29-49df-854b-b04d44a65237");
            return View(Magazine);
        }
        public ActionResult ShowAllEBookBooks()
        {
            var EBook = findBook.FindBookCategory("21f3a449-0781-4d0c-bd1c-c9bb9d59c83c");
            return View(EBook);
        }
        public ActionResult ShowAllForeignLanguageBooks()
        {
            var ForeignDocs = findBook.FindBookCategory("1c39dc1e-525a-43ec-91bb-cb113c208dd3");
            return View(ForeignDocs);
        }
    }
}