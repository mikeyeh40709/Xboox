﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Validation;

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
                    Quantity = item.Quantity,
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

            var selectedTags = GetAllTags().Where(t => createViewModel.PostedTagIds.Contains(t.TagId.ToString()));

            createViewModel.Tags = GetAllTags();
            createViewModel.SelectedTags = selectedTags;
   
            if (!ModelState.IsValid)
            {
                AddedTag(createViewModel.Products, createViewModel.PostedTagIds);

              //  product.ProductId = Guid.NewGuid();
                createViewModel.Products.ProductId = Guid.NewGuid();
                createViewModel.Products.ProductImgId = "1";
                
                //var viewModel = new CreateViewModel
                //{
                    
                    
                //    Products = product,
                //    Categories = context.Category.ToList(),
                //    Tags = context.Tags.ToList()
                    
                //};
                try { 
                    context.Product.Add(createViewModel.Products);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var getFullMessage = string.Join("; ", entityError);
                    var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                }

               
            }

           // List<ProductTags> productTags = new List<ProductTags>
           // {
              
                //new ProductTags{ProductId = product.ProductId ,TagId =tagId1 },
                //new ProductTags{ProductId = product.ProductId ,TagId =tagId2},
          

           // };
            //foreach(var item in productTags)
            //{
            //    context.ProductTags.Add(item);
            //}
        
            return RedirectToAction("Create","Product");
        }

       private void AddedTag(Product product, List<string> SelectedTags)
        {
            List<ProductTags> productTags = new List<ProductTags>();
            if (SelectedTags == null)
            {
                return;
            }
            //if (SelectedTags == null)
            //{
            //    tags.TagId.Clear();
            //    return;
            //}
        
            {
                
             
                foreach (var t in SelectedTags)
                {
                   // if (productTags.Select(x => x.ProductId).Contains(product.ProductId) && productTags.Select(x => x.TagId).Contains(new Guid(t)))
                        productTags.Add(new ProductTags() {ProductId = product.ProductId,TagId = new Guid(t) });
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
    


        public ActionResult Edit(Guid id)
        {
            var product = context.Product.Find(id);
            if (product != null)
            {

                return HttpNotFound();

            }
            else
            {
                //var productInDb = context.Product.ToList();
                //productInDb.
            }

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        public ActionResult Upload(HttpContextBase file)
        {
            if (file != null)
            {

            }
            else
            {
                //檔案上傳
                //處理路徑
                //ProductImg table .add(link)
                //加上productid
                //product欄位加上id字串(string join)
                
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
