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
        private static XbooxLibraryDBContext _context = new XbooxLibraryDBContext();

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
        /// 依據ProductId(GUID)搜尋產品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDetailViewModel FindBookById(string id)
        {
            return FindBookDetail().FirstOrDefault(x => x.ProductId == id);
        }
        /// <summary>
        /// 根據種類搜尋,若是ALL則回傳全部
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
        /// 根據種類,價格搜尋產品
        /// </summary>
        /// <param name="CategoryName">123</param>
        /// <param name="min_price">123</param>
        /// <param name="max_price">123</param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> FindBookByRange(string CategoryName, string min_price, string max_price)
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
        /// 根據名稱搜尋,若有包含及回傳
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public IEnumerable<ProductDetailViewModel> FindBookByName(string Name)
        {
            return FindBookByCategory("All").Where(x => x.Name.Contains(Name));
        }
        /// <summary>
        /// 回傳並排序標籤
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindTag()
        {
            var TagsRepo = new GeneralRepository<Tags>(_context);
            return TagsRepo.GetAll().Select(x => x.TagName).OrderBy(y => y.Substring(0, 1)).ToList();
        }
        /// <summary>
        /// 搜尋Tags,略過All
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindTagSkipFirst()
        {
            return FindTag().Skip(1);
        }
        /// <summary>
        /// 搜尋所有種類
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public Category FindCategory(string CategoryName)
        {
            var CategoryRepo = new GeneralRepository<Category>(_context);
            return CategoryRepo.GetFirst(x => x.Name == CategoryName);
        }
    }
}