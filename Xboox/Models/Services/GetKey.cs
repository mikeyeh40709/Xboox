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
     
        public string GetAllKey(HttpContextBase contextBase)
        {

            if (!contextBase.User.Identity.IsAuthenticated)
            {
                if (contextBase.Request.Cookies["VisitorKey"] == null)
                {
                    Guid VisitorKey = Guid.NewGuid();                   
                    return VisitorKey.ToString();
                }
                else
                {
                    return contextBase.Request.Cookies["VisitorKey"].Value;
                }   
            }
            else
            {              
                var Member = contextBase.User.Identity.GetUserId();

                return Member;
            }
        }


    }


    public class TempCartItems
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }

    }
}