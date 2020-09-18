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
using XbooxLibrary.Services;
using XbooxCMS.ViewModels;


namespace XbooxCMS.Controllers
{
    public class TagsController : Controller
    {
        //private XbooxContext context = new XbooxContext();
        //private XbooxLibraryDBContext context = new XbooxLibraryDBContext();
        // GET: Tags
        public ActionResult Index()
        {
            //TagService service = new TagService();
            return View();
        }

        // GET: Tags/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagService service = new TagService();
            var tags =  service.GetSingleTag((Guid)id);
           // Tags tags = context.Tags.Find(id);
            if (tags == null)
            {
                return HttpNotFound();
            }
            return View(tags);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tags tags)
        {
            if (ModelState.IsValid)
            {
                TagService service = new TagService();
                service.Create(tags);

                //tags.TagId = Guid.NewGuid();
                //context.Tags.Add(tags);
                //context.SaveChanges();

                return RedirectToAction("Index", "Tags");
            }
            else
            {

                return View();

            }
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagService service = new TagService();
            var tags = service.GetSingleTag((Guid)id);
            //Tags tags = context.Tags.Find(id);
            if (tags == null)
            {
                return HttpNotFound();
            }
            return View(tags);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tags tags)
        {
            if (ModelState.IsValid)
            {
                TagService service = new TagService();
                service.Edit(tags);
                //context.Entry(tags).State = EntityState.Modified;
                //context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tags);
        }

        // GET: Tags/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Tags tags = context.Tags.Find(id);
        //    if (tags == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tags);
        //}

        //// POST: Tags/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Tags tags = context.Tags.Find(id);
        //    context.Tags.Remove(tags);
        //    context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
