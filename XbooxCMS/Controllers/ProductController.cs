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

namespace XbooxCMS.Controllers
{
    public class ProductController : Controller
    {
        
        // GET: Product
        private XbooxContext context;
        //private static string GetImg(HttpSessionStateBase httpContext)
        //{
        //   //HttpSessionState session = HttpContext.Current.Session;
        //    var session = httpContext.SessionID;
         
        //    if (session["fileName"] == null)
        //    {
        //        session["fileName"] = "";
        //    }
            
        //    return (string)session["fileName"];
        //}

        private static string imgString = null;
        private static List<string> ImgstringList = null;
       
        private static string getImg()
        {


            if (imgString == null)
            {
                //lock (padlock) //lock此區段程式碼，讓其它thread無法進入。
                //{
                //    if (abc == null)
                //    {
                //        abc = "";
                //    }
                //}
                imgString = "";
            }
            return imgString;
        }

        private static List<string> GetImg()
        {
            if (ImgstringList == null)
            {
                ImgstringList = new List<string>();
            }
            return ImgstringList;
        }

        public IEnumerable<Tags> GetAllTags()
        {
            return context.Tags;
        }
        public ProductController()
        {

            if (context == null)
            {
                context = new XbooxContext();
            }
        
        }


        /// <summary>
        /// 顯示產品資料列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
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
                        

        
            return View(list);
        }

        /// <summary>
        /// 建立產品資料 包含分類 標籤 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
       
            //var productsCategory = context.Category.ToList();
            //var Tag = context.Tags.ToList();
            var viewModel = new CreateViewModel()
            {
                Products = new Product(),
                Categories = context.Category.ToList(),
              //  Tags = context.Tags.ToList(),
                SelectedTags = null,
                
                Tags = GetAllTags().ToList()
            };
            context.SaveChanges();
           // context.SaveChanges();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel createViewModel)
        {
          
            var selectedTags = GetAllTags().Where(t => createViewModel.PostedTagIds.Contains(t.TagId));
    

            createViewModel.Tags = GetAllTags();
          //  ViewModel.SelectedTags = selectedTags;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            createViewModel.Products.ProductId = Guid.NewGuid();
        
       
         
            //for (var i =0;i <fileNameList.Count();i++ )
            //{
            //    productImgs.Add(new ProductImgs() { imgLink = fileNameList[i], ProductId = ViewModel.Products.ProductId });
            //}
            //try
            //{
            //    foreach (var i in ViewBag.List)
            //    {
            //        productImgs.Add(new ProductImgs() { imgLink = i, ProductId = ViewModel.Products.ProductId });
            //    }
            //}
  
            PutImgs(createViewModel.Products);
            
            context.Product.Add(createViewModel.Products);
          
            AddedTag(createViewModel.Products, createViewModel.PostedTagIds, createViewModel.ProductTags);
            

            context.SaveChanges(); 
            //catch (DbEntityValidationException ex)
            //{
            //    var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
            //    var getFullMessage = string.Join("; ", entityError);
            //    var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
            //}
            
            return RedirectToAction("Create","Product");
        }

        /// <summary>
        /// 將Tag加入ProductTag
        /// </summary>
        /// <param name="product"></param>
        /// <param name="SelectedTags"></param>
        /// <param name="tags"></param>
       private void AddedTag(Product product, List<Guid> SelectedTags,ProductTags tags)
        {
           
            if (SelectedTags==null)
            {
                return ;
            }

            {
                //原本被選的Tag
                var pTagList = context.ProductTags.Where(x => x.ProductId == product.ProductId).Select(x => (Guid)x.TagId).ToList();
                if (pTagList.Count == 0)
                {
                    //create
                    foreach (var t in SelectedTags)
                    {
                        tags = new ProductTags()
                        {
                            ProductId = product.ProductId,
                            TagId = t,
                            Id = Guid.NewGuid()
                        };

                        context.ProductTags.Add(tags);
                    }
                }

              
                //edit
                else
                {
                    //找出原本有選但現在沒選的Tag
                    var newTagList1 = pTagList.Except(SelectedTags);


                    //把現在沒選的Tag移除
                    foreach (var t in newTagList1)
                    {

                        var item = context.ProductTags.Where(x => x.TagId == t && x.ProductId == product.ProductId).FirstOrDefault();
                        context.ProductTags.Remove(item);
                    }

                    //找出現在有選但原本沒選的Tag
                    var newTagList2 = SelectedTags.Except(pTagList);

                    //加入沒選過的tag
                    foreach (var t in newTagList2)
                    {
                        tags = new ProductTags()
                        {
                            ProductId = product.ProductId,
                            TagId = t,
                            Id = Guid.NewGuid()
                        };
                        context.ProductTags.Add(tags);
                    }
                }
               

            }

        

            context.SaveChanges();
     
        }
    

