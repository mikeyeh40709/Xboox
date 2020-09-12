using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Api
{
    public class ProductApiController : ApiController
    {
        private XbooxContext context = new XbooxContext();
        private static List<string> ImgstringList = null;
        
        //將圖片做同步處理
        private static List<string> GetImg()
        {
            if (ImgstringList == null)
            {
                ImgstringList = new List<string>();
            }
            return ImgstringList;
        }

        /// <summary>
        /// 拿到產品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProduct()
        {
            var productList = context.Product.ToList();
            List<ProductListViewModel> result = new List<ProductListViewModel>();
            var list = (from p in context.Product
                        join c in context.Category
                        on p.CategoryId equals c.CategoryId
                        select new ProductListViewModel()
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            UnitInStock = p.UnitInStock,
                            Price = p.Price,
                            Publisher = p.Publisher,
                            Author = p.Author,
                            PublishedDate = p.PublishedDate,
                            CategorName = c.Name
                        }).ToList();

            return Json(list);
        }

        /// <summary>
        /// 創建產品
        /// </summary>
        /// <param name="createViewModel"></param>
        /// <returns></returns>

        [HttpPost]
        public IHttpActionResult CreateProduct(CreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                return  BadRequest(ModelState);
            }
            createViewModel.Products.ProductId = Guid.NewGuid();

            PutImgs(createViewModel.Products);

            context.Product.Add(createViewModel.Products);

      //      AddedTag(createViewModel.Products, createViewModel.PostedTagIds, createViewModel.ProductTags);


            context.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }


        private void PutImgs(Product product)
        {
            var productImgs = new ProductImgs();
            //   var imgList = getImg().Split(',').Where(x=>x!="").ToList();


            //用list傳的
            foreach (var i in GetImg())
            {
                productImgs = new ProductImgs()
                {
                    // ProductImgId = 0,
                    imgLink = i,
                    ProductId = product.ProductId,
                };
                context.ProductImgs.Add(productImgs);
            }


            ImgstringList = null;
            context.SaveChanges();
        }


    }
}
