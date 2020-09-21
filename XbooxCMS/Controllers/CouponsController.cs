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
    public class CouponsController : Controller
    {
        private XbooxContext db = new XbooxContext();

        // GET: Coupons
        public ActionResult Index()
        {
            return View(db.Coupons.ToList());
        }

        //// GET: Coupons/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Coupons coupons = db.Coupons.Find(id);
        //    if (coupons == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(coupons);
        //}

        //// GET: Coupons/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Coupons/Create
        //// 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        //// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,CouponName,Discount,CouponCode,StartTime,EndTime")] Coupons coupons)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        coupons.Id = Guid.NewGuid();
        //        db.Coupons.Add(coupons);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(coupons);
        //}

        //// GET: Coupons/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Coupons coupons = db.Coupons.Find(id);
        //    if (coupons == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(coupons);
        //}

        //// POST: Coupons/Edit/5
        //// 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        //// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,CouponName,Discount,CouponCode,StartTime,EndTime")] Coupons coupons)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(coupons).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(coupons);
        //}

        //// GET: Coupons/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Coupons coupons = db.Coupons.Find(id);
        //    if (coupons == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(coupons);
        //}

        //// POST: Coupons/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Coupons coupons = db.Coupons.Find(id);
        //    db.Coupons.Remove(coupons);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
