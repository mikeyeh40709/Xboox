using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Xboox.Models.DataTable;
using XbooxLibrary.Repository;
using XbooxLibrary.Models.DataTable;

namespace Xboox.Models.Services
{
    public class AspNetUserManage
    {
        

        public static UserDetails GetUserDetails(HttpContextBase context)
        {
            XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            GeneralRepository<AspNetUsers> user = new GeneralRepository<AspNetUsers>(db);
            var details = (from u in user.GetAll()
                           where u.UserName == context.User.Identity.Name
                           select new UserDetails
                           {
                               Id = u.Id,
                               Account = u.UserName,
                               Email = u.Email,
                               Phone = u.PhoneNumber
                           }).FirstOrDefault();

            return details;
        }        

        public void EditUserDetails(UserDetails userdetails)
        {
            XbooxLibraryDBContext db = new XbooxLibraryDBContext();
            GeneralRepository<AspNetUsers> user = new GeneralRepository<AspNetUsers>(db);
            var edit = user.GetAll().FirstOrDefault(u => u.Id == userdetails.Id);
            edit.Email = userdetails.Email;
            edit.PhoneNumber = userdetails.Phone;
            user.Update(edit);
            user.SaveContext();

        }
    }
}