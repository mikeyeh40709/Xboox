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
        /// <summary>
        /// 靜態方法，可讓我們的控制器取得購物車物件。 
        /// 它會使用GetCartId方法來處理從使用者的會話讀取 CartId。 
        /// GetCartId 方法需要 HttpCoNtextBase，
        /// 讓它可以從使用者的會話讀取使用者的 CartId。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// ////////////////////////////////////////////
        //HttpContextBase:
        //摘要:
        //     取得關於個別 HTTP 要求的 HTTP 特定資訊。
        //
        // 傳回:
        //     HTTP 內容。
        public static ShoppingCartManage GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartManage();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        //     提供方法，這些方法回應對 ASP.NET MVC 網站提出的 HTTP 要求。
        public static ShoppingCartManage GetCart(Controller controller)
        {
            // public HttpContextBase HttpContext { get; }
            // 摘要:
            //     取得關於個別 HTTP 要求的 HTTP 特定資訊。
            // 
            // 傳回:
            //     HTTP 內容。

            return GetCart(controller.HttpContext);
        }

        //使用GetCartId方法來處理從使用者的會話讀取 CartId。 
        //GetCartId 方法需要 HttpCoNtextBase，讓它可以從使用者的會話讀取使用者的 CartId。
        public string GetCartId(HttpContextBase context)
        {

            //如果今天Session["CartSessionKey"]為空的話,代表會員未登入
            //Session變數是伺服器端用來記錄用戶端個別資訊
            //常用來記錄使用者是否登入的狀態
            /////在HttpContextBase這個抽象類別包含Session這個屬性
            if (context.Session[CartSessionKey] == null)
            {   //User的定義:在衍生類別中覆寫時，取得或設定目前 HTTP 要求的安全性資訊。
                //IPrincipal 介面中有屬性Identity:取得目前主要物件的識別。
                //.Name去取得使用者名稱
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


            return context.Session[CartSessionKey].ToString();
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