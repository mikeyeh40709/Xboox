using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XbooxCMS.Models;

namespace XbooxCMS.Controllers
{
    public class ProductImgsController : Controller
    {
        private XbooxContext db = new XbooxContext();

        // GET: ProductImgs
        public ActionResult Index()
        {
            return View(db.ProductImgs.ToList());
        }

        // GET: ProductImgs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImgs productImgs = db.ProductImgs.Find(id);
            if (productImgs == null)
            {
                return HttpNotFound();
            }
            return View(productImgs);
        }

        // GET: ProductImgs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductImgs/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductImgId,ProductId,imgLink")] ProductImgs productImgs)
        {
            if (ModelState.IsValid)
            {
                db.ProductImgs.Add(productImgs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productImgs);
        }

        // GET: ProductImgs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImgs productImgs = db.ProductImgs.Find(id);
            if (productImgs == null)
            {
                return HttpNotFound();
            }
            return View(productImgs);
        }

        // POST: ProductImgs/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductImgId,ProductId,imgLink")] ProductImgs productImgs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImgs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productImgs);
        }

        // GET: ProductImgs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImgs productImgs = db.ProductImgs.Find(id);
            if (productImgs == null)
            {
                return HttpNotFound();
            }
            return View(productImgs);
        }

        // POST: ProductImgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProductImgs productImgs = db.ProductImgs.Find(id);
            db.ProductImgs.Remove(productImgs);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
