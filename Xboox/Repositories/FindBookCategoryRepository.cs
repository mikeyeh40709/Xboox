using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;
using Xboox.ViewModels;

namespace Xboox.Repositories
{
    public class FindBookCategoryRepository
    {
        private  XbooxContext _context;
        public FindBookCategoryRepository()
        {
            _context = new XbooxContext();
        }

        public IEnumerable<BookCategoryViewModel> FindBookCategory(string CategoryId)
        {
            return FindBookCategory().Where(x => x.CategoryID.ToString() == CategoryId).ToList();
        }
        public IEnumerable<BookCategoryViewModel> FindBookCategory()
        {
            return _context.Product.Select(x => new BookCategoryViewModel()
            {
                Name = x.Name,
                Price = x.Price,
                CategoryID = x.CategoryId.ToString(),
                imgLink = _context.ProductImgs.FirstOrDefault(y => y.ProductId == x.ProductId).imgLink,
                CategoryName = _context.Category.FirstOrDefault(z => z.CategoryId == x.CategoryId).Name,
                ProductId = x.ProductId.ToString()
            }); ;
        }
    }
}