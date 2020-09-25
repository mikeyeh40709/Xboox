using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using XbooxCMS.Models;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxCMS.Services;

namespace XbooxCMS.Controllers
{
    public class OrderController : Controller
    {

        // GET: Order
        public ActionResult Index()
        {

            var service = new OrderService();
            var orderList = service.GetAllOrders();
           
            return View();
        }

          
        }
    
    
    
    }


   
