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
    public class HANG_HOAController : Controller
    {
        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/HANG_HOA
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                var hANG_HOA = db.HANG_HOA.Include(h => h.LOAI_HANG_HOA).Include(h => h.NHA_CUNG_CAP);
                return View(hANG_HOA.ToList());
            }
        }

        // GET: Admin/HANG_HOA/Create
        public ActionResult Create()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                ViewBag.MaLoai = new SelectList(db.LOAI_HANG_HOA, "MaLoai", "TenLoai");
                ViewBag.MaNCC = new SelectList(db.NHA_CUNG_CAP, "MaNCC", "TenNCC");
                return View();
            }
        }

        // POST: Admin/HANG_HOA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHH,TenHangHoa,DonViTinh,DonGia,MaLoai,MaNCC,TonKhoDuKien,DinhMucTK")] HANG_HOA hANG_HOA)
        {
            if (ModelState.IsValid)
            {
                hANG_HOA.TonKhoDuKien = 0;
                db.HANG_HOA.Add(hANG_HOA);
                try
                {
                    List<KHO_HANG> kh = (from n in db.KHO_HANG
                                         select n).ToList();
                    foreach (var item in kh)
                    {
                        Ton_kho tk = new Ton_kho();
                        tk.MaHH = hANG_HOA.MaHH;
                        tk.MaKH = item.MaKH;
                        tk.SoLuong = 0;
                        db.Ton_kho.Add(tk);
                    }
                }
                catch (Exception)
                {
                    ViewBag.Message = "Lỗi hệ thống";
                    return View();
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LOAI_HANG_HOA, "MaLoai", "TenLoai", hANG_HOA.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NHA_CUNG_CAP, "MaNCC", "TenNCC", hANG_HOA.MaNCC);
            return View(hANG_HOA);
        }

        // GET: Admin/HANG_HOA/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HANG_HOA hANG_HOA = db.HANG_HOA.Find(id);
                if (hANG_HOA == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MaLoai = new SelectList(db.LOAI_HANG_HOA, "MaLoai", "TenLoai", hANG_HOA.MaLoai);
                ViewBag.MaNCC = new SelectList(db.NHA_CUNG_CAP, "MaNCC", "TenNCC", hANG_HOA.MaNCC);
                return View(hANG_HOA);
            }
        }

        // POST: Admin/HANG_HOA/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHH,TenHangHoa,DonViTinh,DonGia,MaLoai,MaNCC,TonKhoDuKien,DinhMucTK")] HANG_HOA hANG_HOA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hANG_HOA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.LOAI_HANG_HOA, "MaLoai", "TenLoai", hANG_HOA.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NHA_CUNG_CAP, "MaNCC", "TenNCC", hANG_HOA.MaNCC);
            return View(hANG_HOA);
        }

        // GET: Admin/HANG_HOA/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HANG_HOA hANG_HOA = db.HANG_HOA.Find(id);
            if (hANG_HOA == null)
            {
                return HttpNotFound();
            }
            return View(hANG_HOA);
        }

        // POST: Admin/HANG_HOA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HANG_HOA hANG_HOA = db.HANG_HOA.Find(id);
            db.HANG_HOA.Remove(hANG_HOA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete1(string id)
        {
            HANG_HOA hANG_HOA = db.HANG_HOA.Find(id);
            List<Ton_kho> tk = (from n in db.Ton_kho
                                where n.MaHH.Contains(id)
                                select n).ToList();
            foreach (var item in tk)
            {
                db.Ton_kho.Remove(item);
            }
            db.HANG_HOA.Remove(hANG_HOA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TonKho()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                var tk = db.Ton_kho.ToList();
                return View(tk.ToList());
            }
        }

        public ActionResult TonKhoDK()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                var tk = from n in db.Ton_kho
                         select new { n.MaHH, n.SoLuong };

                var aa = from p in tk
                         group p by p.MaHH into g
                         select new { MaHH = g.Key, SoLuong = g.Sum(a => a.SoLuong) };

                List<TonKhoDK> ls = new List<TonKhoDK>();
                foreach (var item in aa)
                {
                    var kk = (from n in db.HANG_HOA where n.MaHH.Contains(item.MaHH) select n).FirstOrDefault();
                    TonKhoDK ss = new TonKhoDK();
                    ss.tenhh = kk.TenHangHoa;
                    ss.soluong = item.SoLuong;
                    ss.tonkhodk = kk.TonKhoDuKien;
                    ls.Add(ss);
                }
                return View(ls);
            }
        }

        //Cập nhật tồn kho dự kiến
        [HttpPost]
        public ActionResult CNTKDK()
        {
            var tk = from n in db.Ton_kho
                     select new { n.MaHH, n.SoLuong };

            var aa = from p in tk
                     group p by p.MaHH into g
                     select new { MaHH = g.Key, SoLuong = g.Sum(a => a.SoLuong) };

            List<TonKhoDK> ls = new List<TonKhoDK>();
            foreach (var item in aa)
            {
                var kk = (from n in db.HANG_HOA where n.MaHH.Contains(item.MaHH) select n).FirstOrDefault();
                kk.TonKhoDuKien = item.SoLuong;
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("TonKhoDK");
            }
            return RedirectToAction("TonKhoDK");
        }

        public ActionResult khongquyen()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
                return View();
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
