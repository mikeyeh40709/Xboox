using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using XbooxLibrary.Services;

namespace XbooxCMS.WebApi
{
    //[System.Web.Http.RoutePrefix("api/{controller}/{action}")]
    public class ImgController : ApiController
    {
        /// <summary>
        /// 在Edit中單獨刪除圖片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteImg(string query)
        {
            var service = new ProductService();
            service.RemoveImg(query);
            return Json("Deleteok");
        }

        /// <summary>
        /// create裡移除所有上傳圖片
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public IHttpActionResult RemoveAll()
        {
            var service = new ProductService();
            ///List<string>=null

            service.SetNull();


            return Json("setnull");
        }

        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <returns></returns>
        /// 

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            var httpRequest = HttpContext.Current.Request;
           // var postedFile = httpRequest.Files[file];
            if (httpRequest!=null)
            {

                // ImgstringList = new List<string>();
              //  var files = Request.files;
                var File = HttpContext.Current.Request.Files[0];
                //Imgurapi
                var apiClient = new ApiClient("0a9f2fb7434821b", "60f9a494f1607de3b90582298fc88c8e29560199");
                var httpClient = new HttpClient();

               // var fileName = File.FileName;/*C:\Users\User\source\repos\mikeyeh40709\Xboox\Xboox\Assets\Image\Pics\*/
                // session["fileName"]=1
                //從這裡開始可以用imgur
                //  var path = Path.Combine(Server.MapPath("../Assets/Pics"), fileName);


                //   files[0].SaveAs(path);

                var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
                var imageUpload = await imageEndpoint.UploadImageAsync(File.InputStream);

                ProductService.GetImg().Add(imageUpload.Link);


            }
            //;

            //return Json($"fileUpload {Session["fileName"]}");

            return Json(ProductService.GetImg()) ;
        }




        //[HttpPost]
        //public ActionResult Remove(string ccc)
        //{
        //    //var session = httpContext.Session;
        //    List<Object> abc = new List<Object>();

        //    abc.Add(Request.Form["ccc"]);
        //    //abc.Add(Request.Files);
        //  //  abc.Add(JsonConvert.DeserializeObject<string>(Request.Form["ccc"]));
        //    abc.Add(Request.Params["abc"]);
        //    //abc.Add(Request.Params["QUERY_STRING"]);



        //    //abc.Add(Request.);
        //    //abc.Add(Request.);
        //    var files = Request.Files;


        //    //從file移除資料
        //    //接Id 然後再抓Id
        //    //再處理字串(陣列) 在裡面找該圖的filename然後將其從字串移除
        //    //  ImgstringList
        //    //string json = JsonConvert.SerializeObject(files);
        //    return Json(abc);
        //}
    }

}
