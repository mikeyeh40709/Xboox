using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xboox.Models.Services
{
    public partial class ShoppingCartManage
    {
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
        public static ShoppingCartManage GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }


        public string GetCartId(HttpContextBase context)
        {
          
            //如果今天Session["CartSessionKey"]為空的話,代表會員未登入
            //Session變數是伺服器端用來記錄用戶端個別資訊
            //常用來記錄使用者是否登入的狀態
            if (context.Session[CartSessionKey] == null)
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

            //應該是指會員登入狀態
            return context.Session[CartSessionKey].ToString();
        }
    }
}