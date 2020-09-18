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
using System.Web.UI;
using Xboox.Models.Services;
using System.Data.Entity.Core.Metadata.Edm;
using System.Web.WebSockets;
using Microsoft.Ajax.Utilities;
using System.Diagnostics;
using ECPay.Payment.Integration;
using XbooxLibrary.Repository;
using XbooxLibrary.Models.DataTable;

namespace Xboox.Services
{
    public class OrderService
    {
        public static Dictionary<string, string> payment = new Dictionary<string, string>
        {
            { "DTO" , "宅配到府"},
            { "Credit" , PaymentMethod.Credit.ToString() },
            { "BarCode" , PaymentMethod.BARCODE.ToString() },
            { "CVS" , PaymentMethod.CVS.ToString()}
        };
        public List<OrderViewModel> GetOrder(string id)
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderRepo = new GeneralRepository<Order>(dbContext);
                var userRepo = new GeneralRepository<AspNetUsers>(dbContext);
                var orderList = (from o in orderRepo.GetAll()
                                 where o.UserId == id
                                 join user in userRepo.GetAll()
                                 on o.UserId equals user.Id
                                 select new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = o.OrderDate,
                                     UserName = user.UserName,
                                     PurchaserName = o.PurchaserName,
                                     PurchaserEmail = o.PurchaserEmail,
                                     PurchaserAddress = o.City + o.District + o.Road,
                                     PurchaserPhone = o.PurchaserPhone,
                                     Payment = o.Payment,
                                     Paid = o.Paid,
                                     Build = o.Build
                                 }).OrderBy(item => item.OrderDate).ToList();
                return orderList;
            }
        }
        public List<OrderViewModel> GetOrder()
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderRepo = new GeneralRepository<Order>(dbContext);
                var userRepo = new GeneralRepository<AspNetUsers>(dbContext);
                var orderList = (from o in orderRepo.GetAll()
                                 join user in userRepo.GetAll()
                                 on o.UserId equals user.Id
                                 select new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = o.OrderDate,
                                     UserName = user.UserName,
                                     PurchaserName = o.PurchaserName,
                                     PurchaserEmail = o.PurchaserEmail,
                                     PurchaserAddress = $"{o.City}{o.District}{o.Road}",
                                     PurchaserPhone = o.PurchaserPhone,
                                     Payment = o.Payment,
                                     Paid = o.Paid,
                                     Build = o.Build
                                 }).OrderBy(item => item.OrderDate).ToList();
                return orderList;
            }
        }
        public List<OrderDetailsViewModel> GetOrderDetails(string id)
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderDetailRepo = new GeneralRepository<OrderDetails>(dbContext);
                var productRepo = new GeneralRepository<Product>(dbContext);
                var imgRepo = new GeneralRepository<ProductImgs>(dbContext);
                List<OrderDetailsViewModel> orderDetailsList = new List<OrderDetailsViewModel>();
                // 因為多張圖片會重複產品
                var tempList = (from od in orderDetailRepo.GetAll()
                                where od.OrderId.ToString() == id
                                join pd in productRepo.GetAll()
                                on od.ProductId equals pd.ProductId
                                join pi in imgRepo.GetAll()
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
        public OperationResult CreateOrder(HttpContextBase httpcontext, OrderViewModel order, string ecpayNumber)
        {
            var watch = new Stopwatch();
            watch.Start();
            OperationResult operationResult = new OperationResult();
            var dbContext = new XbooxLibraryDBContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var orderRepo = new GeneralRepository<Order>(dbContext);
                    var orderDetailRepo = new GeneralRepository<OrderDetails>(dbContext);
                    var cartRepo = new GeneralRepository<Cart>(dbContext);
                    var cartItemRepo = new GeneralRepository<CartItems>(dbContext);
                    var productRepo = new GeneralRepository<Product>(dbContext);
                    var couponRepo = new GeneralRepository<Coupons>(dbContext);
                    Guid newOrderID = Guid.NewGuid();
                    var userId = httpcontext.User.Identity.GetUserId();
                    // 建立一筆新訂單
                    Order newOrder = new Order()
                    {
                        OrderId = newOrderID,
                        EcpayOrderNumber = ecpayNumber,
                        UserId = userId,
                        OrderDate = DateTime.Now,
                        PurchaserName = order.PurchaserName,
                        City = order.City,
                        District = order.District,
                        Road = order.Road,
                        PurchaserEmail = order.PurchaserEmail,
                        PurchaserPhone = order.PurchaserPhone,
                        Paid = false,
                        Payment = order.Payment,
                        Build = true,
                        Remember = order.Remember
                    };
                    orderRepo.Create(newOrder);
                    orderRepo.SaveContext();
                    // 先拿會員CartItems 裡資料
                    var cartItems = cartItemRepo.GetAll().Where(item => item.CartId.ToString() == userId).ToList();
                    var cart = cartRepo.GetFirst(item => item.CartId.ToString() == userId);
                    var Coupon = couponRepo.GetFirst(item => item.CouponName == order.Discount);
                    foreach (var item in cartItems)
                    {
                        var products = productRepo.GetAll().Where(pd => pd.ProductId == item.ProductId);
                        foreach (var p in products)
                        {
                            if (p.UnitInStock >= item.Quantity)
                            {
                                p.UnitInStock = p.UnitInStock - item.Quantity;
                                OrderDetails orderDetails = new OrderDetails()
                                {
                                    OrderId = newOrderID,
                                    ProductId = p.ProductId,
                                    Quantity = item.Quantity
                                };
                                orderDetailRepo.Create(orderDetails);
                                item.Quantity = 0;
                                Debug.WriteLine(dbContext.Entry(p).State);
                            }
                            else
                            {
                                break;
                            }
                        }
                        cartItemRepo.Delete(item);
                    }
                    cartRepo.Delete(cart);
                    orderRepo.SaveContext();
                    operationResult.isSuccessful = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
                watch.Stop();
                Debug.WriteLine(watch.ElapsedMilliseconds);
                return operationResult;
            }

        }
        public OperationResult EditPaidState(string ECPaidId)
        {
            OperationResult operationResult = new OperationResult();
            using (var dbContext = new XbooxLibraryDBContext())
            {
                try
                {
                    var orderRepo = new GeneralRepository<Order>(dbContext);
                    var order = orderRepo.GetFirst(x => x.EcpayOrderNumber.ToString() == ECPaidId);
                    if (order != null)
                    {
                        if (!order.Paid)
                        {
                            order.Paid = true;
                        }
                        else
                        {
                            order.Paid = false;
                        }
                        orderRepo.SaveContext();
                        operationResult.isSuccessful = true;
                    }
                }
                catch (Exception ex)
                {
                    operationResult.isSuccessful = false;
                    operationResult.exception = ex;
                }
                return operationResult;
            }

        }
        public OperationResult CancelOrder(string orderId)
        {
            OperationResult operationResult = new OperationResult();
            var dbContext = new XbooxLibraryDBContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var orderRepo = new GeneralRepository<Order>(dbContext);
                    var orderDetailRepo = new GeneralRepository<OrderDetails>(dbContext);
                    var productRepo = new GeneralRepository<Product>(dbContext);
                    var orderDetails = orderDetailRepo.GetAll().Where(item => item.OrderId.ToString() == orderId);
                    var order = orderRepo.GetFirst(item => item.OrderId.ToString() == orderId);
                    if (order != null && orderDetails != null)
                    {
                        if (order.Paid == false)
                        {
                            order.Build = false;
                            foreach (var item in orderDetails)
                            {
                                var products = productRepo.GetAll().Where(pd => pd.ProductId == item.ProductId).OrderBy(pd => pd.PublishedDate);
                                foreach (var pd in products)
                                {
                                    if (item.Quantity <= 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        pd.UnitInStock = pd.UnitInStock + item.Quantity;
                                    }
                                }
                            }
                            productRepo.SaveContext();
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