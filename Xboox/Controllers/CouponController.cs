using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models.DataTable;

namespace Xboox.Controllers
{
    public class CouponController : Controller
    {        
        // GET: Coupon
        public ActionResult ShowCoupon()
        {
            return View();
        }
    }
}