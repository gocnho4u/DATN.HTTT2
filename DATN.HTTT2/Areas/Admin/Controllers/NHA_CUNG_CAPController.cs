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
    public class NHA_CUNG_CAPController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/NHA_CUNG_CAP
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.NHA_CUNG_CAP.ToList());
            }
        }

        // GET: Admin/NHA_CUNG_CAP/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHA_CUNG_CAP nHA_CUNG_CAP = db.NHA_CUNG_CAP.Find(id);
            if (nHA_CUNG_CAP == null)
            {
                return HttpNotFound();
            }
            return View(nHA_CUNG_CAP);
        }

        // GET: Admin/NHA_CUNG_CAP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NHA_CUNG_CAP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNCC,TenNCC,DiaChi,SoDienThoai,Email")] NHA_CUNG_CAP nHA_CUNG_CAP)
        {
            if (ModelState.IsValid)
            {
                db.NHA_CUNG_CAP.Add(nHA_CUNG_CAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nHA_CUNG_CAP);
        }

        // GET: Admin/NHA_CUNG_CAP/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHA_CUNG_CAP nHA_CUNG_CAP = db.NHA_CUNG_CAP.Find(id);
            if (nHA_CUNG_CAP == null)
            {
                return HttpNotFound();
            }
            return View(nHA_CUNG_CAP);
        }

        // POST: Admin/NHA_CUNG_CAP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNCC,TenNCC,DiaChi,SoDienThoai,Email")] NHA_CUNG_CAP nHA_CUNG_CAP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nHA_CUNG_CAP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nHA_CUNG_CAP);
        }

        // GET: Admin/NHA_CUNG_CAP/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHA_CUNG_CAP nHA_CUNG_CAP = db.NHA_CUNG_CAP.Find(id);
            if (nHA_CUNG_CAP == null)
            {
                return HttpNotFound();
            }
            return View(nHA_CUNG_CAP);
        }

        // POST: Admin/NHA_CUNG_CAP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NHA_CUNG_CAP nHA_CUNG_CAP = db.NHA_CUNG_CAP.Find(id);
            db.NHA_CUNG_CAP.Remove(nHA_CUNG_CAP);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete1(string id)
        {
            NHA_CUNG_CAP hANG_HOA = db.NHA_CUNG_CAP.Find(id);

            db.NHA_CUNG_CAP.Remove(hANG_HOA);
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
