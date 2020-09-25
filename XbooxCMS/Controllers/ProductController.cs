using System;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;
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


    }
}
