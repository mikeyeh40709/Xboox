using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Xboox.Models.DataTable;
using Xboox.Models.ViewModels;

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

        //使用GetCartId方法來處理從使用者的會話讀取 CartId。 
        //GetCartId 方法需要 HttpCoNtextBase，讓它可以從使用者的會話讀取使用者的 CartId。
        public string GetCartId(HttpContextBase context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {   
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
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
                       context.User.Identity.Name;
            }
            return context.Session[CartSessionKey].ToString();
            //最後會傳回去給ShoppingCartId
        }
        public void MigrateCart(string userName)
        {
            var shoppingCart = xbooxDb.CartItmes.Where(
                c => c.CartId ==Guid.Parse(ShoppingCartId));

            foreach (CartItmes item in shoppingCart)
            {
                item.CartId = Guid.Parse(userName);
            }
            xbooxDb.SaveChanges();
        }
        public void AddToCart(Product p)
        {
            var cartItem = xbooxDb.CartItmes.SingleOrDefault(
                c => c.CartId == Guid.Parse(ShoppingCartId)
                && c.ProductId == p.ProductId);
            if (cartItem==null)
            {
                Guid randomId = Guid.NewGuid();
               new CartItmes
                {
                    //這邊要考慮一下
                    CartId = Guid.Parse(ShoppingCartId),
                    ProductId = p.ProductId,
                    Quantity = 1,
                    Id= randomId
                };
                xbooxDb.CartItmes.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            xbooxDb.SaveChanges();
        }

        public List<CartItmes> GetCartItems()
        {
            return xbooxDb.CartItmes.Where(
                cart => cart.CartId == Guid.Parse(ShoppingCartId)).ToList();
        }






    }
}