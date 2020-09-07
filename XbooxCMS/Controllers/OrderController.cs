using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {


            using (var context = new XbooxContext())
            {
                var orderList = (from o in context.Order
                                 join user in context.AspNetUsers
                                 on o.UserId equals user.Id
                                 select new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = o.OrderDate,
                                     UserName = user.UserName,
                                     PurchaserName = o.PurchaserName,
                                     PurchaserEmail = o.PurchaserEmail,
                                     PurchaserAddress = o.PurchaserAddress,
                                     PurchaserPhone = o.PurchaserPhone,
                                     StateId = o.StateId
                                 }).OrderBy(item => item.OrderDate).ToList();

                return View(orderList);
            }

        }

   
        public ActionResult Detail(string id)
        {
            using (var context = new XbooxContext())
            {
                List<OrderDetailsViewModel> orderDetailsList = new List<OrderDetailsViewModel>();
                // 因為多張圖片會重複產品
                var tempList = (from od in context.OrderDetails
                                where od.OrderId.ToString() == id
                                join pd in context.Product
                                on od.ProductId equals pd.ProductId
                                join pi in context.ProductImgs
                                on pd.ProductId equals pi.ProductId
                                where pd.ProductId == pi.ProductId
                                select new OrderDetailsViewModel
                                {
                                    Imagelink = pi.imgLink,
                                    ProductName = pd.Name,
                                    Quantity = od.Quantity,
                                    UnitPrice = pd.Price,
                                    Total = Math.Round(pd.Price * od.Quantity)
                                }).GroupBy(item => item.ProductName);
                foreach (var productList in tempList)
                {
                    var firstProductItem = productList.FirstOrDefault(item => !item.Imagelink.Contains("-0"));
                    orderDetailsList.Add(firstProductItem);
                }
                return Json(orderDetailsList,JsonRequestBehavior.AllowGet);
            }
          
        }
    
    
    
    }


   
}