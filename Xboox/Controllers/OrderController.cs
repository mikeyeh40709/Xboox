using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xboox.Models;
using Xboox.Models.ViewModels;
using Xboox.Services;

namespace Xboox.Controllers
{
    public class OrderController : Controller
    {
        private XbooxContext context;
        OrderService service = new OrderService();
        public OrderController()
        {
            if(context == null)
            {
                context = new XbooxContext();
            }
        }
        // GET: Order
        public ActionResult UserView(string userName)
        {
            
            return View();
        }
        public ActionResult ManagerView()
        {
            var result = service.GetOrder();
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            return View();
        }
        // 後臺會拿到所有使用者訂單
        public ActionResult GetAllOrder()
        {
            return View();
        }
        // 使用者拿到自己的訂單
        public ActionResult GetOrder(Guid? id)
        {
            return View();
        }

        // 編輯付款狀態(後台)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeState(Guid? id)
        {
            return View();
        }
        // 刪除某筆訂單(使用者和後台)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder()
        {
            return View();
        }
    }
}