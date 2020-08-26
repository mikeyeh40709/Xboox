using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using XbooxCMS.Models.ViewModels;

namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        private XBooxContext _context;
        public CartController()
        {
            if (_context == null)
            {
                _context = new XBooxContext();
            }
        }
        // GET: Cart
        public ActionResult Product()
        {
            var cartItems = _context.CartItmes.ToList();
            List<CartViewModel> carts = new List<CartViewModel>();
            //foreach(var item in cartItems)
            //{
            //    carts.Add(new CartViewModel()
            //    {
            //        Name 
            //    });
            //}
            return View();
        }
        public ActionResult Bill()
        {
            return View();
        }
    }
}