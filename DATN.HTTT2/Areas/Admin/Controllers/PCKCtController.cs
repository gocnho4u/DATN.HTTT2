using DATN.HTTT2.Areas.Admin.Models;
using DATN.HTTT2.Areas.Admin.Models.ViewModels;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATN.HTTT2.Areas.Admin.Controllers
{
    public class PCKCtController : Controller
    {
        public string timMaHH(string key)
        {
            string mahh = (from n in db.HANG_HOA
                           where n.TenHangHoa.Contains(key)
                           select n.MaHH).FirstOrDefault();
            return mahh;
        }

        private HTTTEntities db = new HTTTEntities();
        // GET: Admin/PCKCt
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.PHIEU_CHUYEN_KHO.OrderByDescending(m=>m.NgayChuyen).ToList());
            }
        }

        // GET: Admin/PCKCt/Details/5
        public ActionResult Details(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                XemPCK xemct = new XemPCK();
                PHIEU_CHUYEN_KHO pck = db.PHIEU_CHUYEN_KHO.Find(id);
                List<CHI_TIET_PCK> ct = (from n in db.CHI_TIET_PCK
                                         where n.MaPCK.Contains(id)
                                         select n).ToList();
                xemct.PHIEU_CHUYEN_KHO = pck;
                xemct.cHI_TIET_PCKs = ct;
                return View(xemct);
            }
        }

        // GET: Admin/PCKCt/Create
        public ActionResult Create()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                int dem = (from n in db.PHIEU_CHUYEN_KHO
                           select n).Count() + 1;
                ViewBag.MaDH = "PCK" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ViewBag.MaKHN = new SelectList(db.KHO_HANG, "MaKH", "TenKH");
                ViewBag.MaKHX = new SelectList(db.KHO_HANG, "MaKH", "TenKH");
                return View();
            }
        }

        // POST: Admin/PCKCt/Create
        [HttpPost]
        public JsonResult Create(PCKCt pCK)
        {
            NHAN_VIEN nv = (NHAN_VIEN)Session["Nhanvien"];
            bool status = true;

            var isValidModel = TryUpdateModel(pCK);

            if (isValidModel)
            {
                using (HTTTEntities db = new HTTTEntities())
                {
                    int dem = (from n in db.PHIEU_CHUYEN_KHO
                               select n).Count() + 1;
                    PHIEU_CHUYEN_KHO pc = new PHIEU_CHUYEN_KHO()
                    {
                        MaPCK = "PCK" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString(),
                        NgayChuyen = DateTime.Now,
                        MaNV=nv.MaNV,
                        MaKHN= pCK.MaKHN,
                        MaKHX=pCK.MaKHX,

                    };
                    var mack = pc.MaPCK;
                    db.PHIEU_CHUYEN_KHO.Add(pc);

                    if (db.SaveChanges() > 0)
                    {
                        
                        int count = 1;
                        string makh = db.KHO_HANG.Min(o => o.MaKH);

                        foreach (var item in pCK.ctck)
                        {
                            CHI_TIET_PCK ct = new CHI_TIET_PCK()
                            {
                                MaPCK = mack,
                                STT = count,
                                MaHH = timMaHH(item.hanghoa),
                                SoLuong=item.SoLuong,
                                
                            };
                            db.CHI_TIET_PCK.Add(ct);
                            Ton_kho tk = (from n in db.Ton_kho
                                          where n.MaHH.Contains(ct.MaHH) && n.MaKH.Contains(pCK.MaKHX)
                                          select n).FirstOrDefault();
                            tk.SoLuong -= ct.SoLuong;
                            Ton_kho tk1 = (from n in db.Ton_kho
                                          where n.MaHH.Contains(ct.MaHH) && n.MaKH.Contains(pCK.MaKHN)
                                          select n).FirstOrDefault();
                            tk1.SoLuong += ct.SoLuong;
                            count++;
                        }

                        if (db.SaveChanges() > 0)
                        {
                            return new JsonResult { Data = new { status = status, message = "Lưu thành công" } };
                        }
                    }
                }
            }

            status = false;
            return new JsonResult { Data = new { status = status, message = "Lỗi !" } };
        }

        // GET: Admin/PCKCt/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/PCKCt/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/PCKCt/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/PCKCt/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete1(string id, FormCollection collection)
        {
            try
            {
                List<CHI_TIET_PCK> ds = (from n in db.CHI_TIET_PCK
                                         where n.MaPCK.Contains(id)
                                         select n).ToList();
                PHIEU_CHUYEN_KHO pCK = (from n in db.PHIEU_CHUYEN_KHO
                                   where n.MaPCK.Contains(id)
                                   select n).FirstOrDefault();

                foreach (var item in ds)
                {

                    Ton_kho tk = (from n in db.Ton_kho
                                  where n.MaHH.Contains(item.MaHH) && n.MaKH.Contains(pCK.MaKHX)
                                  select n).FirstOrDefault();
                    tk.SoLuong += item.SoLuong;
                    Ton_kho tk1 = (from n in db.Ton_kho
                                   where n.MaHH.Contains(item.MaHH) && n.MaKH.Contains(pCK.MaKHN)
                                   select n).FirstOrDefault();
                    tk1.SoLuong -= item.SoLuong;

                    db.CHI_TIET_PCK.Remove(item);
                }

                db.PHIEU_CHUYEN_KHO.Remove(pCK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details1(string id)
        {
            XemPCK xemct = new XemPCK();
            PHIEU_CHUYEN_KHO ddh = db.PHIEU_CHUYEN_KHO.Find(id);
            List<CHI_TIET_PCK> ct = (from n in db.CHI_TIET_PCK
                                     where n.MaPCK.Contains(id)
                                     select n).ToList();
            xemct.PHIEU_CHUYEN_KHO = ddh;
            xemct.cHI_TIET_PCKs = ct;
            return View(xemct);
        }

        public ActionResult Print(string id)
        {
            return new ActionAsPdf("Details1", new { id = id });
        }
    }
}
