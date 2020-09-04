using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Xboox.Models.DataTable;
using Xboox.ViewModels;

namespace Xboox.Controllers
{
    public class BookCategoryController : Controller
    {
        private XbooxContext context = new XbooxContext();
        
        // GET: CnBookPage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowAllCnBooks()
        {
            //List<BookCategoryViewModel> Cnbooks = new List<BookCategoryViewModel>();

            var CnBooks = context.Product.Where(z => z.CategoryId.ToString() == "761e6b0f-b8fd-45d2-bf35-9f09c7cc25b8").Select(y => new BookCategoryViewModel()
            {
                Name = y.Name,
                Price = y.Price,
                CategoryID = y.CategoryId.ToString(),
                imgLink = context.ProductImgs.FirstOrDefault(x => x.ProductId == y.ProductId).imgLink,
                CategoryName = context.Category.FirstOrDefault(x => x.CategoryId == y.CategoryId).Name,
                ProductId = y.ProductId.ToString()
                //Tags = context.ProductTags
            }).ToList();

            return View(CnBooks);
        }
    }
}