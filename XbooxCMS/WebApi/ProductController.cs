using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using XbooxCMS.ViewModels;
using XbooxLibrary.Services;

namespace XbooxCMS.WebApi
{
    public class ProductController : ApiController
    {

        [System.Web.Http.HttpGet]
        public List<ProductListViewModel> GetllProducts()
        {
            var service = new ProductService();
            var pist = service.GetAllProducts();

            return pist;
        }



        [System.Web.Http.HttpGet]
        public List<string> GetllPictures(Guid id)
        {
            var service = new ProductService();
            var imgList = service.GetPictures(id);

            return imgList;
        }


        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteProduct(Guid id)
        {

            var service = new ProductService();
            service.Delete(id);
            return Json("Deleteok");
        }

        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        public HttpResponseMessage CreateProduct(CreateDataModel createDataModel)
        {

            var service = new ProductService();

            if (!ModelState.IsValid)
            {
                //建立失敗
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            service.Create(createDataModel);

            return  new HttpResponseMessage(HttpStatusCode.OK);
           
        }


        //[System.Web.Http.HttpPut]
        //[ValidateAntiForgeryToken]
        //public HttpResponseMessage EditProduct([FromBody]CreateDataModel DataModel)
        //{

        //    var service = new ProductService();



        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    service.Edit(DataModel);

        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}
    }
}
