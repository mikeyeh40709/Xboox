using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.DataTable;

namespace Xboox.Models.Services
{
    public class AspNetUserManage
    {
        

        public static List<UserDetails> GetUserDetails(HttpContextBase context)
        {
            XbooxContext db = new XbooxContext();
            var details =( from u in db.AspNetUsers
                          where u.UserName == context.User.Identity.Name
                          select new UserDetails
                          {
                              Account = u.UserName,
                              Email = u.Email,
                              Phone = u.PhoneNumber
                          }).ToList();

            return details;
        }
        
    }
}