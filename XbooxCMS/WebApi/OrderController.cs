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

       
        [HttpGet]
        public List<OrderViewModel> GetAllOrders()
        {
            var service = new OrderService();
            var orderList = service.GetAllOrders();
            return orderList;
        }


        [HttpGet]
     
        public List<OrderDetailsViewModel> GetOrderDeatils(string id)
        {
            var service = new OrderService();
            var result = service.GetOrderDeatils(id);
            return result;
        }
    }
}

