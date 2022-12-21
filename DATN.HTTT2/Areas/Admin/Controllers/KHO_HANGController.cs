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
    public class KHO_HANGController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/KHO_HANG
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.KHO_HANG.ToList());
            }
        }

        // GET: Admin/KHO_HANG/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHO_HANG kHO_HANG = db.KHO_HANG.Find(id);
            if (kHO_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHO_HANG);
        }

        // GET: Admin/KHO_HANG/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KHO_HANG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,DiaChi")] KHO_HANG kHO_HANG)
        {
            if (ModelState.IsValid)
            {
                db.KHO_HANG.Add(kHO_HANG);
                List<HANG_HOA> ds = (from n in db.HANG_HOA
                                     select n).ToList();
                foreach (var item in ds)
                {
                    Ton_kho tk = new Ton_kho();
                    tk.MaHH = item.MaHH;
                    tk.MaKH = kHO_HANG.MaKH;
                    tk.SoLuong = 0;
                    db.Ton_kho.Add(tk);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kHO_HANG);
        }

        // GET: Admin/KHO_HANG/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHO_HANG kHO_HANG = db.KHO_HANG.Find(id);
            if (kHO_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHO_HANG);
        }

        // POST: Admin/KHO_HANG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,DiaChi")] KHO_HANG kHO_HANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHO_HANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHO_HANG);
        }

        // GET: Admin/KHO_HANG/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHO_HANG kHO_HANG = db.KHO_HANG.Find(id);
            if (kHO_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHO_HANG);
        }

        // POST: Admin/KHO_HANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KHO_HANG kHO_HANG = db.KHO_HANG.Find(id);
            db.KHO_HANG.Remove(kHO_HANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete1(string id)
        {
            KHO_HANG hANG_HOA = db.KHO_HANG.Find(id);
            List<Ton_kho> ds = (from n in db.Ton_kho
                                where n.MaKH.Contains(id)
                                select n).ToList();
            foreach (var item in ds)
            {
                db.Ton_kho.Remove(item);
            }
            db.KHO_HANG.Remove(hANG_HOA);
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
