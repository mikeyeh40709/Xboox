using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace XbooxCMS.Service
{
    public class DashboardService
    {
        public IQueryable<SalesRevenueViewModel> GetSalesRevenue()
        {
            XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
            GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
            GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);

            var temp = from od in odtable.GetAll()
                       join o in otable.GetAll()
                       on od.OrderId equals o.OrderId
                       join p in ptable.GetAll()
                       on od.ProductId equals p.ProductId
                       //where o.Paid == true   //paid 付款狀態 註解掉可以取得較多data
                       select new
                       {
                           Year = o.OrderDate.Year,
                           Month = o.OrderDate.Month,
                           Total = p.Price * od.Quantity
                       };
            var Revenue = from t in temp
                          group t by new { t.Month,t.Year} into g
                          select new SalesRevenueViewModel
                          {
                              Year = g.Key.Year,
                              Month = g.Key.Month,
                              Revenue = g.Sum(x => x.Total)
                          };


            return Revenue;
        }

        public IQueryable<TopProducts> GetTopProducts()
        {
            XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
            GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
            GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);
            var temp = from od in odtable.GetAll()
                       join o in otable.GetAll()
                       on od.OrderId equals o.OrderId
                       join p in ptable.GetAll()
                       on od.ProductId equals p.ProductId
                       //where o.StateId == 2
                       select new
                       {
                           Year = o.OrderDate.Year,
                           Month = o.OrderDate.Month,
                           Name = p.Name,
                           Quantity = od.Quantity
                       };

            var topproducts = from t in temp
                              group t by new {t.Year, t.Month, t.Name } into g
                              select new TopProducts
                              {
                                  Month = g.Key.Month,
                                  Name = g.Key.Name,
                                  Quantity = g.Sum(x => x.Quantity)
                              };

            return topproducts;
        }

    }
}