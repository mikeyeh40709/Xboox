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
        
        // GET: Product
       // private XbooxContext context;
  
        //private static string GetImg(HttpSessionStateBase httpContext)
        //{
        //    HttpSessionState session = HttpContext.Current.Session;
        //    //var session = httpContext.SessionID;

        //    if (session["fileName"] == null)
        //    {
        //        session["fileName"] = "";
        //    }

        //    return (string)session["fileName"];
        //}

        //private static List<string> ImgstringList = null;
        //private static void LoadTempData(this ControllerBase controller)
        //{
        //    controller.TempData["ic"] = 5;
        //}


        //private static List<string> GetImg()
        //{
        //    if (ImgstringList == null)
        //    {
        //        ImgstringList = new List<string>();
        //    }
        //    return ImgstringList;
        //}


        public ProductController()
        {

            //if (context == null)
            //{
            //    context = new XbooxContext();
            //}
        
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


        public ActionResult Details(Guid id)
        {


            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateDataModel createDataModel)
        {

           
            var service = new ProductService();

            if (!ModelState.IsValid)
            {
                //建立失敗
                return RedirectToAction("Index", "Home");
            }


            service.Create(createDataModel);


            return RedirectToAction("Index","Product");
        }

        /// <summary>
        /// 將Tag加入ProductTag
        /// </summary>
        /// <param name="product"></param>
        /// <param name="SelectedTags"></param>
        /// <param name="tags"></param>
       //private void AddedTag(Product product, List<Guid> SelectedTags,ProductTags tags)
       // {
           
       //     if (SelectedTags==null)
       //     {
       //         return ;
       //     }

       //     {
       //         //原本被選的Tag
       //         var pTagList = context.ProductTags.Where(x => x.ProductId == product.ProductId).Select(x => (Guid)x.TagId).ToList();
       //         if (pTagList.Count == 0)
       //         {
       //             //create
       //             foreach (var t in SelectedTags)
       //             {
       //                 tags = new ProductTags()
       //                 {
       //                     ProductId = product.ProductId,
       //                     TagId = t,
       //                     Id = Guid.NewGuid()
       //                 };

       //                 context.ProductTags.Add(tags);
       //             }
       //         }

              
       //         //edit
       //         else
       //         {
       //             //找出原本有選但現在沒選的Tag
       //             var newTagList1 = pTagList.Except(SelectedTags);


       //             //把現在沒選的Tag移除
       //             foreach (var t in newTagList1)
       //             {

       //                 var item = context.ProductTags.Where(x => x.TagId == t && x.ProductId == product.ProductId).FirstOrDefault();
       //                 context.ProductTags.Remove(item);
       //             }

       //             //找出現在有選但原本沒選的Tag
       //             var newTagList2 = SelectedTags.Except(pTagList);

       //             //加入沒選過的tag
       //             foreach (var t in newTagList2)
       //             {
       //                 tags = new ProductTags()
       //                 {
       //                     ProductId = product.ProductId,
       //                     TagId = t,
       //                     Id = Guid.NewGuid()
       //                 };
       //                 context.ProductTags.Add(tags);
       //             }
       //         }
               

       //     }

        

       //     context.SaveChanges();
     
       // }
    

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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateDataModel DataModel)
        {

            var service = new ProductService();
            
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Wrong");
                return RedirectToAction("Index", "Product");
            }

            service.Edit(DataModel);

    
            return RedirectToAction("Index", "Product"); ;
        }



        public ActionResult Details()
        {

            return View();
        }

        
        //public ActionResult Delete()
        //{
        //    return RedirectToAction("Index", "Product");
        //}
        ////[ActionName("Delete")]
        //[HttpDelete]
        public ActionResult Delete(Guid id)
        {

            var service = new ProductService();
            service.Delete(id);
       
                return RedirectToAction("Index","Product");
            }
           
        


        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>

        //把imgid以字串[1,2,3...]的方式加進Product表格
      //  private void PutImgs(Product product)
      //  {
        //   var productImgs = new ProductImgs();
     //   var imgList = getImg().Split(',').Where(x=>x!="").ToList();
            

            //用list傳的
          //  foreach(var i in GetImg())
          //  {
               // productImgs = new ProductImgs()
              //  {
                  // ProductImgId = 0,
          //          imgLink = i,
          //          ProductId = product.ProductId,
          //      };
        //       context.ProductImgs.Add(productImgs);
      //      }



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
       //      ImgstringList = null;
         //    context.SaveChanges();
      //  }




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
                ProductService.GetImg().Add(imageUpload.Link);


                //ViewBag.List = fileNameCollection;
            }
            //;
          
            //return Json($"fileUpload {Session["fileName"]}");
            
            return Json (ProductService.GetImg());


      
        }

     
        [HttpPost]
        public ActionResult Remove(string ccc)
        {
            //var session = httpContext.Session;
            List<Object> abc = new List<Object>();
            
            abc.Add(Request.Form["ccc"]);
            //abc.Add(Request.Files);
          //  abc.Add(JsonConvert.DeserializeObject<string>(Request.Form["ccc"]));
            abc.Add(Request.Params["abc"]);
            //abc.Add(Request.Params["QUERY_STRING"]);
            
            
           
            //abc.Add(Request.);
            //abc.Add(Request.);
            var files = Request.Files;


            //從file移除資料
            //接Id 然後再抓Id
            //再處理字串(陣列) 在裡面找該圖的filename然後將其從字串移除
            //  ImgstringList
            //string json = JsonConvert.SerializeObject(files);
            return Json(abc);
        }


        [HttpPost]
        public ActionResult RemoveAll()
        {
            var service = new ProductService();
            ///List<string>=null

            service.SetNull();

            //abc.Add(Request.);
            //abc.Add(Request.);
          

            //從file移除資料
            //接Id 然後再抓Id
            //再處理字串(陣列) 在裡面找該圖的filename然後將其從字串移除
            //  ImgstringList
            //string json = JsonConvert.SerializeObject(files);
            return Json("setnull");
        }


        //protected override void Dispose(bool disposing)
        //{
        //    context.Dispose();
        //}

    }
}
