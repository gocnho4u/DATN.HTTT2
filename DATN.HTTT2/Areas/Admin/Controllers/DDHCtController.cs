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
    public class DDHCtController : Controller
    {
        public string timMaNV(string key)
        {
            string manv = (from n in db.NHAN_VIEN
                           where n.HoTen.Contains(key)
                           select n.MaNV).ToString();
            return manv;
        }

        public string timMaDL(string key)
        {
            string madl = (from n in db.DAI_LY
                           where n.Ten.Contains(key)
                           select n.MaDL).FirstOrDefault();
            return madl;
        }

        public string timMaHH(string key)
        {
            string mahh = (from n in db.HANG_HOA
                           where n.TenHangHoa.Contains(key)
                           select n.MaHH).FirstOrDefault();
            return mahh;
        }

       

        private HTTTEntities db = new HTTTEntities();

        // GET: Admin/DDHCt
        public ActionResult Index()
        {
            if (Session["Nhanvien"]==null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.DON_DAT_HANG.OrderByDescending(m=>m.NgayDat));
            }
           
        }

        // GET: Admin/DDHCt/Details/5
        public ActionResult Details(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            { 
            XemDDH xemct = new XemDDH();
            DON_DAT_HANG ddh = db.DON_DAT_HANG.Find(id);
            List<CHI_TIET_DDH> ct = (from n in db.CHI_TIET_DDH
                                     where n.MaDDH.Contains(id)
                                     select n).ToList();
            double tongtien = 0;
            foreach (var item in ct)
            {
                tongtien += item.TongTien;
            }
            ViewBag.TongTien = tongtien;
            xemct.DON_DAT_HANG = ddh;
            xemct.cHI_TIET_DDHs = ct;
            return View(xemct);
            }
        }

        // GET: Admin/DDHCt/Create
        public ActionResult Create()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                int dem = (from n in db.DON_DAT_HANG
                           select n).Count() + 1;
                ViewBag.MaDH = "DDH" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                return View();
            }
        }

        // POST: Admin/DDHCt/Create
        [HttpPost]
        public JsonResult Create(DDHCt dt)
        {
            NHAN_VIEN nv = (NHAN_VIEN)Session["Nhanvien"];
            bool status = true;

            var isValidModel = TryUpdateModel(dt);

            if (isValidModel)
            {
                using (HTTTEntities db = new HTTTEntities())
                {
                    string madh = "";
                    int dem = (from n in db.DON_DAT_HANG
                               select n).Count() + 1;
                    DON_DAT_HANG ddh = new DON_DAT_HANG()
                    { 
                        MaDDH = "DDH" +dem+ DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString(),
                        NgayDat = DateTime.Now,
                        DiaChi = dt.diachi,
                        GiaoHang = "Chưa giao",
                        MaNV = nv.MaNV,
                        MaDL = timMaDL(dt.daily),
                        
                    };
                    madh = ddh.MaDDH;
                    db.DON_DAT_HANG.Add(ddh);

                    if (db.SaveChanges() > 0)
                    {
                       
                        int count = 1;
                        string makh = db.KHO_HANG.Min(o => o.MaKH);

                        foreach (var item in dt.danhsachct)
                        {
                            CHI_TIET_DDH ct = new CHI_TIET_DDH()
                            {
                                MaDDH = madh,
                                STT=count,
                                SoLuong=item.soluong,
                                ChietKhau=item.chietkhau,
                                TongTien=item.thanhtien,
                                GhiChu=item.ghichu,
                                MaHH=timMaHH(item.hanghoa),
                            };
                            db.CHI_TIET_DDH.Add(ct);
                            HANG_HOA hh = (from n in db.HANG_HOA
                                           where n.MaHH.Contains(ct.MaHH)
                                           select n).FirstOrDefault();
                            hh.TonKhoDuKien -= ct.SoLuong;
                            //Ton_kho tk = (from n in db.Ton_kho
                            //              where n.MaHH.Contains(hh.MaHH) && n.MaKH.Contains(makh)
                            //              select n).FirstOrDefault();
                            //tk.SoLuong -= ct.SoLuong;
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

        // GET: Admin/DDHCt/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                DDHCt ddh = new DDHCt();
                DON_DAT_HANG dh = (from n in db.DON_DAT_HANG
                                   where n.MaDDH.Contains(id)
                                   select n).FirstOrDefault();
                List<CHI_TIET_DDH> ct = (from n in db.CHI_TIET_DDH
                                         where n.MaDDH.Contains(id)
                                         select n).ToList();
                List<listchitiet> dsct = new List<listchitiet>();
                double tongtien = 0;
                foreach (var item in ct)
                {
                    listchitiet c = new listchitiet();
                    c.hanghoa = item.HANG_HOA.TenHangHoa;
                    c.soluong = item.SoLuong;
                    c.chietkhau = item.ChietKhau;
                    c.thanhtien = item.TongTien;
                    c.ghichu = item.GhiChu;
                    c.dvt = item.HANG_HOA.DonViTinh;
                    c.dongia = item.HANG_HOA.DonGia;
                    dsct.Add(c);
                    tongtien += item.TongTien;
                }
                ddh.madh = id;
                ddh.daily = dh.DAI_LY.Ten;
                ddh.diachi = dh.DiaChi;
                ddh.danhsachct = dsct;
                ViewBag.TongTien = tongtien;
                return View(ddh);
            }
        }

        // POST: Admin/DDHCt/Edit/5
        [HttpPost]
        public JsonResult Edit(DDHCt dt)
        {
            bool status = true;

            var isValidModel = TryUpdateModel(dt);

            if (isValidModel)
            {
                using (HTTTEntities db = new HTTTEntities())
                {
                    List<CHI_TIET_DDH> dsctx = (from n in db.CHI_TIET_DDH
                                                where n.MaDDH.Contains(dt.madh)
                                                select n).ToList();

                    foreach (var item in dsctx)
                    {
                        HANG_HOA hh = (from n in db.HANG_HOA
                                       where n.MaHH.Contains(item.MaHH)
                                       select n).FirstOrDefault();
                        hh.TonKhoDuKien += item.SoLuong;
                        db.CHI_TIET_DDH.Remove(item);
                    }
                    int count = 1;

                        foreach (var item in dt.danhsachct)
                        {
                            CHI_TIET_DDH ct = new CHI_TIET_DDH()
                            {
                                MaDDH = dt.madh,
                                STT = count,
                                SoLuong = item.soluong,
                                ChietKhau = item.chietkhau,
                                TongTien = item.thanhtien,
                                GhiChu = item.ghichu,
                                MaHH = timMaHH(item.hanghoa),
                            };
                            db.CHI_TIET_DDH.Add(ct);
                            HANG_HOA hh = (from n in db.HANG_HOA
                                           where n.MaHH.Contains(ct.MaHH)
                                           select n).FirstOrDefault();
                            hh.TonKhoDuKien -= ct.SoLuong;
                            //Ton_kho tk = (from n in db.Ton_kho
                            //              where n.MaHH.Contains(hh.MaHH) && n.MaKH.Contains(makh)
                            //              select n).FirstOrDefault();
                            //tk.SoLuong -= ct.SoLuong;
                            count++;
                        }

                        if (db.SaveChanges() > 0)
                        {
                            return new JsonResult { Data = new { status = status, message = "Lưu thành công" } };
                        }
                    }
                }
            status = false;
            return new JsonResult { Data = new { status = status, message = "Lỗi !" } };
        }

        // GET: Admin/DDHCt/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Admin/DDHCt/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                         where n.MaDDH.Contains(id)
                                         select n).ToList();              
                DON_DAT_HANG dh = (from n in db.DON_DAT_HANG
                                   where n.MaDDH.Contains(id)
                                   select n).FirstOrDefault();

                    string makh = db.KHO_HANG.Min(o => o.MaKH);
                    foreach (var item in ds)
                    {
                        HANG_HOA hh = (from n in db.HANG_HOA
                                       where n.MaHH.Contains(item.MaHH)
                                       select n).FirstOrDefault();
                        hh.TonKhoDuKien += item.SoLuong;
                        if (dh.GiaoHang=="Đã giao")
                        {
                            Ton_kho tk = (from n in db.Ton_kho
                                          where n.MaHH.Contains(hh.MaHH) && n.MaKH.Contains(makh)
                                          select n).FirstOrDefault();
                            tk.SoLuong += item.SoLuong;
                        }
                        db.CHI_TIET_DDH.Remove(item);
                    }

                db.DON_DAT_HANG.Remove(dh);
                db.SaveChanges();
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
                List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                         where n.MaDDH.Contains(id)
                                         select n).ToList();
                DON_DAT_HANG dh = (from n in db.DON_DAT_HANG
                                   where n.MaDDH.Contains(id)
                                   select n).FirstOrDefault();

                string makh = db.KHO_HANG.Min(o => o.MaKH);
                foreach (var item in ds)
                {
                    HANG_HOA hh = (from n in db.HANG_HOA
                                   where n.MaHH.Contains(item.MaHH)
                                   select n).FirstOrDefault();
                    hh.TonKhoDuKien += item.SoLuong;
                    if (dh.GiaoHang == "Đã giao")
                    {
                        Ton_kho tk = (from n in db.Ton_kho
                                      where n.MaHH.Contains(hh.MaHH) && n.MaKH.Contains(makh)
                                      select n).FirstOrDefault();
                        tk.SoLuong += item.SoLuong;
                    }

                    db.CHI_TIET_DDH.Remove(item);
                }

                db.DON_DAT_HANG.Remove(dh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public string DoiTinhTrang(string id)
        {
            var user = db.DON_DAT_HANG.Find(id);
            if (user.GiaoHang == "Đã giao")
            {
                user.GiaoHang = "Chưa giao";
            }
            else if(user.GiaoHang == "Chưa giao")
            {
                user.GiaoHang = "Đang giao";
            }
            else
            {
                user.GiaoHang = "Đã giao";
            }
            //user.GiaoHang = !user.GiaoHang;
            

            List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                     where n.MaDDH.Contains(id)
                                     select n).ToList();

            string makh = db.KHO_HANG.Min(o => o.MaKH);
            foreach (var item in ds)
            {
                HANG_HOA hh = (from n in db.HANG_HOA
                               where n.MaHH.Contains(item.MaHH)
                               select n).FirstOrDefault();

                Ton_kho tk = (from n in db.Ton_kho
                              where n.MaHH.Contains(hh.MaHH) && n.MaKH.Contains(makh)
                              select n).FirstOrDefault();
                if (user.GiaoHang == "Đang giao")
                {
                    tk.SoLuong -= item.SoLuong;
                }
                else if (user.GiaoHang == "Chưa giao")
                {
                    tk.SoLuong += item.SoLuong;
                }
                else continue;
            }
            db.SaveChanges();
            return user.GiaoHang;
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = DoiTinhTrang(id);
            return Json(new
            {
                status = result
            });
        }

        public ActionResult Details1(string id)
        {
            XemDDH xemct = new XemDDH();
            DON_DAT_HANG ddh = db.DON_DAT_HANG.Find(id);
            List<CHI_TIET_DDH> ct = (from n in db.CHI_TIET_DDH
                                     where n.MaDDH.Contains(id)
                                     select n).ToList();
            double tongtien = 0;
            foreach (var item in ct)
            {
                tongtien += item.TongTien;
            }
            ViewBag.TongTien = tongtien;
            xemct.DON_DAT_HANG = ddh;
            xemct.cHI_TIET_DDHs = ct;
            return View(xemct);
        }

        public ActionResult Print(string id)
        {
            return new ActionAsPdf("Details1", new { id = id });
        }
        
    }
}
