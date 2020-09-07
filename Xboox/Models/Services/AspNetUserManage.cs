using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;

namespace Xboox.Models.Services
{
    public class AspNetUserManage
    {
        

        public static AspNetUsers GetUserDetails(HttpContextBase context)
        {
            XbooxContext db = new XbooxContext();
            var details = db.AspNetUsers.FirstOrDefault(u => u.UserName == context.User.Identity.Name);
            //var details = ( from u in db.AspNetUsers
            //              where u.UserName == context.User.Identity.Name
            //              select new UserDetails
            //              {
            //                  Account = u.UserName,
            //                  Email = u.Email,
            //                  Phone = u.PhoneNumber
            //              }).ToList();

            return details;
        }

        public static String GetUserName(HttpContextBase context)
        {
            var username = context.User.Identity.Name;
            return username;
        }
        
    }
}