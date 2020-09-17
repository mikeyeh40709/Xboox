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
        //public IQueryable<SalesRevenueViewModel> GetSalesRevenue()
        //{
        //    XbooxLibraryDBContext db = new XbooxLibraryDBContext();
        //    GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
        //    GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
        //    GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);
        //    var temp = from od in odtable.GetAll()
        //               join o in otable.GetAll()
        //               on od.OrderId equals o.OrderId
        //               join p in ptable.GetAll()
        //               on od.ProductId equals p.ProductId
        //               where o.OrderDate.Year == 2020
        //               select new
        //               {
        //                   Month = o.OrderDate.Month,
        //                   //Total = od.UnitPrice * od.Quantity
        //               };
        //    var Revenue = from t in temp
        //                  group t by t.Month into g
        //                  select new SalesRevenueViewModel
        //                  {
        //                      Month = g.Key,
        //                      Revenue = g.Sum(x => x.Total)
        //                  };


        //    return Revenue;
        //}

        //public IQueryable<TopProducts> GetTopProducts()
        //{
        //    XbooxLibraryDBContext db = new XbooxLibraryDBContext();

        //    var temp = from od in db.OrderDetails
        //               join o in db.Order
        //               on od.OrderId equals o.OrderId
        //               //where o.StateId == 2
        //               select new
        //               {
        //                   Month = o.OrderDate.Month,
        //                   //Name = od.ProductName,
        //                   Quantity = od.Quantity
        //               };

        //    var topproducts = from t in temp
        //                      group t by new { t.Month, t.Name } into g
        //                      select new TopProducts
        //                      {
        //                          Month = g.Key.Month,
        //                          Name = g.Key.Name,
        //                          Quantity = g.Sum(x => x.Quantity)
        //                      };

        //    return topproducts;
        //}

    }
}