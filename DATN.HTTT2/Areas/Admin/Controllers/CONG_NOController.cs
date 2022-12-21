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
    public class CONG_NOController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/CONG_NO
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.CONG_NO.ToList());
            }
        }

        // GET: Admin/CONG_NO/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONG_NO cONG_NO = db.CONG_NO.Find(id);
            if (cONG_NO == null)
            {
                return HttpNotFound();
            }
            return View(cONG_NO);
        }

        // GET: Admin/CONG_NO/Create
        public ActionResult Create()
        {
            ViewBag.MaDL = new SelectList(db.DAI_LY, "MaDL", "Ten");
            return View();
        }

        // POST: Admin/CONG_NO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCN,MaDL,GhiChu")] CONG_NO cONG_NO)
        {
            if (ModelState.IsValid)
            {
                db.CONG_NO.Add(cONG_NO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDL = new SelectList(db.DAI_LY, "MaDL", "Ten", cONG_NO.MaDL);
            return View(cONG_NO);
        }

        // GET: Admin/CONG_NO/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONG_NO cONG_NO = db.CONG_NO.Find(id);
            if (cONG_NO == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDL = new SelectList(db.DAI_LY, "MaDL", "Ten", cONG_NO.MaDL);
            return View(cONG_NO);
        }

        // POST: Admin/CONG_NO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCN,MaDL,GhiChu")] CONG_NO cONG_NO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cONG_NO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDL = new SelectList(db.DAI_LY, "MaDL", "Ten", cONG_NO.MaDL);
            return View(cONG_NO);
        }

        // GET: Admin/CONG_NO/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONG_NO cONG_NO = db.CONG_NO.Find(id);
            if (cONG_NO == null)
            {
                return HttpNotFound();
            }
            return View(cONG_NO);
        }

        // POST: Admin/CONG_NO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CONG_NO cONG_NO = db.CONG_NO.Find(id);
            db.CONG_NO.Remove(cONG_NO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete1(string id)
        {
            CONG_NO hANG_HOA = db.CONG_NO.Find(id);
            
            db.CONG_NO.Remove(hANG_HOA);
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
