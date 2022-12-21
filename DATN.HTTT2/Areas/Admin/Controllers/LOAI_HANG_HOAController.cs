using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DATN.HTTT2.Areas.Admin.Models;

namespace DATN.HTTT2.Areas.Admin.Controllers
{
    public class LOAI_HANG_HOAController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/LOAI_HANG_HOA
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.LOAI_HANG_HOA.ToList());
            }
        }

        // GET: Admin/LOAI_HANG_HOA/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_HANG_HOA lOAI_HANG_HOA = db.LOAI_HANG_HOA.Find(id);
            if (lOAI_HANG_HOA == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_HANG_HOA);
        }

        // GET: Admin/LOAI_HANG_HOA/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LOAI_HANG_HOA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoai,TenLoai")] LOAI_HANG_HOA lOAI_HANG_HOA)
        {
            if (ModelState.IsValid)
            {
                db.LOAI_HANG_HOA.Add(lOAI_HANG_HOA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lOAI_HANG_HOA);
        }

        // GET: Admin/LOAI_HANG_HOA/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_HANG_HOA lOAI_HANG_HOA = db.LOAI_HANG_HOA.Find(id);
            if (lOAI_HANG_HOA == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_HANG_HOA);
        }

        // POST: Admin/LOAI_HANG_HOA/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoai,TenLoai")] LOAI_HANG_HOA lOAI_HANG_HOA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOAI_HANG_HOA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOAI_HANG_HOA);
        }

        // GET: Admin/LOAI_HANG_HOA/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_HANG_HOA lOAI_HANG_HOA = db.LOAI_HANG_HOA.Find(id);
            if (lOAI_HANG_HOA == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_HANG_HOA);
        }

        // POST: Admin/LOAI_HANG_HOA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LOAI_HANG_HOA lOAI_HANG_HOA = db.LOAI_HANG_HOA.Find(id);
            db.LOAI_HANG_HOA.Remove(lOAI_HANG_HOA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete1(string id)
        {
            LOAI_HANG_HOA hANG_HOA = db.LOAI_HANG_HOA.Find(id);
            db.LOAI_HANG_HOA.Remove(hANG_HOA);
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
