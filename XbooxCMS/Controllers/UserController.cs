using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;
using XbooxCMS.Services;
using XbooxLibrary.Models.DataTable;
namespace XbooxCMS.Controllers
{
    public class UserController : Controller
    {

        public UserController()
        {
            //if (context == null)
            //{
            //    context = new XbooxLibraryDBContext();
            //}
        }
        // GET: User
        public ActionResult Index()
        {

            UserService service = new UserService();
            var userList = service.GetAllUsers();
            //var userList = context.AspNetUsers.ToList();
            //List<UserListViewModel> viewModel = new List<UserListViewModel>();
            //foreach(var i in userList)
            //{
            //    viewModel.Add(new UserListViewModel()
            //    {
            //        Id = i.Id,
            //        Email = i.Email,
            //        UserName = i.UserName,
            //        PhoneNumber = i.PhoneNumber
            //    });
            //};

            return View(userList);
        }
    }
}