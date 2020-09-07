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
        FindBookDetailRepository findBook = new FindBookDetailRepository();

        // GET: CnBookPage
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult ShowAllAllBooks(Guid? id)
        //{
        //    if (id.HasValue)
        //    {
        //        return View(findBook.FindBookCategory(id.ToString()));
        //    }
        //    return View(findBook.FindBookCategory());
        //}
        public ActionResult ShowAllBooks()
        {
            var AllBooks = findBook.FindBookDetail();
            return View(AllBooks);
        }
        public ActionResult ShowNovelBooks()
        {
            var Novel = findBook.FindBookDetail("a26d7886-3439-409a-adf9-45b47734bfae");
            return View(Novel);
        }
        public ActionResult ShowChineseBooks()
        {
            var ChineseBooks = findBook.FindBookDetail("761e6b0f-b8fd-45d2-bf35-9f09c7cc25b8");
            return View(ChineseBooks);
        }
        public ActionResult ShowComicsBooks()
        {
            var Comics = findBook.FindBookDetail("66ab9b96-6d3e-457f-b487-77ce7ab35d1c");
            return View(Comics);
        }
        public ActionResult ShowMagazineBooks()
        {
            var Magazine = findBook.FindBookDetail("8443c1ae-ab29-49df-854b-b04d44a65237");
            return View(Magazine);
        }
        public ActionResult ShowEBookBooks()
        {
            var EBook = findBook.FindBookDetail("21f3a449-0781-4d0c-bd1c-c9bb9d59c83c");
            return View(EBook);
        }
        public ActionResult ShowForeignLanguageBooks()
        {
            var ForeignDocs = findBook.FindBookDetail("1c39dc1e-525a-43ec-91bb-cb113c208dd3");
            return View(ForeignDocs);
        }
    }
}