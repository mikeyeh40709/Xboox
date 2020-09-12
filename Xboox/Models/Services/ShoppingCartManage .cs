using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Xboox.Models.DataTable;
using Xboox.Models.ViewModels;
using Xboox.ViewModels;

namespace Xboox.Models.Services
{
    public partial class ShoppingCartManage
    {
        XbooxContext xbooxDb = new XbooxContext();

        public void AddToCart(string values,HttpContextBase content)
        {

            var CartItems = JsonConvert.DeserializeObject<List<TempCartItems>>(values);
            var GetUserKey = content.Request.Cookies["VisitorKey"].Value;

            var cart = xbooxDb.Cart.SingleOrDefault(ca => ca.CartId.ToString() == GetUserKey);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = Guid.Parse(GetUserKey),
                    UserId =content.User.Identity.Name
                };
                xbooxDb.Cart.Add(cart);
                xbooxDb.SaveChanges();
            }

            foreach (var item in CartItems)
            {
                var ProductCheck = xbooxDb.CartItems.FirstOrDefault(
               c => c.ProductId.ToString() == item.ProductId && c.CartId.ToString() == GetUserKey);
              

                if (ProductCheck == null)
                {
                    Guid randomId = Guid.NewGuid();
                    CartItems cartItem = new CartItems
                    {
                        CartId = Guid.Parse(GetUserKey),
                        ProductId = Guid.Parse(item.ProductId),
                        Quantity = item.Count,
                        Id = randomId
                    };
                    xbooxDb.CartItems.Add(cartItem);
                }
                else
                {
                        ProductCheck.Quantity = item.Count;
                }
            }
            xbooxDb.SaveChanges();
        }
        
        public List<CartViewModel> GetCartItems(HttpContextBase context)
        {
            var watch = new Stopwatch();
            watch.Start();
            var GetUserKey = context.Request.Cookies["VisitorKey"].Value;
            using (var db = new XbooxContext())
            {
                List<CartViewModel> CartItemList = new List<CartViewModel>();
                var tempList = (from c in db.CartItems
                                join p in db.Product
                                on c.ProductId equals p.ProductId
                                join i in db.ProductImgs
                                on p.ProductId equals i.ProductId
                                where p.ProductId == i.ProductId
                                where c.CartId.ToString() == GetUserKey
                                select new CartViewModel
                                {
                                   ProductId =p.ProductId,
                                   ProductImgLink = i.imgLink,
                                   Name = p.Name,
                                   Price = p.Price,
                                   Quantity = c.Quantity,
                                   TotalPrice = c.Quantity * p.Price

                                }).GroupBy(item => item.Name);
                foreach (var pdList in tempList)
                {

                    var firstItem = pdList.FirstOrDefault(item => !item.Name.Contains("-0"));
                    CartItemList.Add(firstItem);
                }
                watch.Stop();
                Debug.WriteLine(watch.ElapsedMilliseconds);
                return CartItemList;
            }

        }
       
    }
}
