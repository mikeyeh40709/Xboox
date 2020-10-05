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
        /// <summary>
        /// 取得付款類別
        /// </summary>
        public static Dictionary<string, string> payment = new Dictionary<string, string>
        {
            { "DTO" , "宅配到府"},
            { "Credit" , PaymentMethod.Credit.ToString() },
            { "BarCode" , PaymentMethod.BARCODE.ToString() },
            { "CVS" , PaymentMethod.CVS.ToString()}
        };
        #region 使用UserID取所有訂單
        /// <summary>
        /// 利用UserId取訂單
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<OrderViewModel> GetOrder(string userId)
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderRepo = new GeneralRepository<Order>(dbContext);
                var userRepo = new GeneralRepository<AspNetUsers>(dbContext);
                var orderList = (from o in orderRepo.GetAll().AsEnumerable()
                                 where o.UserId == userId
                                 join user in userRepo.GetAll().AsEnumerable()
                                 on o.UserId equals user.Id
                                 select new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = (DateTime)TimeCheckerService.GetTaipeiTime(o.OrderDate),
                                     UserName = user.UserName,
                                     PurchaserName = o.PurchaserName,
                                     PurchaserEmail = o.PurchaserEmail,
                                     PurchaserAddress = o.City + o.District + o.Road,
                                     PurchaserPhone = o.PurchaserPhone,
                                     Payment = o.Payment,
                                     PayDate = TimeCheckerService.GetTaipeiTime(o.PayDate),
                                     Paid = o.Paid,
                                     Build = o.Build
                                 }).OrderBy(item => item.OrderDate).ToList();
                return orderList;
            }
        }
        #endregion

        #region 用訂單Id取訂單
        /// <summary>
        /// 利用訂單Id取訂單
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderViewModel> GetOrder(HttpContextBase httpContext, string orderId)
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderRepo = new GeneralRepository<Order>(dbContext);
                var userRepo = new GeneralRepository<AspNetUsers>(dbContext);
                var userId = httpContext.User.Identity.GetUserId();
                var orderList = (from o in orderRepo.GetAll().AsEnumerable()
                                 where o.UserId == userId && o.OrderId.ToString() == orderId
                                 join user in userRepo.GetAll().AsEnumerable()
                                 on o.UserId equals user.Id
                                 select new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     EcpayOrderNumber = o.EcpayOrderNumber,
                                     OrderDate = (DateTime)TimeCheckerService.GetTaipeiTime(o.OrderDate),
                                     UserName = user.UserName,
                                     PurchaserName = o.PurchaserName,
                                     PurchaserEmail = o.PurchaserEmail,
                                     City = o.City,
                                     District = o.District,
                                     Road = o.Road,
                                     PurchaserPhone = o.PurchaserPhone,
                                     Payment = o.Payment,
                                     PayDate = TimeCheckerService.GetTaipeiTime(o.PayDate),
                                     Paid = o.Paid,
                                     Build = o.Build
                                 }).OrderBy(item => item.OrderDate).ToList();
                return orderList;
            }
        }
        #endregion

        #region 取得所有訂單
        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        public List<OrderViewModel> GetOrder()
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderRepo = new GeneralRepository<Order>(dbContext);
                var userRepo = new GeneralRepository<AspNetUsers>(dbContext);
                var orderList = (from o in orderRepo.GetAll().AsEnumerable()
                                 join user in userRepo.GetAll().AsEnumerable()
                                 on o.UserId equals user.Id
                                 select new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = (DateTime)TimeCheckerService.GetTaipeiTime(o.OrderDate),
                                     UserName = user.UserName,
                                     PurchaserName = o.PurchaserName,
                                     PurchaserEmail = o.PurchaserEmail,
                                     PurchaserAddress = $"{o.City}{o.District}{o.Road}",
                                     PurchaserPhone = o.PurchaserPhone,
                                     Payment = o.Payment,
                                     Paid = o.Paid,
                                     PayDate = TimeCheckerService.GetTaipeiTime(o.PayDate),
                                     Build = o.Build
                                 }).OrderBy(item => item.OrderDate).ToList();
                return orderList;
            }
        }
        #endregion

        #region 拿到訂單細節
        /// <summary>
        ///  拿到訂單裡的產品資訊
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderDetailsViewModel> GetOrderDetails(string orderId)
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderDetailRepo = new GeneralRepository<OrderDetails>(dbContext);
                var productRepo = new GeneralRepository<Product>(dbContext);
                var imgRepo = new GeneralRepository<ProductImgs>(dbContext);
                var CouponRepo = new GeneralRepository<Coupons>(dbContext);
                List<OrderDetailsViewModel> orderDetailsList = new List<OrderDetailsViewModel>();
                // 因為多張圖片會重複產品
                var tempList = (from od in orderDetailRepo.GetAll().AsEnumerable()
                                where od.OrderId.ToString() == orderId
                                join pd in productRepo.GetAll().AsEnumerable()
                                on od.ProductId equals pd.ProductId
                                join pi in imgRepo.GetAll().AsEnumerable()
                                on pd.ProductId equals pi.ProductId
                                where pd.ProductId == pi.ProductId
                                select new OrderDetailsViewModel
                                {
                                    Imagelink = pi.imgLink,
                                    Name = pd.Name,
                                    Quantity = od.Quantity,
                                    Price = pd.Price,
                                    Coupon = CouponRepo.GetFirst(item => item.Id == od.Discount),
                                    Total = Math.Round(pd.Price * od.Quantity)
                                }).GroupBy(item => item.Name);
                foreach (var productList in tempList)
                {
                    var firstProductItem = productList.FirstOrDefault(item => !item.Imagelink.Contains("-0"));
                    orderDetailsList.Add(firstProductItem);
                }
                return orderDetailsList;
            }

        }
        #endregion

        #region 拿到記住我的訂單資訊
        /// <summary>
        /// 拿到記住訂單的資訊
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public OrderViewModel GetRecordInfo(HttpContextBase httpContext)
        {
            using (var dbContext = new XbooxLibraryDBContext())
            {
                var orderRepo = new GeneralRepository<Order>(dbContext);
                var userRepo = new GeneralRepository<AspNetUsers>(dbContext);
                var userId = httpContext.User.Identity.GetUserId();
                var orderList = orderRepo.GetAll()
                    .Where(item => item.UserId == userId)
                    .OrderByDescending(item => item.OrderDate);
                var order = orderList.FirstOrDefault();
                OrderViewModel orderInfo = new OrderViewModel();
                if (order != null)
                {
                    if (order.Remember)
                    {
                        orderInfo.PurchaserName = order.PurchaserName;
                        orderInfo.City = order.City;
                        orderInfo.District = order.District;
                        orderInfo.Road = order.Road;
                        orderInfo.PurchaserEmail = order.PurchaserEmail;
                        orderInfo.PurchaserPhone = order.PurchaserPhone;
                        orderInfo.Remember = order.Remember;
                    }
                }
                return orderInfo;
            }
        }
        #endregion

        #region 建立訂單(httpcontext, order, ecpaynumber)
        /// <summary>
        /// 建立訂單
        /// </summary>
        /// <param name="httpcontext"></param>
        /// <param name="order"></param>
        /// <param name="ecpayNumber"></param>
        /// <returns></returns>
        public OperationResult CreateOrder(HttpContextBase httpcontext, OrderViewModel order, string ecpayNumber)
        {
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
                        OrderDate = DateTime.UtcNow,
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
                    var Coupon = couponRepo.GetFirst(item => item.CouponCode == order.Discount);
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
                                    Quantity = item.Quantity,
                                };
                                if (Coupon != null)
                                {
                                    orderDetails.Discount = Coupon.Id;
                                }
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
                return operationResult;
            }

        }
        #endregion

        #region 編輯付款狀態(orderId)
        /// <summary>
        /// 編輯當筆訂單的付款狀態(orderId)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OperationResult EditPaidState(string orderId)
        {
            OperationResult operationResult = new OperationResult();
            using (var dbContext = new XbooxLibraryDBContext())
            {
                try
                {
                    var orderRepo = new GeneralRepository<Order>(dbContext);
                    var order = orderRepo.GetFirst(x => x.OrderId.ToString() == orderId);
                    if (order != null)
                    {
                        if (!order.Paid)
                        {
                            order.Paid = true;
                            order.PayDate = DateTime.UtcNow;
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
        #endregion 編輯付款狀態(ecpayNumber)
        /// <summary>
        /// 編輯當筆訂單的付款狀態(ecpayNumber)
        /// </summary>
        /// <param name="ecpayNumber"></param>
        /// <returns></returns>
        public OperationResult EditPaidStateByEcNumber(string ecpayNumber)
        {
            OperationResult operationResult = new OperationResult();
            using (var dbContext = new XbooxLibraryDBContext())
            {
                try
                {
                    var orderRepo = new GeneralRepository<Order>(dbContext);
                    var order = orderRepo.GetFirst(x => x.EcpayOrderNumber.ToString() == ecpayNumber);
                    if (order != null)
                    {
                        if (!order.Paid)
                        {
                            order.Paid = true;
                            order.PayDate = DateTime.UtcNow;
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
        #region 取消訂單
        /// <summary>
        /// 取消當筆訂單(將狀態改為取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
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
        #endregion
    }
    public class FilterOrderDataService
    {
        private static OrderService orderService = new OrderService();
        /// <summary>
        /// 利用年月日篩選訂單資料
        /// 年: 輸入年分(西年)
        /// 月: 輸入數字(期間)
        /// 日: 輸入數字(期間)
        /// </summary>
        public Dictionary<string, Func<string,int,List<OrderViewModel>>> filterOrders = new Dictionary<string, Func<string, int, List<OrderViewModel>>>()
        {
            {"YEAR",(userId,num) => orderService.GetOrder(userId).Where(item => item.OrderDate.Year == num).ToList()  },
            {"MONTH",(userId,num) => orderService.GetOrder(userId).Where(item => item.OrderDate >= DateTime.UtcNow.AddMonths(-num)).ToList() },
            {"DAY",(userId,num) => orderService.GetOrder(userId).Where(item => item.OrderDate>= DateTime.UtcNow.AddDays(-num)).ToList() }
        };
        public List<OrderViewModel> GetOrderList(string userId, string start, string end)
        {
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);
            var orderList = orderService.GetOrder(userId).Where(item => item.OrderDate >= startDate && item.OrderDate <= endDate).ToList();
            return orderList;
        }
        public List<OrderViewModel> GetOrderList(string userId, string orderId)
        {
            var orderList = orderService.GetOrder(userId).Where(item => item.OrderId.ToString().Contains(orderId)).ToList();
            return orderList;
        }
    }
}