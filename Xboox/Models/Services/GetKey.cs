using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xboox.Models.Services;
namespace Xboox.Models.Services
{
    public  class GetKey
    {
        private string Visitor { get; set; }
        ShoppingCartManage ShoppingCartManage = new ShoppingCartManage();
        //private static HttpContextBase contextBase;
        public string GetAllKey(HttpContextBase contextBase)
        {

            if (!contextBase.User.Identity.IsAuthenticated)
            {
                if (contextBase.Request.Cookies["VisitorKey"] == null)
                {
                    Guid VisitorKey = Guid.NewGuid();
                    //ViewBag.XbooxKey = VisitorKey;
                    Visitor = VisitorKey.ToString();
                    return VisitorKey.ToString();
                }
                else
                {
                    return contextBase.Request.Cookies["VisitorKey"].Value;
                }   
            }
            else
            {
                //var Visitor = contextBase.Request.Cookies["VisitorKey"].Value;
               
                var Member = contextBase.User.Identity.GetUserId();
                if (Visitor != null)
                {
                    ShoppingCartManage.MigrateCart(Visitor, Member);
                }

                //after

                //ViewBag.XbooxKey = MemberKey;
                return Member;
            }

            //return RedirectToAction()
           
            //return View(ViewBag.Key);
        }


    }
}