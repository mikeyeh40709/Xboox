using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace Xboox.Services
{
    public class FindBookDetailService
    {
        private XbooxLibraryDBContext _context = new XbooxLibraryDBContext();

        public IEnumerable<ProductDetailViewModel> FindBookDetail(string CategoryName)
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
            if (CategoryName == "All")
            {
                return Products;
            }
            else
            {
                return Products.Where(x => x.CategoryName == CategoryName);
            }

        }
        public IEnumerable<ProductDetailViewModel> FindBookByRange(string CategoryName, string min_price, string max_price)
        {
            if (min_price == null || max_price == null)
            {
                return FindBookDetail(CategoryName);
            }
            else
            {
                return FindBookDetail(CategoryName).Where(x => x.Price >= Convert.ToDecimal(min_price) && x.Price <= Convert.ToDecimal(max_price));
            }
        }
        public IEnumerable<string> FindTag()
        {
            var TagsRepo = new GeneralRepository<Tags>(_context);
            return TagsRepo.GetAll().Select(x => x.TagName).OrderBy(y => y.Substring(0, 1)).ToList();
        }
        public Category FindCategory(string CategoryName)
        {
            var CategoryRepo = new GeneralRepository<Category>(_context);
            if (CategoryName == "All")
            {
                return CategoryRepo.GetFirst(x => x.CategoryId.ToString() == "dc5c22d1-ff3e-45fe-87e3-e577ee771551");
            }
            else
            {
                return CategoryRepo.GetFirst(x => x.Name == CategoryName);
            }
        }
    }
}