using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Xboox.Models;
using System.Data.Entity;
using System.Web.Mvc;
using Xboox.ViewModels;
using Xboox.Models.DataTable;

namespace Xboox.Services
{
    public class OrderService
    {
        private XbooxContext context = new XbooxContext();
        public List<OrderViewModel> GetOrder(string id)
        {
            var orderList = context.Order
                .Where(item => item.UserId == id)
                .Select(item => new OrderViewModel
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    UserName = item.UserId,
                    PurchaserName = item.PurchaserName,
                    PurchaserEmail = item.PurchaserEmail,
                    PurchaserAddress = item.PurchaserAddress,
                    PurchaserPhone = item.PurchaserPhone,
                    StateId = item.StateId
                })
                .OrderBy(item => item.OrderDate).ToList();
            return orderList;
        }
        public List<OrderViewModel> GetOrder()
        {
            var orderList = context.Order
               .Select(item => new OrderViewModel
               {
                   OrderId = item.OrderId,
                   OrderDate = item.OrderDate,
                   UserName = item.UserId,
                   PurchaserName = item.PurchaserName,
                   PurchaserEmail = item.PurchaserEmail,
                   PurchaserAddress = item.PurchaserAddress,
                   PurchaserPhone = item.PurchaserPhone,
                   StateId = item.StateId
               })
               .OrderBy(item => item.OrderDate).ToList();
            return orderList;
        }
        public List<OrderDetailsViewModel> GetOrderDetails(string id)
        {
            var orderDetailsList = context.OrderDetails
                .Where(item => item.OrderId.ToString() == id)
                .Join(context.Product, od => od.ProductId, pd => pd.ProductId, (od, pd) => new OrderDetailsViewModel
                {   
                    ProductName = pd.Name,
                    UnitPrice = pd.Price,
                    Quantity = od.Quantity
                }).ToList();
            return orderDetailsList;
        }
        public bool EditState(string id)
        {
            using(var db = new XbooxContext())
            {
                var order = db.Order.FirstOrDefault(x => x.OrderId.ToString() == id);
                if(order != null)
                {
                    if (order.StateId == 0)
                    {
                        order.StateId = 1;
                    }
                    else
                    {
                        order.StateId = 0;
                    }
                    db.SaveChanges();
                    return true;
                }else
                {
                    return false;
                }
            }
        }
        public bool Delete(string id)
        {
            using (var db = new XbooxContext())
            {
                var order = db.Order.FirstOrDefault(x => x.OrderId.ToString() == id);
                if(order != null)
                {
                    db.Order.Remove(order);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}