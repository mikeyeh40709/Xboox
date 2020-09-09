using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;
using Xboox.ViewModels;

namespace Xboox.Repositories
{
    public class FindBookDetailRepository
    {
        private XbooxContext _context;
        public FindBookDetailRepository()
        {
            _context = new XbooxContext();
        }

        public IEnumerable<ProductDetailViewModel> FindBookDetail(string CategoryId)
        {
            return FindBookDetail().Where(x => x.CategoryID == CategoryId).ToList();
        }
        public IEnumerable<ProductDetailViewModel> FindBookDetail()
        {
            return _context.Product.ToList().Select(x => new ProductDetailViewModel()
            {
                Name = x.Name,
                Price = x.Price,
                CategoryID = x.CategoryId.ToString(),
                ImgLink = _context.ProductImgs.FirstOrDefault(y => y.ProductId == x.ProductId).imgLink,
                ImgLinks = _context.ProductImgs.Where(q => q.ProductId == x.ProductId).Select(j => j.imgLink).ToList(),
                CategoryName = _context.Category.FirstOrDefault(z => z.CategoryId == x.CategoryId).Name,
                ProductId = x.ProductId.ToString(),
                TagId = _context.ProductTags.FirstOrDefault(z => z.ProductId == x.ProductId).TagId.ToString(),
                TagName = _context.Tags.FirstOrDefault(y => y.TagId == _context.ProductTags.FirstOrDefault(k => k.ProductId == x.ProductId).TagId).TagName,
                UnitInStock = x.UnitInStock,
                ISBN = x.ISBN,
                Publisher = x.Publisher,
                Description = x.Description,
                Specification = x.Specification,
                Author = x.Author,
                Intro = x.Intro,
                Language = x.Language,
                PublishedDate = x.PublishedDate.ToString("yyyy/MM/dd"),
            });
        }
       
    }
}