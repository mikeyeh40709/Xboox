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

        public ActionResult Create()
        {
            List<TagViewModel> tags = new List<TagViewModel>
            {
                new TagViewModel{TagName = "奇幻"},
                new TagViewModel{TagName = "愛情"},
                new TagViewModel{TagName = "小說"},
                new TagViewModel{TagName = "歷史"},
                new TagViewModel{TagName = "財經"},

            };


            //var productsCategory = context.Category.ToList();
            //var Tag = context.Tags.ToList();
            var viewModel = new CreateViewModel()
            {
                Products = new Product(),
                Categories = context.Category.ToList(),
                Tags = context.Tags.ToList()
                
            };

            //var tag = db.ProductTags.ToList();
            // var products = context.Product.Include(p => p.CategoryId);

            //List<Product> products = new List<Product>()
            //{

            //};
    

            context.SaveChanges();
            return View(viewModel);
        }



        public ActionResult Edit()
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


        public ActionResult Upload()
        {
            return View();
        }


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