using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;
using System.Data.Entity;
using System.Diagnostics;

namespace XbooxCMS.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private XbooxCMSContext context;

        public ProductController()
        {
            if (context == null)
            {
                context = new XbooxCMSContext();
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
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Author = item.Author,
                    PublishedDate = item.PublishedDate,
                    
                    
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
                Tags = context.Tags.ToList()
                
            };

           // context.SaveChanges();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
               // var products = from p in product.Name join c in context.Category.ToList() on p equals c.CategoryId select new  ;
                product.ProductId = Guid.NewGuid();

                var viewModel = new CreateViewModel
                {
                    
                    Products = product,
                    Categories = context.Category.ToList(),
                    Tags = context.Tags.ToList()
                };
               
            }
            context.Product.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index","Product");
        }




        public ActionResult Edit(Guid id)
        {
            var product = context.Product.SingleOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                return HttpNotFound();

            }
            else
            {

            }

            return View();
        }


        [HttpPost]
        public ActionResult Edit(Product product)
        {

            return View();
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

        public ActionResult Upload(HttpContextBase file)
        {
            if (file != null)
            {

            }
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
                return View();
            }
            else
            {
                Debug.WriteLine("新增失敗");
                    return View();
                
            }

          
        }
    }
}