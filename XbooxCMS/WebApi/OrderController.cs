using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XbooxCMS.ViewModels;
using XbooxCMS.Services;

namespace XbooxCMS.WebApi
{
    [RoutePrefix("api/[Controller]/[Action]")]
    public class OrderController : ApiController
    {

       //所有訂單
        [HttpGet]
        public List<OrderViewModel> GetAllOrders()
        {
            var service = new OrderService();
            var orderList = service.GetAllOrders();
            return orderList;
        }

        //訂單產品內容
        [HttpGet]
     
        public List<OrderDetailsViewModel> GetOrderDeatils(string id)
        {
            var service = new OrderService();
            var result = service.GetOrderDeatils(id);
            return result;
        }

        //取消訂單
        [HttpPost]
        public IHttpActionResult CancelOrder(string id)
        {
            var service = new OrderService();
            var result = service.CancelOrder(id);
            return Json("ok");
        }
    }
}

