using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Product()
        {
            return View();
        }
        public ActionResult Bill()
        {
            return View();
        }
    }
}