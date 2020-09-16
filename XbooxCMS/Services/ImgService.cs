using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Repository;
using XbooxLibrary.Models.DataTable;

namespace XbooxCMS.Services
{
    public class ImgService
    {


        private static List<string> ImgstringList = null;

        private static List<string> GetImg()
        {
            if (ImgstringList == null)
            {
                ImgstringList = new List<string>();
            }
            return ImgstringList;
        }

        private void PutImgs(Product product)
        {

            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<ProductImgs> ImgRepo = new GeneralRepository<ProductImgs>(context);
            //   var imgList = getImg().Split(',').Where(x=>x!="").ToList();


            //用list傳的
            foreach (var i in GetImg())
            {
                ProductImgs productImgs = new ProductImgs()
                {
                    // ProductImgId = 0,
                    imgLink = i,
                    ProductId = product.ProductId,
                };
                ImgRepo.Create(productImgs);
               // context.ProductImgs.Add(productImgs);
            }



            //   用string 傳的
            //     foreach (var i in imgList)
            //    {
            //        productImgs = new ProductImgs()
            ////        {
            // ProductImgId = 0,
            //             imgLink = i,
            //             ProductId = product.ProductId,
            //          };
            //         context.ProductImgs.Add(productImgs);
            //productImgs.Add(new ProductImgs() { imgLink = i.ToString(), ProductId = product.ProductId });
            //    }


            // context.SaveChanges();

            //  var PiList = new List<ProductImgs>();
            // var img = productImgs
            ///    var productImgs = productImgs.Where(x => x.ProductId == product.ProductId);
            //    freach (var i in productImgs)
            //   {
            //   product.ProductImgId = i.ProductImgId + ",";
            //product.ProductImgId = Convert.ToInt64(product.ProductImgId) + ",";
            //   }

            //    imgString = null;
            ImgstringList = null;
            ImgRepo.SaveContext();
        }
    }
}