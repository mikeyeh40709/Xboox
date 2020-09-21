using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;
using Newtonsoft.Json;
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
                       //where o.Paid == true   //paid 付款狀態 註解掉可以取得較多data
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

        public TitleData GetTitleData()
        {
            XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            GeneralRepository<AspNetUsers> ms = new GeneralRepository<AspNetUsers>(db);
            GeneralRepository<Product> ps = new GeneralRepository<Product>(db);
            GeneralRepository<Order> os = new GeneralRepository<Order>(db);
            GeneralRepository<OrderDetails> ods = new GeneralRepository<OrderDetails>(db);
            DateTime NowMonth = DateTime.Now;
            var revenue = (from o in os.GetAll()
                           join od in ods.GetAll()
                           on o.OrderId equals od.OrderId
                           join p in ps.GetAll()
                           on od.ProductId equals p.ProductId
                           where o.OrderDate.Month == NowMonth.Month
                           //where o.Paid == true   //paid 付款狀態 註解掉可以取得較多data
                           select new
                           {
                               Revenue = p.Price * od.Quantity
                           }).ToList();


            var totalreneve = revenue.Sum(x => x.Revenue);

            var members = (from m in ms.GetAll()
                           select new
                           {
                               members = m.Id
                           }).Count();

            var products = (from p in ps.GetAll()
                            select new
                            {
                                products = p.Name
                            }).Count();

            var orders = (from o in os.GetAll()
                          where o.OrderDate.Month == NowMonth.Month
                          select new
                          {
                              orders = o.OrderId
                          }
                          ).Count();
            TitleData title = new TitleData() {
                products = products,
                orders = orders,
                members = members,
                revenue = totalreneve
            };
            

            return title;
        }

    }
}