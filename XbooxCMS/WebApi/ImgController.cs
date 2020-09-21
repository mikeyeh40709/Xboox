using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using XbooxLibrary.Services;

namespace XbooxCMS.WebApi
{
    [System.Web.Http.RoutePrefix("api/{controller}/{action}")]
    public class ImgController : ApiController
    {
        [System.Web.Http.HttpPost]
        public IHttpActionResult DeleteImg(string query)
        {
            var service = new ProductService();
            service.RemoveImg(query);
            return Json("Deleteok");
        }

        //    [System.Web.Http.HttpPost]
        //    public async Task<ActionResult> Upload()
        //    {


        //        if (Request.Files.Count > 0)
        //        {

        //            // ImgstringList = new List<string>();
        //            var files = Request.files;
        //            var File = Request.Files[0];
        //            //Imgurapi
        //            var apiClient = new ApiClient("0a9f2fb7434821b", "60f9a494f1607de3b90582298fc88c8e29560199");
        //            var httpClient = new HttpClient();

        //            var fileName = files[0].FileName;/*C:\Users\User\source\repos\mikeyeh40709\Xboox\Xboox\Assets\Image\Pics\*/
        //            // session["fileName"]=1
        //            //從這裡開始可以用imgur
        //            //  var path = Path.Combine(Server.MapPath("../Assets/Pics"), fileName);


        //            //   files[0].SaveAs(path);

        //            var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
        //            var imageUpload = await imageEndpoint.UploadImageAsync(File.InputStream);

        //            ProductService.GetImg().Add(imageUpload.Link);


        //        }
        //        //;

        //        //return Json($"fileUpload {Session["fileName"]}");

        //        //return Json("OK");
        //    }
        }

    }
