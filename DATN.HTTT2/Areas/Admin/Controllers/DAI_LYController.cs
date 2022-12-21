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
    public class DAI_LYController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/DAI_LY
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.DAI_LY.ToList());
            }
        }

        // GET: Admin/DAI_LY/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAI_LY dAI_LY = db.DAI_LY.Find(id);
            if (dAI_LY == null)
            {
                return HttpNotFound();
            }
            return View(dAI_LY);
        }

        // GET: Admin/DAI_LY/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DAI_LY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDL,Ten,DiaChi,SoDienThoai")] DAI_LY dAI_LY)
        {
            if (ModelState.IsValid)
            {
                db.DAI_LY.Add(dAI_LY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dAI_LY);
        }

        // GET: Admin/DAI_LY/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAI_LY dAI_LY = db.DAI_LY.Find(id);
            if (dAI_LY == null)
            {
                return HttpNotFound();
            }
            return View(dAI_LY);
        }

        // POST: Admin/DAI_LY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDL,Ten,DiaChi,SoDienThoai")] DAI_LY dAI_LY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dAI_LY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dAI_LY);
        }

        // GET: Admin/DAI_LY/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAI_LY dAI_LY = db.DAI_LY.Find(id);
            if (dAI_LY == null)
            {
                return HttpNotFound();
            }
            return View(dAI_LY);
        }

        // POST: Admin/DAI_LY/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DAI_LY dAI_LY = db.DAI_LY.Find(id);
            db.DAI_LY.Remove(dAI_LY);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete1(string id)
        {
            DAI_LY hANG_HOA = db.DAI_LY.Find(id);
            List<CONG_NO> tk = (from n in db.CONG_NO
                                where n.MaDL.Contains(id)
                                select n).ToList();
            foreach (var item in tk)
            {
                db.CONG_NO.Remove(item);
            }
            db.DAI_LY.Remove(hANG_HOA);
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
