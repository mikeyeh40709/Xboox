using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Xboox.Models.DataTable;
using Xboox.Models.ViewModels;

namespace Xboox.Models.Services
{
    public class CartOperator
    {
        public const string CartSessionKey = "CartId";
        [WebMethod(enableSession:true)]
        public static ViewModels.CartViewModel GetCurrentCart()
        {
            if (System.Web.HttpContext.Current!=null)
            {
                if (System.Web.HttpContext.Current.Session[CartSessionKey]==null)
                {
                    var order = new CartViewModel();
                    System.Web.HttpContext.Current.Session[CartSessionKey] = order;
                }
                return (CartViewModel)System.Web.HttpContext.Current.Session[CartSessionKey];
            }
            else
            {
                throw new InvalidOperationException("Current為空,請檢查");
            }
        }
      

        
    }
}