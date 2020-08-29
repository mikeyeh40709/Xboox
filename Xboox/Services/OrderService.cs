using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Xboox.Models;
using System.Data.Entity;
using Xboox.Models.ViewModels;
using System.Web.Mvc;
using Xboox.ViewModels;

namespace Xboox.Services
{
    public class OrderService
    {
        private XbooxContext context = new XbooxContext();
        public List<OrderViewModel> GetOrder(string id)
        {
            var order = context.Order.ToList();
            var orderDetails = context.OrderDetails.ToList();
            List<OrderViewModel> orderList = new List<OrderViewModel>();
            List<OrderDetailsViewModel> orderDetailsList = new List<OrderDetailsViewModel>();
            var orderDetailsQuery = (from o in order
                                     join od in orderDetails
                                     on o.OrderId equals od.OrderId
                                     select new OrderDetailsViewModel
                                     {
                   
                                         ProductName = od.ProductName,
                                         Quantity = od.Quantity,
                                         UnitPrice = od.UnitPrice,
                                         Total = od.UnitPrice * Convert.ToDecimal(od.Quantity)
                                     }).ToList();
            var orderquery = (from o in order
                              join odlist in
                              (from o in order
                               join od in orderDetails
                               on o.OrderId equals od.OrderId
                               select od
                               )
                              on o.OrderId equals odlist.OrderId
                              where o.UserId == id
                              select new OrderViewModel()
                              {
                                  OrderId = o.OrderId,
                                  UserName = o.UserId,
                                  OrderDate = o.OrderDate,
                                  PurchaserName = o.PurchaserName,
                                  PurchaserAddress = o.PurchaserAddress,
                                  PurchaserEmail = o.PurchaserEmail,
                                  PurchaserPhone = o.PurchaserPhone,
                                  OrderDetailsList = orderDetailsQuery
                              }
                         ).ToList();
            foreach(var item in orderquery)
            {
                orderList.Add(item);
            }
            return orderList;
        }
        public List<OrderViewModel> GetOrder()
        {
            var order = context.Order.ToList();
            var orderDetails = context.OrderDetails.ToList();
            List<OrderViewModel> orderList = new List<OrderViewModel>();
            List<OrderDetailsViewModel> orderDetailsList = new List<OrderDetailsViewModel>();
            var orderDetailsQuery = (from o in order
                                     join od in orderDetails
                                     on o.OrderId equals od.OrderId
                                     select new OrderDetailsViewModel
                                     {

                                         ProductName = od.ProductName,
                                         Quantity = od.Quantity,
                                         UnitPrice = od.UnitPrice,
                                         Total = od.UnitPrice * Convert.ToDecimal(od.Quantity)
                                     }).ToList();
            var orderquery = (from o in order
                              join odlist in
                              (from o in order
                               join od in orderDetails
                               on o.OrderId equals od.OrderId
                               select od
                               )
                              on o.OrderId equals odlist.OrderId
                              select new OrderViewModel()
                              {
                                  OrderId = o.OrderId,
                                  UserName = o.UserId,
                                  OrderDate = o.OrderDate,
                                  PurchaserName = o.PurchaserName,
                                  PurchaserAddress = o.PurchaserAddress,
                                  PurchaserEmail = o.PurchaserEmail,
                                  PurchaserPhone = o.PurchaserPhone,
                                  OrderDetailsList = orderDetailsQuery
                              }
                         ).ToList();
            foreach (var item in orderquery)
            {
                orderList.Add(item);
            }
            return orderList;
        }
        //public Guid CreateOrder(string userId ,Order order)
        //{
        //    Order newOrder = new Order() 
        //    { 
        //     };

        //}
    }
}