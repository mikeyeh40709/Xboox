using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.Services;
namespace Xboox.Models.Services
{
    public class SetCookieService
    {
        public static HttpCookie SetCookie()
        {
            HttpCookie SetCookies = new HttpCookie("VisitorKey");
            SetCookies.Value = GetAllKey();
            SetCookies.Expires = DateTime.Now.AddDays(7);
            return SetCookies;
        }

        private static string GetAllKey()
        {

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Request.Cookies["VisitorKey"] == null)
                {
                    Guid VisitorKey = Guid.NewGuid();                   
                    return VisitorKey.ToString();
                }
                else
                {
                    return (HttpContext.Current.Request.Cookies["VisitorKey"].Value);
                }   
            }
            else
            {              
                var Member = HttpContext.Current.User.Identity.GetUserId();

                return Member;
            }
        }


    }
    public class TempCartItems
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }

    }
}