using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;

namespace XbooxCMS.Controllers
{
    public class UserController : Controller
    {
        private XbooxContext context = new XbooxContext();
        // GET: User
        public ActionResult Index()
        {
             var users = context.AspNetRoles.ToList();

            return View(users);
        }
    }
}