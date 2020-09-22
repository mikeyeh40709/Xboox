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
using XbooxCMS.Services;
using System.Data.SqlClient;

namespace XbooxCMS.Service
{
    public class DashboardService
    {
        static XbooxLibraryDBContext db = new XbooxLibraryDBContext();
        GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
        GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
        GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);
        GeneralRepository<AspNetUsers> mtable = new GeneralRepository<AspNetUsers>(db);
        public IEnumerable<SalesRevenueViewModel> GetSalesRevenue()
        {
            //XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            //GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
            //GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
            //GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);
            using (SqlConnection connection = new SqlConnection("data source=xboox.database.windows.net;initial catalog=Xboox;user id=Xboox;password=nQVc544E;MultipleActiveResultSets=True;App=EntityFramework"))
            {
                try
                {
                    connection.Open();
                    var temp = from od in odtable.GetAll().AsEnumerable()
                               join o in otable.GetAll().AsEnumerable()
                               on od.OrderId equals o.OrderId
                               join p in ptable.GetAll().AsEnumerable()
                               on od.ProductId equals p.ProductId
                               where o.Paid == true   //paid 付款狀態 註解掉可以取得較多data
                               select new
                               {
                                   Year = o.OrderDate.Year,
                                   Month = o.OrderDate.Month,
                                   Total = p.Price * od.Quantity
                               };
                    var Revenue = from t in temp
                                  group t by new { t.Month, t.Year } into g
                                  select new SalesRevenueViewModel
                                  {
                                      Year = g.Key.Year,
                                      Month = g.Key.Month,
                                      Revenue = g.Sum(x => x.Total)
                                  };

                    return Revenue;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
               


        }

        public IQueryable<TopProducts> GetTopProducts()
        {
            //XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            //GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
            //GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
            //GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);
            using (SqlConnection connection = new SqlConnection("data source=xboox.database.windows.net;initial catalog=Xboox;user id=Xboox;password=nQVc544E;MultipleActiveResultSets=True;App=EntityFramework"))
            {
                try
                {
                    connection.Open();
                    var temp = from od in odtable.GetAll()
                               join o in otable.GetAll()
                               on od.OrderId equals o.OrderId
                               join p in ptable.GetAll()
                               on od.ProductId equals p.ProductId
                               where o.Paid == true   //paid 付款狀態 註解掉可以取得較多data
                               select new
                               {
                                   Year = o.OrderDate.Year,
                                   Month = o.OrderDate.Month,
                                   Name = p.Name,
                                   Quantity = od.Quantity
                               };

                    var topproducts = from t in temp
                                      group t by new { t.Year, t.Month, t.Name } into g
                                      select new TopProducts
                                      {
                                          Month = g.Key.Month,
                                          Name = g.Key.Name,
                                          Quantity = g.Sum(x => x.Quantity)
                                      };

                    return topproducts;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public TitleData GetTitleData()
        {
            //XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            //GeneralRepository<AspNetUsers> mtable = new GeneralRepository<AspNetUsers>(db);
            //GeneralRepository<Product> ptable = new GeneralRepository<Product>(db);
            //GeneralRepository<Order> otable = new GeneralRepository<Order>(db);
            //GeneralRepository<OrderDetails> odtable = new GeneralRepository<OrderDetails>(db);
            using (SqlConnection connection = new SqlConnection("data source=xboox.database.windows.net;initial catalog=Xboox;user id=Xboox;password=nQVc544E;MultipleActiveResultSets=True;App=EntityFramework"))
            {
                try
                {
                    connection.Open();
                    var revenue = (from o in otable.GetAll().AsEnumerable()
                                   join od in odtable.GetAll().AsEnumerable()
                                   on o.OrderId equals od.OrderId
                                   join p in ptable.GetAll().AsEnumerable()
                                   on od.ProductId equals p.ProductId
                                   where o.OrderDate.Month == DateTime.UtcNow.Month
                                   where o.Paid == true   //paid 付款狀態 註解掉可以取得較多data
                                   select new
                                   {
                                       Revenue = p.Price * od.Quantity
                                   }).Sum(x => x.Revenue);

                    //entity 資料庫映射回來的 實體
                    //linq to object(Ienumerable) ,  linq to entity(IQueryable)

                    var members = mtable.GetAll().Count();
                    //(from m in mtable.GetAll()
                    //           select new
                    //           {
                    //               members = m.Id
                    //           }).Count();

                    var products = ptable.GetAll().Count();
                    //(from p in ptable.GetAll()
                    //            select new
                    //            {
                    //                products = p.Name
                    //            }).Count();

                    var orders = otable.GetAll().Where(x => x.OrderDate.Month == DateTime.UtcNow.Month && x.Paid == true).Count();
                    //(from o in otable.GetAll()
                    //          where o.OrderDate.Month == DateTime.UtcNow.Month
                    //          where o.Paid == true
                    //          select new
                    //          {
                    //              orders = o.OrderId
                    //          }
                    //          ).Count();
                    TitleData title = new TitleData()
                    {
                        products = products,
                        orders = orders,
                        members = members,
                        revenue = revenue
                    };


                    return title;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

    }
}