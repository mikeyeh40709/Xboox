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
        private enum payment
        {
            Unpaid = 0,
            Paid = 1
        }
        private XbooxContext context = new XbooxContext();
        public List<OrderViewModel> GetOrder(string id)
        {
            using (var contexy = new XbooxContext())
            {
                var orderList = (from o in contexy.Order
                                 where o.UserId == id
                                 join user in contexy.AspNetUsers
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
            using (var contexy = new XbooxContext())
            {
                var orderList = (from o in contexy.Order
                                 join user in contexy.AspNetUsers
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
            using (var contexy = new XbooxContext())
            {
                List<OrderDetailsViewModel> orderDetailsList = new List<OrderDetailsViewModel>();
                // 因為多張圖片會重複產品
                var tempList = (from od in contexy.OrderDetails
                                join pd in contexy.Product
                                on od.ProductId equals pd.ProductId
                                join pi in contexy.ProductImgs
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
        //public bool Create(CartItems, OrderDetails orderDetails)
        //{
        //    XbooxContext context = new XbooxContext()
        //    using (var transaction = context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var sellingCount = orderDetails.Quantity;
        //            Selling entity = new Selling()
        //            {
        //                PartNo = input.PartNo,
        //                Quantity = input.Quantity,
        //                SalesJobNumber = input.SalesJobNumber,
        //                SellingDay = input.SellingDay,
        //                UnitPrice = input.UnitPrice
        //            };
        //            sellingRepo.Create(entity);
        //            context.SaveChanges();

        //            var products = procurementRepo.GetAll()
        //                .Where(x => x.PartNo == input.PartNo)
        //                .OrderBy(x => x.PurchasingDay);
        //            foreach (var p in products)
        //            {
        //                if (sellingCount <= 0)
        //                {
        //                    break;
        //                }
        //                else
        //                {
        //                    if (p.InvetoryQuantity >= sellingCount)
        //                    {
        //                        p.InvetoryQuantity = p.InvetoryQuantity - sellingCount;
        //                        CreateSellingSource(sellingSourceRepo, entity.SellingId, p.ProcurementId, sellingCount);
        //                        sellingCount = 0;
        //                    }
        //                }
        //            }
        //            context.SaveChanges();
        //            result.isSuccessful = true;
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            result.isSuccessful = false;
        //            result.exception = ex;
        //            transaction.Rollback();
        //        }
        //    }
        //    return result;
        //}
        public bool EditState(string id)
        {
            XbooxContext contexy = new XbooxContext();
            using (var transaction = contexy.Database.BeginTransaction())
            {
                var order = contexy.Order.FirstOrDefault(x => x.OrderId.ToString() == id);
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
                    contexy.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Delete(string id)
        {
            XbooxContext contexy = new XbooxContext();
            using (var transaction = contexy.Database.BeginTransaction())
            {
                var OrderDetails = contexy.OrderDetails.Where(item => item.OrderId.ToString() == id);
                var order = contexy.Order.FirstOrDefault(item => item.OrderId.ToString() == id);
                if (order != null && OrderDetails != null)
                {
                    if (order.StateId == (int)payment.Unpaid)
                    {
                        foreach (var item in OrderDetails)
                        {
                            var products = contexy.Product.Where(pd => pd.ProductId == item.ProductId).OrderBy(pd => pd.PublishedDate);
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
                            contexy.OrderDetails.Remove(item);
                        }
                        contexy.Order.Remove(order);
                        contexy.SaveChanges();
                    }

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