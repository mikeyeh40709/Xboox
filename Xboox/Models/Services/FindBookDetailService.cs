using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Xboox.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace Xboox.Services
{
    public class FindBookDetailService
    {
        private static XbooxLibraryDBContext _context;
        public FindBookDetailService()
        {
            _context = new XbooxLibraryDBContext();
        }
        /// <summary>
        /// Transform All books' type to ProductDetailViewModel type
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ProductDetailViewModel> FindBookDetail()
        {
            var ProductRepo = new GeneralRepository<Product>(_context);
            var ProductImgsRepo = new GeneralRepository<ProductImgs>(_context);
            var CategoryRepo = new GeneralRepository<Category>(_context);
            var ProductTagsRepo = new GeneralRepository<ProductTags>(_context);
            var TagsRepo = new GeneralRepository<Tags>(_context);
            var Products = ProductRepo.GetAll().ToList().Select(x => new ProductDetailViewModel()
            {
                Name = x.Name,
                Price = x.Price,
                CategoryID = x.CategoryId.ToString(),
                ProductId = x.ProductId.ToString(),
                UnitInStock = x.UnitInStock,
                ISBN = x.ISBN,
                Publisher = x.Publisher,
                Description = x.Description,
                Specification = x.Specification,
                Author = x.Author,
                Intro = x.Intro,
                Language = x.Language,
                ImgLinks = ProductImgsRepo.GetAll().Where(q => q.ProductId == x.ProductId).Select(j => j.imgLink).ToList(),
                ImgLink = ProductImgsRepo.GetFirst(y => y.ProductId == x.ProductId).imgLink,
                PublishedDate = x.PublishedDate.ToString("yyyy/MM/dd"),
                CategoryName = CategoryRepo.GetFirst(z => z.CategoryId == x.CategoryId).Name,
                TagId = ProductTagsRepo.GetFirst(z => z.ProductId == x.ProductId).TagId.ToString(),
                TagName = TagsRepo.GetFirst(y => y.TagId == _context.ProductTags.FirstOrDefault(k => k.ProductId == x.ProductId).TagId).TagName
            });
            return Products;
        }
        /// <summary>
        /// Search books by ProductId(GUID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDetailViewModel FindBookById(string id)
        {
            return FindBookDetail().FirstOrDefault(x => x.ProductId == id);
        }
        /// <summary>
        /// Search books by category, if it's "All" then return all books
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> FindBookByCategory(string CategoryName)
        {
            if (CategoryName == "All")
            {
                return FindBookDetail();
            }
            else
            {
                return FindBookDetail().Where(x => x.CategoryName == CategoryName);
            }
        }
        /// <summary>
        /// Search books by category and price range
        /// </summary>
        /// <param name="CategoryName">123</param>
        /// <param name="min_price">123</param>
        /// <param name="max_price">123</param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> FindBookByCateAndRange(string CategoryName, string min_price, string max_price)
        {
            if (min_price == null || max_price == null)
            {
                return FindBookByCategory(CategoryName);
            }
            else
            {
                return FindBookByCategory(CategoryName).Where(x => x.Price >= Convert.ToDecimal(min_price) && x.Price <= Convert.ToDecimal(max_price));
            }
        }
        /// <summary>
        /// Total of books by category and range
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <returns></returns>
        public int TotolBooksByCateAndRange(string CategoryName, string min_price, string max_price)
        {
            return FindBookByCateAndRange(CategoryName, min_price, max_price).Count();
        }
        /// <summary>
        /// Search books by name and price range
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> FindBookByNameAndRange(string Name, string min_price, string max_price)
        {
            var byCategory = FindBookByCategory("All").Where(x => x.Name.Contains(Name));
            if (min_price == null || max_price == null)
            {
                return byCategory;
            }
            else
            {
                return byCategory.Where(x => x.Price >= Convert.ToDecimal(min_price) && x.Price <= Convert.ToDecimal(max_price));
            }
        }
        /// <summary>
        /// Total of books by name and range
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <returns></returns>
        public int TotolBooksByNameAndRange(string Name, string min_price, string max_price)
        {
            return FindBookByNameAndRange(Name, min_price, max_price).Count();
        }
        /// <summary>
        /// Search all Tags and Order by first letter
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindTag()
        {
            var TagsRepo = new GeneralRepository<Tags>(_context);
            return TagsRepo.GetAll().Select(x => x.TagName).OrderBy(y => y.Substring(0, 1)).ToList();
        }
        /// <summary>
        /// Search all Tags, except "All"
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindTagSkipFirst()
        {
            return FindTag().Skip(1);
        }
        /// <summary>
        /// Search All Categories
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public string FindCategoryName(string CategoryName)
        {
            var CategoryRepo = new GeneralRepository<Category>(_context);
            return CategoryRepo.GetFirst(x => x.Name == CategoryName).Name;
        }
        /// <summary>
        /// Paging total books with category
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <param name="ActivePageNum"></param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> PagingTotalBooksWithCate(string CategoryName, string min_price, string max_price, int ActivePageNum = 1)
        {
            //products per page
            int pageRows = 12;
            //decide last book from last page, start next page with one after that 
            int startRow = (ActivePageNum - 1) * pageRows;
            //show products after paging
            var resultWithPaging = FindBookByCateAndRange(CategoryName, min_price, max_price).OrderBy(x => x.Name.Substring(0, 1)).Skip(startRow).Take(pageRows);
            return resultWithPaging;
        }
        /// <summary>
        /// Count total pages with category
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <returns></returns>
        public int CountTotalPagesWithCate(string CategoryName, string min_price, string max_price)
        {
            int pageRows = 12;
            int totalRows = FindBookByCateAndRange(CategoryName, min_price, max_price).Count();
            return totalRows % pageRows == 0 ? totalRows / pageRows : totalRows / pageRows + 1;
        }
        /// <summary>
        /// paging total books with name
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <param name="ActivePageNum"></param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> PagingTotalBooksWithName(string Name, string min_price, string max_price, int ActivePageNum = 1)
        {
            int pageRows = 12;
            int startRow = (ActivePageNum - 1) * pageRows;
            var resultWithPaging = FindBookByNameAndRange(Name, min_price, max_price).OrderBy(x => x.Name.Substring(0, 1)).Skip(startRow).Take(pageRows);
            return resultWithPaging;
        }
        /// <summary>
        /// Count total pages with name
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="min_price"></param>
        /// <param name="max_price"></param>
        /// <returns></returns>
        public int CountTotalPagesWithName(string Name, string min_price, string max_price)
        {
            int pageRows = 12;
            int totalRows = FindBookByNameAndRange(Name, min_price, max_price).Count();
            return totalRows % pageRows == 0 ? totalRows / pageRows : totalRows / pageRows + 1;
        }
    }
}