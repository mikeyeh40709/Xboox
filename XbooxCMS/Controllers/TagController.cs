using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;

namespace XbooxCMS.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        private XbooxContext context;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTag(Tags tags)
        {

            if (ModelState.IsValid)
            {
                //var viewModel = new TagViewModel()
                //{
                //    TagId = Guid.NewGuid(),
                //    TagName 
                //}

                tags.TagId = Guid.NewGuid();
                context.Tags.Add(tags);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
               
                return View();

            }


        }
    }
}