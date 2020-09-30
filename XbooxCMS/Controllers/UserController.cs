using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;
using XbooxCMS.ViewModels;
using XbooxCMS.Services;
using XbooxLibrary.Models.DataTable;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;

namespace XbooxCMS.Controllers
{
    [Authorize(Roles = "Admin")]
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
           return View();
        }
    }
}