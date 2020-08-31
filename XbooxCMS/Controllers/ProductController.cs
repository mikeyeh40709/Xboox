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

namespace XbooxCMS.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private XbooxContext context;


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
            foreach(var item in productList)
            {
                result.Add(new ProductListViewModel()
                {
                   // ProductId = Guid.NewGuid(),
                    Name = item.Name,
                    Quantity = item.UnitInStock,
                    Price = item.Price,
                    Author = item.Author,
                  //  PublishedDate = item.PublishedDate,
                    
                    
                    //欄位
                });
                
            }
            List<ProductListViewModel> products = new List<ProductListViewModel>
            {
               new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 236, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 236, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 236, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = Guid.NewGuid() ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"},
               new ProductListViewModel{ProductId = productId ,Name = "人間失格",Quantity = 200, Price = 150,Author = "太宰治",PublishedDate = "11-15-2019"}
            };
            return View(products);
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
            List<ProductImgs> productImgs = new List<ProductImgs>();
            HttpFileCollectionBase files = Request.Files;
            var selectedTags = GetAllTags().Where(t => createViewModel.PostedTagIds.Contains(t.TagId.ToString()));
    

            createViewModel.Tags = GetAllTags();
            createViewModel.SelectedTags = selectedTags;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
        
            AddedTag(createViewModel.Products, createViewModel.PostedTagIds);
            
         
           foreach(var i in ViewBag.List)
            {
                productImgs.Add(new ProductImgs() { imgLink = i, ProductId = createViewModel.Products.ProductId });
            }
            //將session name取出來 放進productImg
            //將session name取出來 然後一整串放進product


            //foreach (HttpPostedFileBase file in files)
            //{
            //    if (file != null && files.Count > 0)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        var path = Path.Combine(Server.MapPath("~Assets/Pics"), fileName);
            //        //要設定傳入product或 productId
            //        //要把Imglink改成可以自動生成id
            //        productImgs.Add(new ProductImgs() { imgLink = path, ProductId = createViewModel.Products.ProductId });
            //        file.SaveAs(path);

            //    }
            //}

            createViewModel.Products.ProductId = Guid.NewGuid();
    
            PutImgs(createViewModel.Products);
              //  try { 
             context.Product.Add(createViewModel.Products);
  
              //  }
               // catch (DbEntityValidationException ex)
               // {
             //       var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
              //      var getFullMessage = string.Join("; ", entityError);
              //      var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
               // }


            context.SaveChanges();
            return RedirectToAction("Create","Product");
        }

       private void AddedTag(Product product, List<string> SelectedTags)
        {
            List<ProductTags> productTags = new List<ProductTags>();
            if (SelectedTags == null)
            {
                return;
            }

            {
 
                foreach (var t in SelectedTags)
                {

                   // if (productTags.Select(x => x.ProductId).Contains(product.ProductId) && productTags.Select(x => x.TagId).Contains(new Guid(t)))
                        productTags.Add(new ProductTags() {ProductId = product.ProductId,TagId = new Guid(t) ,Id= Guid.NewGuid() });
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
        }
    

        /// <summary>
        /// 編輯產品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var product = context.Product.Find(id);
            if (product != null)
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


        [HttpPost]
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
        public ActionResult Delete()
        {
            return View();
        }


        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>

        //把imgid以字串[1,2,3...]的方式加進Product表格
        private void PutImgs(Product product)
        {
            List<ProductImgs> productImgs = new List<ProductImgs>();
            var imgs = productImgs.Where(x => x.ProductId == product.ProductId);
            foreach (var i in imgs)
            {
                product.ProductImgId = i.ProductImgId + ",";
                //product.ProductImgId = Convert.ToInt64(product.ProductImgId) + ",";
            }
            // context.SaveChanges();
        }




        [HttpPost]
        public ActionResult Upload( )
        {
            var sessiontest = "";
            //var Files = Request.Files;
            List<ProductImgs> productImgs = new List<ProductImgs>();
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
                List<string> fileNameCollection = new List<string>();
                if (Request.Files.Count > 0)
                { }
                for (var i = 0; i < files.Count; i++)
                {
                    if (files[i] != null && files[i].ContentLength != 0)
                    {
                        var fileName = files[i].FileName;
                        var path = Path.Combine(Server.MapPath("../Assets/Pics"), fileName);
                        fileNameCollection.Add(fileName);
                        //productImgs.Add(new ProductImgs() { imgLink = path, ProductId = createViewModel.Products.ProductId });
                        files[i].SaveAs(path);
                    }
                }
                ViewBag.List = fileNameCollection;
            }
            //;
            return Json(sessiontest);
            //foreach(var file in files)
            //  {
            //      if(file !=null && file.ContentLength > 0)
            //      {
            //          var fileName = Path.GetFileName(file.FileName);
            //          var path = Path.Combine(Server.MapPath("~Assets/Pics"),fileName);
            //          //要設定傳入product或 productId
            //          //要把Imglink改成可以自動生成id
            //          productImgs.Add(new ProductImgs() { imgLink = path, ProductId = product.ProductId });
            //          file.SaveAs(path);

            //      }
            //  }

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
                return RedirectToAction("Index","Home");
            }
            else
            {
                Debug.WriteLine("新增失敗");
                    return View();
                
            }

          
        }
    }
}