        /// <summary>
        /// 編輯產品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var product = context.Product.FirstOrDefault(p => p.ProductId == id);
          
            if (product == null)
            {

                return HttpNotFound();

            }
            else
            {
                List<Tags> TagList = new List<Tags>(); 
                var viewModels = new CreateViewModel();
                viewModels.Products = product;
                viewModels.Tags = context.Tags.ToList();
                viewModels.Categories = context.Category.ToList();
         
                var temp = context.ProductTags.Where(x => x.ProductId == product.ProductId).Select(x => x.TagId).ToList();
                //找到本來被選中的tag
                foreach (var t in temp)
                {
                    
                   TagList.Add(new Tags() { TagId= (Guid)t,TagName = context.Tags.Where(x=>x.TagId==t).Select(x=>x.TagName).FirstOrDefault()});
                    Debug.WriteLine(t);
                };
                viewModels.SelectedTags = TagList;


                return View(viewModels);
            }

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Wrong");
                return RedirectToAction("Index", "Product");
            }
            var selectedTags = GetAllTags().Where(t => ViewModel.PostedTagIds.Contains(t.TagId));

            //reload
            ViewModel.Tags = GetAllTags();
           // ViewModel.SelectedTags = selectedTags;

            var  productInDb = context.Product.Single(c => c.ProductId == ViewModel.Products.ProductId);
            productInDb.Name = ViewModel.Products.Name;
            productInDb.Price = ViewModel.Products.Price;
            productInDb.Description = ViewModel.Products.Description;
            productInDb.CategoryId = ViewModel.Products.CategoryId;
            productInDb.Publisher = ViewModel.Products.Publisher;
            productInDb.PublishedDate = ViewModel.Products.PublishedDate;
            productInDb.UnitInStock = ViewModel.Products.UnitInStock;
            productInDb.Specification = ViewModel.Products.Specification;


            //  productInDb.ProductImgId = ViewModel.Products.ProductImgId;
            PutImgs(ViewModel.Products);
            AddedTag(ViewModel.Products, ViewModel.PostedTagIds, ViewModel.ProductTags);
            //productInDb.ProductTags = ViewModel.Products.ProductTags;
            
            productInDb.Language = ViewModel.Products.Language;
            productInDb.Intro = ViewModel.Products.Intro;
     
            context.SaveChanges();
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
            Product product = context.Product.SingleOrDefault(p => p.ProductId == id);
            var tags = context.ProductTags.Where(p => p.ProductId == id);
            var imgs = context.ProductImgs.Where(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (imgs != null)
                {
                    foreach (var i in imgs)
                    {
                        context.ProductImgs.Remove(i);
                    }
                }
                if (tags != null)
                {
                    foreach (var i in tags)
                    {
                        context.ProductTags.Remove(i);
                    }
                }
                //刪除product 刪除 tag
                
                context.Product.Remove(product);
                context.SaveChanges();
                return RedirectToAction("Index","Product");
            }
           
        }


        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>

        //把imgid以字串[1,2,3...]的方式加進Product表格
        private void PutImgs(Product product)
        {
           var productImgs = new ProductImgs();
     //   var imgList = getImg().Split(',').Where(x=>x!="").ToList();
            

            //用list傳的
            foreach(var i in GetImg())
            {
                productImgs = new ProductImgs()
                {
                    // ProductImgId = 0,
                    imgLink = i,
                    ProductId = product.ProductId,
                };
                context.ProductImgs.Add(productImgs);
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
             context.SaveChanges();
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
             
             //從這裡開始可以用imgur
           //  var path = Path.Combine(Server.MapPath("../Assets/Pics"), fileName);


          //   files[0].SaveAs(path);
 
                var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
                var imageUpload = await imageEndpoint.UploadImageAsync(File.InputStream);
                GetImg().Add(imageUpload.Link);


                //ViewBag.List = fileNameCollection;
            }
            //;
          
            //return Json($"fileUpload {Session["fileName"]}");
            
            return Json(GetImg());


      
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



        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

    }
}
