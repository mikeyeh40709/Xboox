using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xboox.Models.Services
{
    public  class GetKey
    {
        //private static HttpContextBase contextBase;
        public string GetAllKey(HttpContextBase contextBase)
        {

            if (!contextBase.User.Identity.IsAuthenticated)
            {
                Guid VisitorKey = Guid.NewGuid();
                //ViewBag.XbooxKey = VisitorKey;
                return VisitorKey.ToString();
            }
            else
            {
                var MemberKey = contextBase.User.Identity.GetUserId();
                //ViewBag.XbooxKey = MemberKey;
                return MemberKey;
            }

            //return RedirectToAction()
           
            //return View(ViewBag.Key);
        }


    }
}