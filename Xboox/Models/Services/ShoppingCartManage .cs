using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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

        public void AddToCart(Product p, HttpContextBase context)
        {


            var GetUserKey = context.Request.Cookies["VisitorKey"].Value;
       
            var cart = xbooxDb.Cart.SingleOrDefault(ca => ca.CartId.ToString() == GetUserKey);

            if (cart == null)
            {

                cart = new Cart
                {
                    CartId = Guid.Parse(GetUserKey),
                    //因為UserId欄位允許null , 今天若是會員未登入會取拿名字
                    //如是訪客即為null
                    UserId = context.User.Identity.Name
                };
                xbooxDb.Cart.Add(cart);
                xbooxDb.SaveChanges();
                
            }
            var cartItem = xbooxDb.CartItems.SingleOrDefault(
                c => c.CartId.ToString() == GetUserKey
                && c.ProductId == p.ProductId);
            if (cartItem == null)
            {
                Guid randomId = Guid.NewGuid();
                cartItem = new CartItems
                {
                  
                    CartId = Guid.Parse(GetUserKey),
                    ProductId = p.ProductId,
                    Quantity = 1,
                    Id = randomId
                };
                xbooxDb.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            xbooxDb.SaveChanges();
        }
        public List<CartViewModel> GetCartItems(HttpContextBase context)
        {
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
                return CartItemList;
            }

        }
       
    }
}
