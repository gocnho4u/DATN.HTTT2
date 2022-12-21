using DATN.HTTT2.Areas.Admin.Models;
using DATN.HTTT2.Areas.Admin.Models.ViewModels;
using DATN.HTTT2.dao;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace DATN.HTTT2.Controllers
{
    public class HomeController : Controller
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
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult Index(string tentk, string matkhau)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Session["DaiLy"] = LoginDao.Instance.login_kh(tentk, matkhau);
                    //int a = LoginDao.Instance.login_kh(tentk, matkhau);
                    Session["Nhanvien"] = LoginDao.Instance.login_nv(tentk, matkhau);
                    //int b = LoginDao.Instance.login_nv(tentk, matkhau);
                    if (Session["DaiLy"] != null && Session["Nhanvien"] == null)
                    {
                        TempData["TenDL"] = LoginDao.Instance.tendl(tentk);
                        return RedirectToAction("LapDDH", "Home");
                    }
                    else if (Session["Nhanvien"] != null && Session["DaiLy"] == null)
                    {
                        return RedirectToAction("Index", "DDHCt", new { area = "Admin" });
                    }
                    else
                    {
                        ViewBag.Message = "Đăng nhập không thành công";
                        return View();
                    }
                }
                catch (Exception)
                {
                    ViewBag.Message = "Lỗi hệ thống !";
                    return View();
                }
            }
            else
            return View();
        }

        public ActionResult LapDDH()
        {
            if (Session["DaiLy"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                DAI_LY dl = (DAI_LY)Session["DaiLy"];
                ViewBag.TenDL = dl.Ten;
                return View();
            }
            
        }

        [HttpPost]
        public JsonResult LapDDH(DDHCt dt)
        {
            

                DAI_LY dl = (DAI_LY)Session["DaiLy"];
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
                            MaDDH = "DDH" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString(),
                            NgayDat = DateTime.Now,
                            DiaChi = dt.diachi,
                            GiaoHang = "Chưa giao",
                            MaNV = "TD",
                            MaDL = dl.MaDL,

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
                }

                status = false;
                return new JsonResult { Data = new { status = status, message = "Lỗi !" } };
           

        }

        // GET: Admin/DDHCt
        public ActionResult XemDDH()
        {
            if (Session["DaiLy"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                DAI_LY dl = (DAI_LY)Session["DaiLy"];
                List<DON_DAT_HANG> ds = (from n in db.DON_DAT_HANG
                                         where n.MaDL.Contains(dl.MaDL)
                                         orderby n.NgayDat descending
                                         select n).ToList();
                return View(ds);

            }
        }

        public ActionResult Details(string id)
        {
            if (Session["DaiLy"] == null)
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

        public ActionResult DangXuat()
        {
            Session["DaiLy"] = null;
            Session["Nhanvien"] = null;
            Session.Abandon();
            return RedirectToAction("Index");
        }

        // GET: Home
        public ActionResult Loi()
        {   
            return View();
        }


        public ActionResult DoiMK(string id)
        {
            if (Session["DaiLy"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                DAI_LY dl = (DAI_LY)Session["DaiLy"];
                id = dl.MaDL;
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMK([Bind(Include = "MaDL,Ten,DiaChi,SoDienThoai")] DAI_LY dAI_LY)
        {
            if (Session["DaiLy"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dAI_LY).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("XemDDH");
                }
                return View();
            }
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

        public ActionResult QLGiaoHang()
        {
            if (Session["DaiLy"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                DAI_LY dl = (DAI_LY)Session["DaiLy"];
                List<DON_DAT_HANG> ds = (from n in db.DON_DAT_HANG
                                         where n.MaDL.Contains(dl.MaDL)
                                         orderby n.NgayDat descending
                                         select n).ToList();
                return View(ds);

            }
        }
    }
}