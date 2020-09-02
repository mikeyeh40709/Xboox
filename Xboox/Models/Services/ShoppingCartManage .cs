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
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        public static ShoppingCartManage GetCart(Controller controller)
        {

            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product p, HttpContextBase context)
        {
            var cart = xbooxDb.Cart.SingleOrDefault(
                ca => ca.CartId.ToString() == ShoppingCartId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = Guid.Parse(ShoppingCartId),
                    UserId = ShoppingCartId
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
            //   CartItems cartItems = new CartItems();

            //var x =  xbooxDb.CartItems.Where(
            //      c => c.CartId.ToString() == ShoppingCartId);

            //   if (x==ShoppingCartId)
            //   {

            //   }
            using (var db = new XbooxContext())
            {
                List<CartViewModel> cartList = new List<CartViewModel>();
                var getFirstItem = (from p in db.Product
                                join cart in db.CartItems
                                on p.ProductId equals cart.ProductId
                                join i in db.ProductImgs
                                on p.ProductId equals i.ProductId

                                select new CartViewModel
                                {
                                    ProductImgLink = i.imgLink,
                                    Name = p.Name,
                                    Price = p.Price,
                                    Count = cart.Quantity
                                    //TotalPrice = cart.Quantity * p.Price
                                }).FirstOrDefault();
                cartList.Add(getFirstItem);
                return cartList;
            }

            //xbooxDb.CartItems.Where(item => item.CartId.ToString() == ShoppingCartId).ToList();

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
        //public int RemoveFromCart(Guid id)
        //{
        //    // Get the cart
        //    var cartItem = xbooxDb.CartItmes.Single(
        //        cart => cart.CartId == Guid.Parse(ShoppingCartId)
        //        && cart.Id == id);

        //    int itemCount = 0;

        //    if (cartItem != null)
        //    {
        //        if (cartItem > 1)
        //        {
        //            cartItem.Count--;
        //            itemCount = cartItem.Count;
        //        }
        //        else
        //        {
        //            storeDB.Carts.Remove(cartItem);
        //        }
        //        // Save changes
        //        storeDB.SaveChanges();
        //    }
        //    return itemCount;
        //}
    }
}