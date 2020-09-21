using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using XbooxCMS.Models;
using XbooxLibrary.Models.DataTable;
using XbooxCMS.ViewModels;
using XbooxCMS.Services;

namespace XbooxCMS.Controllers
{
    public class TagsController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
    }
}
