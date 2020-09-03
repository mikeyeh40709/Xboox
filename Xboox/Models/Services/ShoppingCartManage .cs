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
        //ViewModels.CartViewModel CartView = new ViewModels.CartViewModel();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCartManage GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartManage();
            //這裡為問題發生點
            //if (cart.ShoppingCartId==null)
            //{
                cart.ShoppingCartId = cart.GetCartId(context);
            //}     
            return cart;
        }
        //public static ShoppingCartManage GetCart(Controller controller)
        //{

        //    return GetCart(controller.HttpContext);
        //}

        public void AddToCart(Product p, HttpContextBase context)
        {
            var cart = xbooxDb.Cart.SingleOrDefault(
                ca => ca.CartId.ToString() == ShoppingCartId);

            if (cart == null)
            {

                cart = new Cart
                {
                    CartId = Guid.Parse(ShoppingCartId),
                    //因為UserId欄位允許null , 今天若是會員未登入會取拿名字
                    //如是訪客即為null
                    UserId = context.User.Identity.Name
                };
                xbooxDb.Cart.Add(cart);
                xbooxDb.SaveChanges();
            }



            var cartItem = xbooxDb.CartItems.SingleOrDefault(
                c => c.CartId.ToString() == ShoppingCartId
                && c.ProductId == p.ProductId);
            if (cartItem == null)
            {
                Guid randomId = Guid.NewGuid();
                cartItem = new CartItems
                {
                    //這邊要考慮一下
                    CartId = Guid.Parse(ShoppingCartId),
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
        public List<CartViewModel> GetCartItems()
           
        {
            using (var db = new XbooxContext())
            {
                var personId = HttpContext.Current.Session[CartSessionKey].ToString();
                List<CartViewModel> CartItemList = new List<CartViewModel>();
                


                var personalCartList = db.CartItems.Where(item => item.CartId.ToString() == personId).ToList();
                var tempList = (from c in personalCartList
                                join p in db.Product
                                on c.ProductId equals p.ProductId
                                join i in db.ProductImgs
                                on p.ProductId equals i.ProductId
                                where p.ProductId == i.ProductId
                                select new CartViewModel
                                {
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




            //using (var db = new XbooxContext())
            //{
            //    List<CartViewModel> cartList = new List<CartViewModel>();
            //    var getFirstItem = (from p in db.Product
            //                    join cart in db.CartItems
            //                    on p.ProductId equals cart.ProductId
            //                    join i in db.ProductImgs
            //                    on p.ProductId equals i.ProductId

            //                    select new CartViewModel
            //                    {
            //                        ProductImgLink = i.imgLink,
            //                        Name = p.Name,
            //                        Price = p.Price,
            //                        Count = cart.Quantity
            //                        //TotalPrice = cart.Quantity * p.Price
            //                    }).FirstOrDefault();
            //    cartList.Add(getFirstItem);
            //    return cartList;
            //}

            //xbooxDb.CartItems.Where(item => item.CartId.ToString() == ShoppingCartId).ToList();

        }


        public int RemoveFromCart(Guid id, Product p)
        {
            // Get the cart
            var cartItem = xbooxDb.CartItems.SingleOrDefault(
                c => c.CartId.ToString() == ShoppingCartId
                && c.ProductId == p.ProductId);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    xbooxDb.CartItems.Remove(cartItem);
                }
                // Save changes
                xbooxDb.SaveChanges();
            }
            return itemCount;
        }


        public string GetCartId(HttpContextBase context)
        {
            //這邊判斷是否有無授權
            if (!context.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.GetUserId()))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.GetUserId();
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            else
            {
                context.Session[CartSessionKey] =
                       context.User.Identity.GetUserId();
            }
            return context.Session[CartSessionKey].ToString();
            //最後會傳回去給ShoppingCartId
        }



        //public int GetCount()
        //{
        //    // Get the count of each item in the cart and sum them up
        //    int? count = (from cartItems in xbooxDb.CartItems
        //                  where cartItems.CartId.ToString() == ShoppingCartId
        //                  select (int?)cartItems.Quantity).Sum();
        //    // Return 0 if all entries are null
        //    return count ?? 0;
        //}
        //public decimal GetTotal(Product p)
        //{
        //    // Multiply album price by count of that album to get 
        //    // the current price for each of those albums in the cart
        //    // sum all album price totals to get the cart total
        //    decimal? total = (from cartItems in xbooxDb.CartItems
        //                      where cartItems.CartId.ToString() == ShoppingCartId
        //                      select (int?)cartItems.Quantity *
        //                      p.Price).Sum();

        //    return total ?? decimal.Zero;
        //}






        public void MigrateCart(string userName)
        {
            var shoppingCart = xbooxDb.CartItems.Where(
                c => c.CartId.ToString() == ShoppingCartId);

            foreach (CartItems item in shoppingCart)
            {
                item.CartId = Guid.Parse(userName);
            }
            xbooxDb.SaveChanges();
        }





    }
}