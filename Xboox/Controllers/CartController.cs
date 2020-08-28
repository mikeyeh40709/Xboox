using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.ViewModels;

namespace Xboox.Controllers
{
    public class CartController : Controller
    {
        private XbooxContext _context;
        public CartController()
        {
            if (_context == null)
            {
                _context = new XbooxContext();
            }
        }
        // GET: Carts
        public ActionResult Product()
        {
            //var cartItems = _context.CartItmes.ToList();
            List<CartViewModel> carts = new List<CartViewModel>()
            {
                new CartViewModel{Name="Wellness And Paradise",ProductImg="Wellnes.png",Total=67},
                new CartViewModel{Name="Wellness And Paradise",ProductImg="Wellnes.png",Total=67}
            };
         
            //foreach(var item in cartItems)
            //{
            //    carts.Add(new CartViewModel()
            //    {
            //        Name 
            //    });
            //}
            return View(carts);
        }

        public ActionResult ShopCart()
        {
            //var cartItems = _context.CartItmes.ToList();
            List<CartViewModel> carts = new List<CartViewModel>()
            {
                new CartViewModel{Name="Wellness And Paradise",ProductImg="Wellnes.png",Total=67},
                new CartViewModel{Name="Wellness And Paradise",ProductImg="Wellnes.png",Total=67}
            };

            //foreach(var item in cartItems)
            //{
            //    carts.Add(new CartViewModel()
            //    {
            //        Name 
            //    });
            //}
            return View(carts);
        }




        public ActionResult Bill()
        {
            return View();
        }
    }
}