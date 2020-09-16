using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxLibrary.Models.DataTable;
using XbooxCMS.ViewModels;
using XbooxLibrary.Repository;

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
            var orderList = (from o in odrepo.GetAll()
                            join user in userrepo.GetAll()
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










        public List<OrderDetailsViewModel> GetOrderDeatils(string id)
        {
           XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            var result = new List<OrderDetailsViewModel>();
            var odrepo = new GeneralRepository<OrderDetails>(context);
            var productrepo = new GeneralRepository<Product>(context);
            var imgrepo = new GeneralRepository<ProductImgs>(context);
          
            var tempList = (from od in odrepo.GetAll()
                            where od.OrderId.ToString()==id
                            join pd in productrepo.GetAll()
                            on od.ProductId equals pd.ProductId
                            join pi in imgrepo.GetAll()
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
                result.Add(firstProductItem);
            }
            return result;
        }


    }
    
}