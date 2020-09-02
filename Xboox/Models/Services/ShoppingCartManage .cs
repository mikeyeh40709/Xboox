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

        public void AddToCart(Product p)
        {
            var cartItem = xbooxDb.CartItems.SingleOrDefault(
                c => c.CartId.ToString() == ShoppingCartId
                && c.ProductId == p.ProductId);
            if (cartItem==null)
            {

            }
            else
            {

            }


        }


        //public void AddToCart(Product book)
        //{
        //    var cartDb = xbooxDb.Cart;
        //    var productDb = xbooxDb.Product;
        //    var cartItemDb = xbooxDb.CartItmes;
        //    List<CartItmes> CartItemList = new List<CartItmes>();
        //    var query = from ci in cartItemDb
        //                join p in productDb
        //                on ci.ProductId equals p.ProductId
        //                select new CartItmes
        //                {
        //                    ProductId = p.ProductId
        //                };
        //    foreach (var id in query)
        //    {
        //        CartItemList.Add(id);
        //    }
        //    //從Model裡面的Cart.cs裡面抓取CartId並給予ViewModel裡面的
        //    //CartViewModel裡面的CartId
        //    //並且跟上述的ShoppingCartId做比對
        //    List<CartViewModel> CartViewList = new List<CartViewModel>();
        //    {
        //        foreach (var c in cartDb)
        //        {
        //            new CartViewModel { Id = c.CartId };
        //        }
        //    }

        //    var FindcartId = CartViewList.SingleOrDefault(
        //    c => c.Id.ToString() == ShoppingCartId);
        //    var FindProductId = CartItemList.SingleOrDefault(ci => ci.ProductId == book.ProductId);


        //    if (FindcartId == null && FindProductId == null)
        //    {
        //        // Create a new cart item if no cart item exists
        //        newCartItem = new CartViewModel
        //        {


        //        };
        //        xbooxDb.Carts.Add(newCartItem);
        //    }
        //    else
        //    {
        //        // If the item does exist in the cart, 
        //        // then add one to the quantity
        //        cartItem.Count++;
        //    }
        //    // Save changes
        //    xbooxDb.SaveChanges();
        //}



    }
}