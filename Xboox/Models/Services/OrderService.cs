using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Web;
using System.Net;
using Microsoft.AspNet.Identity;
using Xboox.Models;
using System.Data.Entity;
using System.Web.Mvc;
using Xboox.ViewModels;
using Xboox.Models.DataTable;
using System.Web.UI;
using Xboox.Models.Services;
using System.Data.Entity.Core.Metadata.Edm;
using System.Web.WebSockets;
using Microsoft.Ajax.Utilities;
using System.Diagnostics;

namespace Xboox.Services
{
    public class OrderService
    {
        private enum payment
        {
            Unpaid = 0,
            Paid = 1
        }
        public List<OrderViewModel> GetOrder(string id)
        {
            using (var context = new XbooxContext())
            {
                var orderList = (from o in context.Order
                                 where o.UserId == id
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
                return orderList;
            }

        }
        public List<OrderViewModel> GetOrder()
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
                return orderList;
            }
        }
        public List<OrderDetailsViewModel> GetOrderDetails(string id)
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
                return orderDetailsList;
            }

        }
        public OperationResult CreateOrder(HttpContextBase httpcontext, Order order)
        {
            OperationResult operationResult = new OperationResult();
            XbooxContext context = new XbooxContext();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Guid newOrderID = Guid.NewGuid();
                    var userId = httpcontext.User.Identity.GetUserId();
                    // 建立一筆新訂單
                    Order newOrder = new Order()
                    {
                        OrderId = newOrderID,
                        UserId = userId,
                        OrderDate = DateTime.Now,
                        PurchaserName = order.PurchaserName,
                        PurchaserAddress = order.PurchaserAddress,
                        PurchaserEmail = order.PurchaserEmail,
                        PurchaserPhone = order.PurchaserPhone,
                        StateId = order.StateId
                    };
                    context.Order.Add(newOrder);
                    context.SaveChanges();
                    // 先拿會員CartItems 裡資料
                    var cartItems = context.CartItems.Where(item => item.CartId.ToString() == userId).ToList();
                    var cart = context.Cart.FirstOrDefault(item => item.CartId.ToString() == userId);
                    foreach (var item in cartItems)
                    {
                        var products = context.Product.Where(pd => pd.ProductId == item.ProductId);
                        foreach (var p in products)
                        {
                            if (p.UnitInStock >= item.Quantity)
                            {
                                p.UnitInStock = p.UnitInStock - item.Quantity;
                                OrderDetails orderDetails = new OrderDetails()
                                {
                                    OrderId = newOrderID,
                                    ProductId = p.ProductId,
                                    ProductName = p.Name,
                                    UnitPrice = p.Price,
                                    Quantity = item.Quantity
                                };
                                context.OrderDetails.Add(orderDetails);
                                item.Quantity = 0;
                                Debug.WriteLine(context.Entry(p).State);
                            }
                            else
                            {
                                break;
                            }
                        }
                        context.CartItems.Remove(item);
                    }
                    context.Cart.Remove(cart);
                    context.SaveChanges();
                    operationResult.isSuccessful = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
                return operationResult;
            }

        }
        public OperationResult EditState(string id)
        {
            OperationResult operationResult = new OperationResult();
            XbooxContext context = new XbooxContext();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var order = context.Order.FirstOrDefault(x => x.OrderId.ToString() == id);
                    if (order != null)
                    {
                        if (order.StateId == (int)payment.Unpaid)
                        {
                            order.StateId = (int)payment.Paid;
                        }
                        else
                        {
                            order.StateId = (int)payment.Unpaid;
                        }
                        context.SaveChanges();
                        operationResult.isSuccessful = true;
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
                return operationResult;
            }

        }
        public OperationResult Delete(string id)
        {
            OperationResult operationResult = new OperationResult();
            XbooxContext context = new XbooxContext();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var OrderDetails = context.OrderDetails.Where(item => item.OrderId.ToString() == id);
                    var order = context.Order.FirstOrDefault(item => item.OrderId.ToString() == id);
                    if (order != null && OrderDetails != null)
                    {
                        if (order.StateId == (int)payment.Unpaid)
                        {
                            foreach (var item in OrderDetails)
                            {
                                var products = context.Product.Where(pd => pd.ProductId == item.ProductId).OrderBy(pd => pd.PublishedDate);
                                foreach (var pd in products)
                                {
                                    if (item.Quantity <= 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        pd.UnitInStock = pd.UnitInStock - item.Quantity;
                                        item.Quantity = 0;
                                    }
                                }
                                context.OrderDetails.Remove(item);
                            }
                            context.Order.Remove(order);
                            context.SaveChanges();
                            operationResult.isSuccessful = true;
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
            }
            return operationResult;
        }
    }
}