using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.IO;
using System.Web.SessionState;
using Newtonsoft.Json;
using Imgur.API.Authentication;
using System.Net.Http;
using Imgur.API.Endpoints;
using static System.Net.WebRequestMethods;
using System.Threading.Tasks;
using XbooxLibrary.Services;

namespace XbooxCMS.Controllers
{
    public class ProductController : Controller
    {

        public ProductController()
        {

        
        }


        /// <summary>
        /// 顯示產品資料列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            var service = new ProductService();
            var pist = service.GetAllProducts();
     
            return View(pist);
        }


       
        /// <summary>
        /// 建立產品資料 包含分類 標籤 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var service = new ProductService();
          
            var viewModels = new CreateListViewModel()
            {
                Tags = service.GetTags(),
                CategoryViewModels = service.GetCatecory(),

            };


            return View(viewModels);
        }

    
    

        /// <summary>
        /// 編輯產品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {

            var service = new ProductService();
            var getproduct = service.GetSingleProduct(id);
    
          
            if (getproduct == null)
            {

                return HttpNotFound();

            }
            else
            {
                var viewModel = service.CreateEditList(getproduct);

                return View(viewModel);

            }

            
        }







        [HttpPost]
        public async Task<ActionResult>  Upload()
        {
            //foreach (var file in filepond)
           // {
               // if (file != null && file.ContentLength > 0)
                //{
                    //var fileName = Path.GetFileName(file.);
                  //  var path = Path.Combine(Server.MapPath("~Assets/Pics"), fileName);
                    //要設定傳入product或 productId
                    //要把Imglink改成可以自動生成id
                   // productImgs.Add(new ProductImgs() { imgLink = path, ProductId = product.ProductId });
                   // file.SaveAs(path);

                //}
           // }
            //var Files = Request.Files;

            if (Request.Files.Count > 0)
            {

               // ImgstringList = new List<string>();
                var files = Request.Files;
                var File = Request.Files[0];
                //Imgurapi
                var apiClient = new ApiClient("0a9f2fb7434821b", "60f9a494f1607de3b90582298fc88c8e29560199");
                var httpClient = new HttpClient();

                var fileName = files[0].FileName;/*C:\Users\User\source\repos\mikeyeh40709\Xboox\Xboox\Assets\Image\Pics\*/
               // session["fileName"]=1
             //從這裡開始可以用imgur
             //  var path = Path.Combine(Server.MapPath("../Assets/Pics"), fileName);


                //   files[0].SaveAs(path);

                var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
                var imageUpload = await imageEndpoint.UploadImageAsync(File.InputStream);
                //ImgstringLists.Add(imageUpload.Link);
                //TempData["imgString"] = ImgstringLists;
                ProductService.GetImg().Add(imageUpload.Link);


                //ViewBag.List = fileNameCollection;
            }
            //;
          
            //return Json($"fileUpload {Session["fileName"]}");
            
            return Json ("OK");


      
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


        [HttpPost]
        public ActionResult RemoveAll()
        {
            var service = new ProductService();
            ///List<string>=null

            service.SetNull();

 
            return Json("setnull");
        }


     

    }
}
