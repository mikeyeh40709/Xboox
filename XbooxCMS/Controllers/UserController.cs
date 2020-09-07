using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;

namespace XbooxCMS.Controllers
{
    public class UserController : Controller
    {
        private XbooxContext context;
        public UserController()
        {
            if (context == null)
            {
                context = new XbooxContext();
            }
        }
        // GET: User
        public ActionResult Index()
        {
            var userList = context.AspNetUsers.ToList();
            List<UserListViewModel> viewModel = new List<UserListViewModel>();
            foreach(var i in userList)
            {
                viewModel.Add(new UserListViewModel()
                {
                    Id = i.Id,
                    Email = i.Email,
                    UserName = i.UserName,
                    PhoneNumber = i.PhoneNumber
                });
            };



            //var user = (from p in context.AspNetUsers
            //           select new UserListViewModel()
            //           {
            //               Id = p.Id,
            //               Email = p.Email,
            //               UserName = p.UserName,
            //               PhoneNumber = p.PhoneNumber
            //           }).ToList();
            //var users = context.AspNetUsers.ToList();

            return View(viewModel);
        }
    }
}