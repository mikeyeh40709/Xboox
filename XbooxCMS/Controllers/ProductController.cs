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

        Guid productId = Guid.NewGuid();
        Guid productId2 = Guid.NewGuid();

        /// <summary>
        /// 顯示產品資料列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var productList = context.Product.ToList();
            List<ProductListViewModel> result = new List<ProductListViewModel>();
            var list = from p in context.Product
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
                       };
                        
            foreach(var item in productList)
            {
                result.Add(new ProductListViewModel()
                {
                   // ProductId = Guid.NewGuid(),
                    Name = item.Name,
                    UnitInStock = item.UnitInStock,
                    Price = item.Price,
                    Author = item.Author,
                    PublishedDate = item.PublishedDate,
                    //CategorName = from i in item.CategoryId
                    //              join c in context.Category.ToList()
                    //              on item.CategoryId equals c.CategoryId select item.Name +
   

                    //欄位
                });
                
            }
         
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
                Tags = context.Tags.ToList(),
                SelectedTags = null,
             

            };
            context.SaveChanges();
           // context.SaveChanges();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel createViewModel)
        {
          
            var selectedTags = GetAllTags().Where(t => createViewModel.PostedTagIds.Contains(t.TagId.ToString()));
    

            createViewModel.Tags = GetAllTags();
            createViewModel.SelectedTags = selectedTags;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            createViewModel.Products.ProductId = Guid.NewGuid();
        
       
         
            //for (var i =0;i <fileNameList.Count();i++ )
            //{
            //    productImgs.Add(new ProductImgs() { imgLink = fileNameList[i], ProductId = createViewModel.Products.ProductId });
            //}
            //try
            //{
            //    foreach (var i in ViewBag.List)
            //    {
            //        productImgs.Add(new ProductImgs() { imgLink = i, ProductId = createViewModel.Products.ProductId });
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

       private void AddedTag(Product product, List<string> SelectedTags,ProductTags tags)
        {
           
            if (SelectedTags.Count==0)
            {
                return ;
            }

            {
 
                foreach (var t in SelectedTags)
                {
                    tags = new ProductTags()
                    {
                        ProductId = product.ProductId,
                        TagId = new Guid(t),
                        Id = Guid.NewGuid()
                    };

                    // if (productTags.Select(x => x.ProductId).Contains(product.ProductId) && productTags.Select(x => x.TagId).Contains(new Guid(t)))
                    //productsTags.Add(new ProductTags() {ProductId = product.ProductId,TagId = new Guid(t) ,Id= Guid.NewGuid() });
                }
                //var currentTagIds = context.Product.;

                //foreach (var role in GetAllTags())
                //{
                //    if (SelectedTags.Contains(tags.TagId.ToString())
                //    {
                //        // 此role有被勾選到
                //        if (!currentRoleIds.Contains(role.RoleId))
                //        {
                //            // 如果原本member沒有這個角色 就要增加
                //            user.Roles.Add(role);
                //        }
                //    }
                //    else
                //    {
                //        // 此role沒有被勾選到
                //        if (currentRoleIds.Contains(role.RoleId))
                //        {
                //            // 如果原本member有這個角色 就要移除
                //            user.Roles.Remove(role);
                //        }
                //    }
                //}
            }

            context.ProductTags.Add(tags);

            context.SaveChanges();
            // context.ProductTags.Add(tags);
          
           // context.SaveChanges();
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

                var viewModel = new CreateViewModel()
                {
                    
                    
                    Products = product,
                    Tags = context.Tags.ToList(),
                    Categories = context.Category.ToList(),
                   // SelectedTags = 
                };
              
                return View("Create", viewModel);
            }

            
        }


        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
           var  productInDb = context.Product.Single(c => c.ProductId == product.ProductId);
            productInDb.Name = product.Name;
            productInDb.Price = product.Price;
            productInDb.Description = product.Description;
            productInDb.ProductImgId = product.ProductImgId;
            productInDb.CategoryId = product.CategoryId;
            productInDb.Publisher = product.Publisher;
            productInDb.PublishedDate = product.PublishedDate;
            productInDb.UnitInStock = product.UnitInStock;
            productInDb.Specification = product.Specification;

            //待確定//
            productInDb.ProductTags = product.ProductTags;
            
            productInDb.Language = product.Language;
            productInDb.Intro = product.Intro;

            context.SaveChanges();
            return RedirectToAction("Index", "Product"); ;
        }



        public ActionResult Details()
        {
            return View();
        }


        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ComfirmDelete(Guid? id)
        {
            Product product = context.Product.SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
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
            var imgList = imgString.Split(',').Where(x=>x!="").ToList();
            





            foreach (var i in imgList)
            {
                 productImgs = new ProductImgs()
                {
                    // ProductImgId = 0,
                     imgLink = i,
                    ProductId = product.ProductId,
                };
                context.ProductImgs.Add(productImgs);
                //productImgs.Add(new ProductImgs() { imgLink = i.ToString(), ProductId = product.ProductId });
            }
       

           // context.SaveChanges();

          //  var PiList = new List<ProductImgs>();
           // var img = productImgs
        ///    var productImgs = productImgs.Where(x => x.ProductId == product.ProductId);
        //    freach (var i in productImgs)
        //   {
             //   product.ProductImgId = i.ProductImgId + ",";
                //product.ProductImgId = Convert.ToInt64(product.ProductImgId) + ",";
         //   }
             imgString = null;
             context.SaveChanges();
        }




        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> filepond)
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
                var files = Request.Files;

                           //if (file != null && file.ContentLength > 0)
                           //{

                           //    var path = Path.Combine(Server.MapPath("../Assets/Pics"), file.FileName);
                           //    //放進session
                           //    Session["fileName"] = Request.Files[0].FileName;

                           //    sessiontest = (string)Session[$"{Request.Files[0].FileName }"];
                           //   // productImgs.Add(new ProductImgs())
                           //    file.SaveAs(path);
                           //}

                           // if (Request.Files.Count > 0)
                           // { }
                           //  for (var i = 0; i < files.Count; i++)
                           // {
                           // if (files[i] != null && files[i].ContentLength != 0)
                           // {
                           var fileName = files[0].FileName;
                       
                        var path = Path.Combine(Server.MapPath("../Assets/Pics"), fileName);

                        //Session[$"fileName{i}"] = fileName;
                        //productImgs.Add(new ProductImgs() { imgLink = path, ProductId = createViewModel.Products.ProductId });
                        files[0].SaveAs(path);
                // }
                // }

                //if (Session["fileName"] == null)
                //{
                //    Session["fileName"] = fileName;
                //    Debug.WriteLine("getsssion");
                //}
                //else
                //{
                //    Session["fileName"] = Session["fileName"] + "," +fileName;
                //    Debug.WriteLine("getsssion2");
                //}



                imgString = getImg()  + fileName + ",";

                // List<string> fileNameList = (List<string>)Session["fileName"];
                //  fileNameList.Add(fileName);
                //  Session["fileName"] = fileNameList;

                //ViewBag.List = fileNameCollection;
            }
            //;
            //return Json($"fileUpload {Session["fileName"]}");
            return Json($"fileUpload {imgString}");

            //if (Request.Files.Count > 0)
            //{ }
            //for (var i = 0; i < files.Count; i++)
            //{
            //    if (files[i] != null && files[i].ContentLength != 0)
            //    {
            //        var fileName = files[i].FileName;
            //        var path = Path.Combine(Server.MapPath("~Assets/Pics"), fileName);
            //        productImgs.Add(new ProductImgs() { imgLink = path, ProductId = createViewModel.Products.ProductId });
            //        files[i].SaveAs(path);
            //    }
            //}

            //檔案上傳
            //處理路徑
            //ProductImg table .add(link)
            //加上productid
            //product欄位加上id字串(string join)

            // return RedirectToAction("Upload");
            //return View();
        }

     
        [HttpDelete]
        public ActionResult Upload(string imgId)
        {
            return View();
        }



        /// <summary>
        /// 創造標籤
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTag()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTag(Tags tags)
        {

            if (ModelState.IsValid)
            {
                //var viewModel = new TagViewModel()
                //{
                //    TagId = Guid.NewGuid(),
                //    TagName 
                //}

                tags.TagId = Guid.NewGuid();
                context.Tags.Add(tags);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {

                return View();

            }


        }
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

    }
}
