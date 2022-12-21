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
    public class PNHCtController : Controller
    {
        public string timMaNCC(string key)
        {
            string madl = (from n in db.NHA_CUNG_CAP
                           where n.TenNCC.Contains(key)
                           select n.MaNCC).FirstOrDefault();
            return madl;
        }

        public string timMaHH(string key)
        {
            string mahh = (from n in db.HANG_HOA
                           where n.TenHangHoa.Contains(key)
                           select n.MaHH).FirstOrDefault();
            return mahh;
        }
        public string timMaHHCC(string key)
        {
            string mahh = (from n in db.HANG_HOA
                           where n.TenHangHoa.Contains(key)
                           select n.MaNCC).FirstOrDefault();
            return mahh;
        }

        public void Gui(string data12,string mail)
        {
            GMailer.GmailUsername = "khanhchung53.k42@st.ueh.edu.vn";
            GMailer.GmailPassword = "1531998@";

            GMailer mailer = new GMailer();
            mailer.ToEmail = mail;
            mailer.Subject = "Phiếu nhập hàng - Công ty CNV";
            mailer.Body = data12;
            mailer.IsHtml = true;
            mailer.Send();
        }

        private HTTTEntities db = new HTTTEntities();
        // GET: Admin/PNHCt
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                NHAN_VIEN nv = (NHAN_VIEN)Session["Nhanvien"];
                ViewBag.Quyen = nv.QuyenQL;
                return View(db.PHIEU_NHAP_HANG.OrderByDescending(m=>m.NgayNhap));
            }
        }

        // GET: Admin/PNHCt/Details/5
        public ActionResult Details(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                XemPNH xemct = new XemPNH();
                PHIEU_NHAP_HANG ddh = db.PHIEU_NHAP_HANG.Find(id);
                List<CHI_TIET_PNH> ct = (from n in db.CHI_TIET_PNH
                                         where n.MaPNH.Contains(id)
                                         select n).ToList();
                xemct.PHIEU_NHAP_HANG = ddh;
                xemct.cHI_TIET_PNHs = ct;
                return View(xemct);
            }
        }

        // GET: Admin/PNHCt/Create
        public ActionResult Create()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                int dem = (from n in db.PHIEU_NHAP_HANG
                           select n).Count() + 1;
                ViewBag.MaPN = "PNH" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                return View();
            }
        }

        // POST: Admin/DDHCt/Create
        [HttpPost]
        public JsonResult Create(PNHCt dt)
        {
            bool status = true;

            var isValidModel = TryUpdateModel(dt);
            int loi = 0;
            foreach (var item in dt.danhsachct)
            {
                if (timMaHHCC(item.hanghoa)!= timMaNCC(dt.nhacc))
                {
                    loi++;
                }
            }
            if (loi==0)
            {
                if (isValidModel)
                {
                    using (HTTTEntities db = new HTTTEntities())
                    {
                        
                        int dem = (from n in db.PHIEU_NHAP_HANG
                                   select n).Count() + 1;
                        PHIEU_NHAP_HANG pnh = new PHIEU_NHAP_HANG()
                        {
                            MaPNH = "PNH" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString(),
                            NgayNhap = DateTime.Now,
                            NhanHang = false,
                            Gui = false,
                            MaNCC = timMaNCC(dt.nhacc),
                            MaNV = "KT001",
                        };
                        var mapn = pnh.MaPNH;
                        db.PHIEU_NHAP_HANG.Add(pnh);

                        if (db.SaveChanges() > 0)
                        {
                            int count = 1;
                            string makh = db.KHO_HANG.Min(o => o.MaKH);

                            foreach (var item in dt.danhsachct)
                            {

                                CHI_TIET_PNH ct = new CHI_TIET_PNH()
                                {
                                    MaPNH = mapn,
                                    STT = count,
                                    SoLuong = item.soluong,
                                    ChietKhau = 0,
                                    TongTien = item.thanhtien,
                                    GhiChu = item.ghichu,
                                    MaHH = timMaHH(item.hanghoa),
                                };
                                db.CHI_TIET_PNH.Add(ct);
                                HANG_HOA hh = (from n in db.HANG_HOA
                                               where n.MaHH.Contains(ct.MaHH)
                                               select n).FirstOrDefault();
                                //hh.TonKhoDuKien += ct.SoLuong;
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
                else
                {
                    status = false;
                    return new JsonResult { Data = new { status = status, message = "Lỗi !" } };
                }
            }
            else
            {
                status = false;
                return new JsonResult { Data = new { status = status, message = "Lỗi gọi nhầm hàng hóa của nhà cung cấp khác !" } };
            }
            status = false;
            return new JsonResult { Data = new { status = status, message = "Lỗi !" } };
        }

        [HttpPost]
        public JsonResult Create2(string mapn, string data12)
        {
            bool status = true;
            try
            {
                using (HTTTEntities db = new HTTTEntities())
                {
                    List<CHI_TIET_PNH> ds = (from n in db.CHI_TIET_PNH
                                             where n.MaPNH.Contains(mapn)
                                             select n).ToList();
                    PHIEU_NHAP_HANG png = (from n in db.PHIEU_NHAP_HANG
                                           where n.MaPNH.Contains(mapn)
                                           select n).FirstOrDefault();
                    if (png.Gui==false)
                    {
                        foreach (var item in ds)
                        {
                            HANG_HOA hh = (from n in db.HANG_HOA
                                           where n.MaHH.Contains(item.MaHH)
                                           select n).FirstOrDefault();
                            hh.TonKhoDuKien += item.SoLuong;

                        }
                    }
                    string ncc = (from n in db.NHA_CUNG_CAP
                                  where n.MaNCC.Contains(png.MaNCC)
                                  select n.Email).FirstOrDefault();
                    png.Gui = true;
                    db.SaveChanges();
                    Gui(data12,ncc);
                }
                
                return new JsonResult { Data = new { status = status, message = "Gửi thành công" } };
            }
            catch
            {
                status = false;
                return new JsonResult { Data = new { status = status, message = "Lỗi !" } };
            }
        }

     

        // GET: Admin/PNHCt/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                PNHCt ddh = new PNHCt();
                PHIEU_NHAP_HANG dh = (from n in db.PHIEU_NHAP_HANG
                                   where n.MaPNH.Contains(id)
                                   select n).FirstOrDefault();
                List<CHI_TIET_PNH> ct = (from n in db.CHI_TIET_PNH
                                         where n.MaPNH.Contains(id)
                                         select n).ToList();
                List<listchitietpn> dsct = new List<listchitietpn>();
                double tongtien = 0;
                foreach (var item in ct)
                {
                    listchitietpn c = new listchitietpn();
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
                ddh.nhacc = (from n in db.NHA_CUNG_CAP
                             where n.MaNCC.Contains(dh.MaNCC.ToString())
                             select n.TenNCC).FirstOrDefault();
                ddh.mapn = id;
                ddh.danhsachct = dsct;
                ViewBag.TongTien = tongtien;
                return View(ddh);
            }
        }

        // POST: Admin/PNHCt/Edit/5
        [HttpPost]
        public ActionResult Edit(PNHCt dt)
        {
            bool status = true;

            var isValidModel = TryUpdateModel(dt);

            if (isValidModel)
            {
                using (HTTTEntities db = new HTTTEntities())
                {
                    List<CHI_TIET_PNH> dsctx = (from n in db.CHI_TIET_PNH
                                                where n.MaPNH.Contains(dt.mapn)
                                                select n).ToList();

                    foreach (var item in dsctx)
                    {
                        HANG_HOA hh = (from n in db.HANG_HOA
                                       where n.MaHH.Contains(item.MaHH)
                                       select n).FirstOrDefault();
                        hh.TonKhoDuKien -= item.SoLuong;
                        db.CHI_TIET_PNH.Remove(item);
                    }
                    int count = 1;

                    foreach (var item in dt.danhsachct)
                    {
                        CHI_TIET_PNH ct = new CHI_TIET_PNH()
                        {
                            MaPNH = dt.mapn,
                            STT = count,
                            SoLuong = item.soluong,
                            ChietKhau = item.chietkhau,
                            TongTien = item.thanhtien,
                            GhiChu = item.ghichu,
                            MaHH = timMaHH(item.hanghoa),
                        };
                        db.CHI_TIET_PNH.Add(ct);
                        HANG_HOA hh = (from n in db.HANG_HOA
                                       where n.MaHH.Contains(ct.MaHH)
                                       select n).FirstOrDefault();
                        hh.TonKhoDuKien += ct.SoLuong;
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

        // GET: Admin/PNHCt/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/PNHCt/Delete/5
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
                List<CHI_TIET_PNH> ds = (from n in db.CHI_TIET_PNH
                                         where n.MaPNH.Contains(id)
                                         select n).ToList();
                PHIEU_NHAP_HANG dh = (from n in db.PHIEU_NHAP_HANG
                                   where n.MaPNH.Contains(id)
                                   select n).FirstOrDefault();

                string makh = db.KHO_HANG.Min(o => o.MaKH);
                foreach (var item in ds)
                {
                    HANG_HOA hh = (from n in db.HANG_HOA
                                   where n.MaHH.Contains(item.MaHH)
                                   select n).FirstOrDefault();
                    if (dh.Gui==true)
                    {
                        hh.TonKhoDuKien -= item.SoLuong;
                    }
                    
                    if (dh.NhanHang == true)
                    {
                        Ton_kho tk = (from n in db.Ton_kho
                                      where n.MaHH.Contains(hh.MaHH) && n.MaKH.Contains(makh)
                                      select n).FirstOrDefault();
                        tk.SoLuong -= item.SoLuong;

                    }

                    db.CHI_TIET_PNH.Remove(item);
                }

                db.PHIEU_NHAP_HANG.Remove(dh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            
        }

        public bool DoiTinhTrang(string id)
        {
            var user = db.PHIEU_NHAP_HANG.Find(id);
            user.NhanHang = !user.NhanHang;


            List<CHI_TIET_PNH> ds = (from n in db.CHI_TIET_PNH
                                     where n.MaPNH.Contains(id)
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
                if (user.NhanHang == true)
                {
                    tk.SoLuong += item.SoLuong;
                }
                else
                {
                    tk.SoLuong -= item.SoLuong;
                }
            }
            db.SaveChanges();
            return user.NhanHang;
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
            XemPNH xemct = new XemPNH();
            PHIEU_NHAP_HANG ddh = db.PHIEU_NHAP_HANG.Find(id);
            List<CHI_TIET_PNH> ct = (from n in db.CHI_TIET_PNH
                                     where n.MaPNH.Contains(id)
                                     select n).ToList();
            double tongtien = 0;
            foreach (var item in ct)
            {
                tongtien += item.TongTien;
            }
            ViewBag.TongTien = tongtien;
            xemct.PHIEU_NHAP_HANG = ddh;
            xemct.cHI_TIET_PNHs = ct;
            return View(xemct);
        }

        public ActionResult Print(string id)
        {
            return new ActionAsPdf("Details1", new { id = id });
        }
    }
}
