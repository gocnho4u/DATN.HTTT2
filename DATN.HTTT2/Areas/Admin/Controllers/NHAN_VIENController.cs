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
    public class NHAN_VIENController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/NHAN_VIEN
        public ActionResult Index()
        {
            return RedirectToAction("Index", "DDHCt");
        }

        // GET: Admin/NHAN_VIEN/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAN_VIEN nHAN_VIEN = db.NHAN_VIEN.Find(id);
            if (nHAN_VIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHAN_VIEN);
        }

        // GET: Admin/NHAN_VIEN/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NHAN_VIEN/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,HoTen,SoDienThoai,DiaChi,QuyenQL,MatKhau")] NHAN_VIEN nHAN_VIEN)
        {
            if (ModelState.IsValid)
            {
                db.NHAN_VIEN.Add(nHAN_VIEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nHAN_VIEN);
        }

        // GET: Admin/NHAN_VIEN/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAN_VIEN nHAN_VIEN = db.NHAN_VIEN.Find(id);
            if (nHAN_VIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHAN_VIEN);
        }

        // POST: Admin/NHAN_VIEN/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,HoTen,SoDienThoai,DiaChi,QuyenQL,MatKhau")] NHAN_VIEN nHAN_VIEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nHAN_VIEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nHAN_VIEN);
        }

        // GET: Admin/NHAN_VIEN/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAN_VIEN nHAN_VIEN = db.NHAN_VIEN.Find(id);
            if (nHAN_VIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHAN_VIEN);
        }

        // POST: Admin/NHAN_VIEN/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NHAN_VIEN nHAN_VIEN = db.NHAN_VIEN.Find(id);
            db.NHAN_VIEN.Remove(nHAN_VIEN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DoiMK(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                NHAN_VIEN dl = (NHAN_VIEN)Session["Nhanvien"];
                id = dl.MaNV;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NHAN_VIEN dAI_LY = db.NHAN_VIEN.Find(id);
                if (dAI_LY == null)
                {
                    return HttpNotFound();
                }
                return View(dAI_LY);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMK([Bind(Include = "MaNV,HoTen,SoDienThoai,DiaChi,QuyenQL,MatKhau")] NHAN_VIEN nHAN_VIEN)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(nHAN_VIEN).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index","DDHCt");
                }
                return View();
            }
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
