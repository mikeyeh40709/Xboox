using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Models.DataTable;
using XbooxCMS.ViewModels;
using XbooxLibrary.Repository;
using XbooxLibrary.Services;

namespace XbooxCMS.Services
{
    public class OrderService
    {

      
        public List<OrderViewModel> GetAllOrders()
        {

            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            var odrepo = new GeneralRepository<Order>(context);
            var userrepo = new GeneralRepository<AspNetUsers>(context);
            var result = new List<OrderViewModel>();
            var orderList = (from o in odrepo.GetAll().AsEnumerable()
                            join user in userrepo.GetAll().AsEnumerable()
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
                                PayDate = TimeCheckerService.GetTaipeiTime(o.PayDate),
                                Payment = o.Payment,
                                Paid = o.Paid,
                                Build = o.Build
                            }).OrderByDescending(item => item.OrderDate).ToList();


            return orderList;
        }
        public List<OrderDetailsViewModel> GetOrderDeatils(string id)
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
                                where od.OrderId.ToString() == id
                                join pd in productRepo.GetAll().AsEnumerable()
                                on od.ProductId equals pd.ProductId
                                join pi in imgRepo.GetAll().AsEnumerable()
                                on pd.ProductId equals pi.ProductId
                                where pd.ProductId == pi.ProductId
                                select new OrderDetailsViewModel
                                {
                                    Imagelink = pi.imgLink,
                                    ProductName = pd.Name,
                                    Quantity = od.Quantity,
                                    UnitPrice = pd.Price,
                                    Coupon = CouponRepo.GetFirst(item => item.Id == od.Discount),
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

        //取消訂單
        public OperationResult CancelOrder(string id)
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
                    var orderDetails = orderDetailRepo.GetAll().Where(item => item.OrderId.ToString() == id);
                    var order = orderRepo.GetFirst(item => item.OrderId.ToString() == id);
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
                            operationResult.IsSuccessful = true;
                            transaction.Commit();
                        }
                    }
                }

                catch (Exception ex)
                {
                    operationResult.IsSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
            }
            return operationResult;
        }
        //刪除
        public OperationResult DeleteOrder(string id)
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
                    var orderDetails = orderDetailRepo.GetAll().Where(item => item.OrderId.ToString() == id);
                    var order = orderRepo.GetFirst(item => item.OrderId.ToString() == id);
                    if (order != null && orderDetails != null)
                    {
                        foreach(var item in orderDetails)
                        {
                            orderDetailRepo.Delete(item);
                        }
                        orderRepo.Delete(order);
                        productRepo.SaveContext();
                        operationResult.IsSuccessful = true;
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    operationResult.IsSuccessful = false;
                    operationResult.exception = ex;
                    transaction.Rollback();
                }
            }
            return operationResult;
        }
    }
    
}