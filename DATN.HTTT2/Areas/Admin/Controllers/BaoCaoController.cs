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
    public class BaoCaoController : Controller
    {
        private HTTTEntities db = new HTTTEntities();
        // GET: Admin/BaoCao
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                NHAN_VIEN nv = (NHAN_VIEN)Session["Nhanvien"];
                if (nv.QuyenQL == false)
                {
                    return RedirectToAction("khongquyen", "HANG_HOA");
                }
                else
                {
                    ViewBag.Thoigian = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    List<string> ten = new List<string>();
                    ten.Add("Bán hàng");
                    ten.Add("Nhập hàng");
                    ViewBag.ten = ten;
                    //tổng tiền đặt hàng

                    //Ngày hôm nay ************
                    var homnay = DateTime.Now.Date;
                    ViewBag.Ngay = DateTime.Now.Date;
                    //Danh sách đơn đặt hàng đã giao
                    var dddhdg = (from n in db.DON_DAT_HANG
                                  where n.GiaoHang=="Đã giao"
                                    select n);
                    //List báo cáo
                    //Chưa giao
                    var dddhcg = (from n in db.DON_DAT_HANG
                                  where n.GiaoHang == "Chưa giao"
                                  select n);
                    List<BaoCao> dsddhcg = new List<BaoCao>();
                    foreach (var item in dddhcg)
                    {
                        BaoCao a = new BaoCao();
                        a.madh = item.MaDDH;
                        a.ngay = item.NgayDat.Date;
                        dsddhcg.Add(a);
                    }
                    //Đã giao
                    List<BaoCao> dsddhdg = new List<BaoCao>();
                    foreach (var item in dddhdg)
                    {
                        BaoCao a = new BaoCao();
                        a.madh = item.MaDDH;
                        a.ngay = item.NgayDat.Date;
                        dsddhdg.Add(a);
                    }
                    List<string> ddh = (from n in dsddhdg
                                        where n.ngay==homnay
                                        select n.madh).ToList();
                    double tongtienddh = 0;
                    foreach (var item in ddh)
                    {
                        List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                 where n.MaDDH.Contains(item)
                                                 select n).ToList();
                        foreach (var ct in ds)
                        {
                            tongtienddh += ct.TongTien;
                        }

                    }
                    //tổng tiền nhập hàng
                    //Danh sách phiếu nhập hàng chưa gửi
                    var pnhcg = (from n in db.PHIEU_NHAP_HANG
                                 where n.Gui == false
                                 select n);
                    //List báo cáo
                    List<BaoCao> dspnhcg = new List<BaoCao>();
                    foreach (var item in pnhcg)
                    {
                        BaoCao a = new BaoCao();
                        a.madh = item.MaPNH;
                        a.ngay = item.NgayNhap.Date;
                        dspnhcg.Add(a);
                    }
                    //Danh sách phiếu nhập hàng đã gửi
                    var pnhdg = (from n in db.PHIEU_NHAP_HANG
                                  where n.Gui == true
                                  select n);
                    //List báo cáo
                    List<BaoCao> dspnhdg = new List<BaoCao>();
                    foreach (var item in pnhdg)
                    {
                        BaoCao a = new BaoCao();
                        a.madh = item.MaPNH;
                        a.ngay = item.NgayNhap.Date;
                        dspnhdg.Add(a);
                    }
                    List<string> pnh = (from n in dspnhdg
                                        where n.ngay==homnay
                                        select n.madh).ToList();
                    double tongtienpnh = 0;
                    foreach (var item in pnh)
                    {
                        List<CHI_TIET_PNH> ds = (from n in db.CHI_TIET_PNH
                                                 where n.MaPNH.Contains(item)
                                                 select n).ToList();
                        foreach (var ct in ds)
                        {
                            tongtienpnh += ct.TongTien;
                        }

                    }
                    List<double> giatri = new List<double>();
                    giatri.Add(tongtienddh);
                    giatri.Add(tongtienpnh);
                    ViewBag.giatri = giatri;

                    //So sánh khách sỉ và đại lý
                    List<string> ten2 = new List<string>();
                    ten2.Add("Khách mua sỉ");
                    ten2.Add("Đại lý");
                    ViewBag.ten2 = ten2;
                    //tổng tiền đặt hàng
                    //Danh sách đơn đặt hàng all khách sỉ
                    var ddhall = (from n in db.DON_DAT_HANG
                                  where n.MaDL.Contains("KS")
                                  select n);
                    //List báo cáo
                    List<BaoCao> dsddhall = new List<BaoCao>();
                    foreach (var item in ddhall)
                    {
                        BaoCao a = new BaoCao();
                        a.madh = item.MaDDH;
                        a.ngay = item.NgayDat.Date;
                        dsddhall.Add(a);
                    }
                    List<string> ddh2 = (from n in dsddhall
                                         where n.ngay==homnay
                                         select n.madh).ToList();
                    double tongtienddh2 = 0;
                    foreach (var item in ddh2)
                    {
                        List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                 where n.MaDDH.Contains(item)
                                                 select n).ToList();
                        foreach (var ct in ds)
                        {
                            tongtienddh2 += ct.TongTien;
                        }

                    }
                    //tổng tiền mua hàng đại lý
                    //Danh sách đơn đặt hàng all đại lý
                    var ddhalldl = (from n in db.DON_DAT_HANG
                                  where n.MaDL!="KS"
                                  select n);
                    //List báo cáo
                    List<BaoCao> dsddhalldl = new List<BaoCao>();
                    foreach (var item in ddhalldl)
                    {
                        BaoCao a = new BaoCao();
                        a.madh = item.MaDDH;
                        a.ngay = item.NgayDat.Date;
                        dsddhalldl.Add(a);
                    }
                    List<string> ddh4 = (from n in dsddhalldl
                                         where n.ngay==homnay
                                         select n.madh).ToList();
                    double tongtienddh4 = 0;
                    foreach (var item in ddh4)
                    {
                        List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                 where n.MaDDH.Contains(item)
                                                 select n).ToList();
                        foreach (var ct in ds)
                        {
                            tongtienddh4 += ct.TongTien;
                        }

                    }
                    List<double> giatri2 = new List<double>();
                    giatri2.Add(tongtienddh2);
                    giatri2.Add(tongtienddh4);
                    ViewBag.giatri2 = giatri2;
                    ViewBag.giatri2 = giatri2;


                    ViewBag.SODHCG = (from n in dsddhcg
                                    where n.ngay == homnay
                                    select n).Count().ToString() + " đơn hàng";
                    ViewBag.SODH = (from n in dsddhdg
                                    where n.ngay == homnay
                                    select n).Count().ToString()+" đơn hàng";
                    ViewBag.SOPN = (from n in dspnhdg
                                    where n.ngay==homnay
                                    select n).Count().ToString() + " phiếu nhập";
                    ViewBag.SOPNCG = (from n in dspnhcg
                                    where n.ngay == homnay
                                    select n).Count().ToString() + " phiếu nhập";
                    ViewBag.DTDH = tongtienddh + " VND";

                    return View();
                }
            }
        }



        public ActionResult Index2(DateTime? date1,DateTime?date2)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                NHAN_VIEN nv = (NHAN_VIEN)Session["Nhanvien"];
                if (nv.QuyenQL == false)
                {
                    return RedirectToAction("khongquyen", "HANG_HOA");
                }
                else
                {
                    if (Request.Form["submitButton1"] != null)
                    {
                        
                        try
                        {
                            if (date1 == null || date2 == null || date2 < date1)
                            {
                                DateTime date3 = (DateTime)date1; DateTime datebd = date3.Date;
                                DateTime date4 = (DateTime)date2; DateTime datekt = date4.Date;
                                ViewBag.Loi = "Lỗi!!!!";
                                ViewBag.ThoigianBD = ViewBag.ThoigianBD = datebd.Day + "/" + datebd.Month + "/" + datebd.Year;
                                ViewBag.ThoigianKT = datekt.Day + "/" + datekt.Month + "/" + datekt.Year;
                                return View();
                            }
                            else
                            {
                                DateTime date3 = (DateTime)date1; DateTime datebd = date3.Date;
                                DateTime date4 = (DateTime)date2; DateTime datekt = date4.Date;
                                ViewBag.ThoigianBD = datebd.Day + "/" + datebd.Month + "/" + datebd.Year;
                                ViewBag.ThoigianKT = datekt.Day + "/" + datekt.Month + "/" + datekt.Year;
                                ViewBag.NgayBD = date1;
                                ViewBag.NgayKT = date2;
                                try
                                {
                                    List<string> ten = new List<string>();
                                    ten.Add("Bán hàng");
                                    ten.Add("Nhập hàng");
                                    ViewBag.ten = ten;
                                    //tổng tiền đặt hàng

                                    //Danh sách đơn đặt hàng đã giao
                                    var dddhdg = (from n in db.DON_DAT_HANG
                                                  where n.GiaoHang == "Đã giao"
                                                  select n);
                                    //List báo cáo
                                    //Chưa giao
                                    var dddhcg = (from n in db.DON_DAT_HANG
                                                  where n.GiaoHang == "Chưa giao"
                                                  select n);
                                    List<BaoCao> dsddhcg = new List<BaoCao>();
                                    foreach (var item in dddhcg)
                                    {
                                        BaoCao a = new BaoCao();
                                        a.madh = item.MaDDH;
                                        a.ngay = item.NgayDat.Date;
                                        dsddhcg.Add(a);
                                    }
                                    //Đã giao
                                    List<BaoCao> dsddhdg = new List<BaoCao>();
                                    foreach (var item in dddhdg)
                                    {
                                        BaoCao a = new BaoCao();
                                        a.madh = item.MaDDH;
                                        a.ngay = item.NgayDat.Date;
                                        dsddhdg.Add(a);
                                    }
                                    List<string> ddh = (from n in dsddhdg
                                                        where n.ngay >= datebd && n.ngay <= datekt == true
                                                        select n.madh).ToList();
                                    double tongtienddh = 0;
                                    foreach (var item in ddh)
                                    {
                                        List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                                 where n.MaDDH.Contains(item)
                                                                 select n).ToList();
                                        foreach (var ct in ds)
                                        {
                                            tongtienddh += ct.TongTien;
                                        }

                                    }
                                    //tổng tiền nhập hàng
                                    //Danh sách phiếu nhập hàng chưa gửi
                                    var pnhcg = (from n in db.PHIEU_NHAP_HANG
                                                 where n.Gui == false
                                                 select n);
                                    //List báo cáo
                                    List<BaoCao> dspnhcg = new List<BaoCao>();
                                    foreach (var item in pnhcg)
                                    {
                                        BaoCao a = new BaoCao();
                                        a.madh = item.MaPNH;
                                        a.ngay = item.NgayNhap.Date;
                                        dspnhcg.Add(a);
                                    }
                                    //Danh sách phiếu nhập hàng đã gửi
                                    var pnhdg = (from n in db.PHIEU_NHAP_HANG
                                                 where n.Gui == true
                                                 select n);
                                    //List báo cáo
                                    List<BaoCao> dspnhdg = new List<BaoCao>();
                                    foreach (var item in pnhdg)
                                    {
                                        BaoCao a = new BaoCao();
                                        a.madh = item.MaPNH;
                                        a.ngay = item.NgayNhap.Date;
                                        dspnhdg.Add(a);
                                    }
                                    List<string> pnh = (from n in dspnhdg
                                                        where n.ngay >= datebd && n.ngay <= datekt
                                                        select n.madh).ToList();
                                    double tongtienpnh = 0;
                                    foreach (var item in pnh)
                                    {
                                        List<CHI_TIET_PNH> ds = (from n in db.CHI_TIET_PNH
                                                                 where n.MaPNH.Contains(item)
                                                                 select n).ToList();
                                        foreach (var ct in ds)
                                        {
                                            tongtienpnh += ct.TongTien;
                                        }

                                    }
                                    List<double> giatri = new List<double>();
                                    giatri.Add(tongtienddh);
                                    giatri.Add(tongtienpnh);
                                    ViewBag.giatri = giatri;

                                    //So sánh khách sỉ và đại lý
                                    List<string> ten2 = new List<string>();
                                    ten2.Add("Khách mua sỉ");
                                    ten2.Add("Đại lý");
                                    ViewBag.ten2 = ten2;
                                    //tổng tiền đặt hàng
                                    //Danh sách đơn đặt hàng all khách sỉ
                                    var ddhall = (from n in db.DON_DAT_HANG
                                                  where n.MaDL.Contains("KS")
                                                  select n);
                                    //List báo cáo
                                    List<BaoCao> dsddhall = new List<BaoCao>();
                                    foreach (var item in ddhall)
                                    {
                                        BaoCao a = new BaoCao();
                                        a.madh = item.MaDDH;
                                        a.ngay = item.NgayDat.Date;
                                        dsddhall.Add(a);
                                    }
                                    List<string> ddh2 = (from n in dsddhall
                                                         where n.ngay >= datebd && n.ngay <= datekt
                                                         select n.madh).ToList();
                                    double tongtienddh2 = 0;
                                    foreach (var item in ddh2)
                                    {
                                        List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                                 where n.MaDDH.Contains(item)
                                                                 select n).ToList();
                                        foreach (var ct in ds)
                                        {
                                            tongtienddh2 += ct.TongTien;
                                        }

                                    }
                                    //tổng tiền mua hàng đại lý
                                    //Danh sách đơn đặt hàng all đại lý
                                    var ddhalldl = (from n in db.DON_DAT_HANG
                                                    where n.MaDL != "KS"
                                                    select n);
                                    //List báo cáo
                                    List<BaoCao> dsddhalldl = new List<BaoCao>();
                                    foreach (var item in ddhalldl)
                                    {
                                        BaoCao a = new BaoCao();
                                        a.madh = item.MaDDH;
                                        a.ngay = item.NgayDat.Date;
                                        dsddhalldl.Add(a);
                                    }
                                    List<string> ddh4 = (from n in dsddhalldl
                                                         where n.ngay >= datebd && n.ngay <= datekt
                                                         select n.madh).ToList();
                                    double tongtienddh4 = 0;
                                    foreach (var item in ddh4)
                                    {
                                        List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                                 where n.MaDDH.Contains(item)
                                                                 select n).ToList();
                                        foreach (var ct in ds)
                                        {
                                            tongtienddh4 += ct.TongTien;
                                        }

                                    }
                                    List<double> giatri2 = new List<double>();
                                    giatri2.Add(tongtienddh2);
                                    giatri2.Add(tongtienddh4);
                                    giatri2.Max();
                                    ViewBag.giatri2 = giatri2;

                                    ViewBag.SODHCG = (from n in dsddhcg
                                                      where n.ngay >= datebd && n.ngay <= datekt
                                                      select n.madh).Count().ToString() + " đơn hàng";
                                    ViewBag.SODH = (from n in dsddhdg
                                                    where n.ngay >= datebd && n.ngay <= datekt
                                                    select n.madh).Count().ToString() + " đơn hàng";
                                    ViewBag.SOPN = (from n in dspnhdg
                                                    where n.ngay >= datebd && n.ngay <= datekt
                                                    select n.madh).Count().ToString() + " phiếu nhập";
                                    ViewBag.SOPNCG = (from n in dspnhcg
                                                      where n.ngay >= datebd && n.ngay <= datekt
                                                      select n.madh).Count().ToString() + " phiếu nhập";
                                    ViewBag.DTDH = tongtienddh + " VND";
                                    
                                    return View();
                                }
                                catch
                                {
                                    ViewBag.Loi = "Lỗi hệ thống!";
                                    return View();
                                }
                            }
                            //ViewBag.Loi = "Lỗi hệ thống!";
                            //return View();
                        }
                        catch
                        {
                            ViewBag.Loi = "Lỗi hệ thống!";
                            return View();
                        }
                    }
                    else if(Request.Form["submitButton2"] != null)
                    {
                        DateTime date3;
                        DateTime date4;
                        DateTime datebd;
                        DateTime datekt;
                        try
                        {
                            date3 = (DateTime)date1; datebd = date3.Date;
                            date4 = (DateTime)date2; datekt = date4.Date;
                        }
                        catch
                        {
                            var homnay = DateTime.Now.Date;
                            date3 = homnay; datebd = date3.Date;
                            date4 = homnay; datekt = date4.Date;
                        }                       
                        return RedirectToAction("Print", "BaoCao", new { date1 = datebd, date2 = datekt });
                    }
                    else
                    {
                        ViewBag.Loi = "Lỗi hệ thống!";
                        return View();
                    }
                    
                }
            }
        }

        public ActionResult Details1(DateTime? date1, DateTime? date2)
        {
                try
                {
                    if (date1 == null || date2 == null || date2 < date1)
                    {
                        DateTime date3 = (DateTime)date1; DateTime datebd = date3.Date;
                        DateTime date4 = (DateTime)date2; DateTime datekt = date4.Date;
                        ViewBag.Loi = "Lỗi!!!!";
                        ViewBag.ThoigianBD = ViewBag.ThoigianBD = datebd.Day + "/" + datebd.Month + "/" + datebd.Year;
                        ViewBag.ThoigianKT = datekt.Day + "/" + datekt.Month + "/" + datekt.Year;
                        return View();
                    }
                    else
                    {
                        DateTime date3 = (DateTime)date1; DateTime datebd = date3.Date;
                        DateTime date4 = (DateTime)date2; DateTime datekt = date4.Date;
                        ViewBag.ThoigianBD = datebd.Day + "/" + datebd.Month + "/" + datebd.Year;
                        ViewBag.ThoigianKT = datekt.Day + "/" + datekt.Month + "/" + datekt.Year;
                        try
                        {
                            List<string> ten = new List<string>();
                            ten.Add("Bán hàng");
                            ten.Add("Nhập hàng");
                            ViewBag.ten = ten;
                            //tổng tiền đặt hàng

                            //Danh sách đơn đặt hàng đã giao
                            var dddhdg = (from n in db.DON_DAT_HANG
                                          where n.GiaoHang == "Đã giao"
                                          select n);
                            //List báo cáo
                            //Chưa giao
                            var dddhcg = (from n in db.DON_DAT_HANG
                                          where n.GiaoHang == "Chưa giao"
                                          select n);
                            List<BaoCao> dsddhcg = new List<BaoCao>();
                            foreach (var item in dddhcg)
                            {
                                BaoCao a = new BaoCao();
                                a.madh = item.MaDDH;
                                a.ngay = item.NgayDat.Date;
                                dsddhcg.Add(a);
                            }
                            //Đã giao
                            List<BaoCao> dsddhdg = new List<BaoCao>();
                            foreach (var item in dddhdg)
                            {
                                BaoCao a = new BaoCao();
                                a.madh = item.MaDDH;
                                a.ngay = item.NgayDat.Date;
                                dsddhdg.Add(a);
                            }
                            List<string> ddh = (from n in dsddhdg
                                                where n.ngay >= datebd && n.ngay <= datekt == true
                                                select n.madh).ToList();
                            double tongtienddh = 0;
                            foreach (var item in ddh)
                            {
                                List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                         where n.MaDDH.Contains(item)
                                                         select n).ToList();
                                foreach (var ct in ds)
                                {
                                    tongtienddh += ct.TongTien;
                                }

                            }
                            //tổng tiền nhập hàng
                            //Danh sách phiếu nhập hàng chưa gửi
                            var pnhcg = (from n in db.PHIEU_NHAP_HANG
                                         where n.Gui == false
                                         select n);
                            //List báo cáo
                            List<BaoCao> dspnhcg = new List<BaoCao>();
                            foreach (var item in pnhcg)
                            {
                                BaoCao a = new BaoCao();
                                a.madh = item.MaPNH;
                                a.ngay = item.NgayNhap.Date;
                                dspnhcg.Add(a);
                            }
                            //Danh sách phiếu nhập hàng đã gửi
                            var pnhdg = (from n in db.PHIEU_NHAP_HANG
                                         where n.Gui == true
                                         select n);
                            //List báo cáo
                            List<BaoCao> dspnhdg = new List<BaoCao>();
                            foreach (var item in pnhdg)
                            {
                                BaoCao a = new BaoCao();
                                a.madh = item.MaPNH;
                                a.ngay = item.NgayNhap.Date;
                                dspnhdg.Add(a);
                            }
                            List<string> pnh = (from n in dspnhdg
                                                where n.ngay >= datebd && n.ngay <= datekt
                                                select n.madh).ToList();
                            double tongtienpnh = 0;
                            foreach (var item in pnh)
                            {
                                List<CHI_TIET_PNH> ds = (from n in db.CHI_TIET_PNH
                                                         where n.MaPNH.Contains(item)
                                                         select n).ToList();
                                foreach (var ct in ds)
                                {
                                    tongtienpnh += ct.TongTien;
                                }

                            }
                            List<double> giatri = new List<double>();
                            giatri.Add(tongtienddh);
                            giatri.Add(tongtienpnh);
                            ViewBag.giatri = giatri;

                            //So sánh khách sỉ và đại lý
                            List<string> ten2 = new List<string>();
                            ten2.Add("Khách mua sỉ");
                            ten2.Add("Đại lý");
                            ViewBag.ten2 = ten2;
                            //tổng tiền đặt hàng
                            //Danh sách đơn đặt hàng all khách sỉ
                            var ddhall = (from n in db.DON_DAT_HANG
                                          where n.MaDL.Contains("KS")
                                          select n);
                            //List báo cáo
                            List<BaoCao> dsddhall = new List<BaoCao>();
                            foreach (var item in ddhall)
                            {
                                BaoCao a = new BaoCao();
                                a.madh = item.MaDDH;
                                a.ngay = item.NgayDat.Date;
                                dsddhall.Add(a);
                            }
                            List<string> ddh2 = (from n in dsddhall
                                                 where n.ngay >= datebd && n.ngay <= datekt
                                                 select n.madh).ToList();
                            double tongtienddh2 = 0;
                            foreach (var item in ddh2)
                            {
                                List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                         where n.MaDDH.Contains(item)
                                                         select n).ToList();
                                foreach (var ct in ds)
                                {
                                    tongtienddh2 += ct.TongTien;
                                }

                            }
                            //tổng tiền mua hàng đại lý
                            //Danh sách đơn đặt hàng all đại lý
                            var ddhalldl = (from n in db.DON_DAT_HANG
                                            where n.MaDL != "KS"
                                            select n);
                            //List báo cáo
                            List<BaoCao> dsddhalldl = new List<BaoCao>();
                            foreach (var item in ddhalldl)
                            {
                                BaoCao a = new BaoCao();
                                a.madh = item.MaDDH;
                                a.ngay = item.NgayDat.Date;
                                dsddhalldl.Add(a);
                            }
                            List<string> ddh4 = (from n in dsddhalldl
                                                 where n.ngay >= datebd && n.ngay <= datekt
                                                 select n.madh).ToList();
                            double tongtienddh4 = 0;
                            foreach (var item in ddh4)
                            {
                                List<CHI_TIET_DDH> ds = (from n in db.CHI_TIET_DDH
                                                         where n.MaDDH.Contains(item)
                                                         select n).ToList();
                                foreach (var ct in ds)
                                {
                                    tongtienddh4 += ct.TongTien;
                                }

                            }
                            List<double> giatri2 = new List<double>();
                            giatri2.Add(tongtienddh2);
                            giatri2.Add(tongtienddh4);
                            giatri2.Max();
                            ViewBag.giatri2 = giatri2;

                            ViewBag.SODHCG = (from n in dsddhcg
                                              where n.ngay >= datebd && n.ngay <= datekt
                                              select n.madh).Count().ToString() + " đơn hàng";
                            ViewBag.SODH = (from n in dsddhdg
                                            where n.ngay >= datebd && n.ngay <= datekt
                                            select n.madh).Count().ToString() + " đơn hàng";
                            ViewBag.SOPN = (from n in dspnhdg
                                            where n.ngay >= datebd && n.ngay <= datekt
                                            select n.madh).Count().ToString() + " phiếu nhập";
                            ViewBag.SOPNCG = (from n in dspnhcg
                                              where n.ngay >= datebd && n.ngay <= datekt
                                              select n.madh).Count().ToString() + " phiếu nhập";
                            ViewBag.DTDH = tongtienddh + " VND";

                            return View();
                        }
                        catch
                        {
                            ViewBag.Loi = "Lỗi hệ thống!";
                            return View();
                        }
                    }
                    //ViewBag.Loi = "Lỗi hệ thống!";
                    //return View();
                }
                catch
                {
                    ViewBag.Loi = "Lỗi hệ thống!";
                    return View();
                }           
        }

        public ActionResult Print(DateTime? date1, DateTime? date2)
        {
            DateTime date3 = (DateTime)date1; DateTime datebd = date3.Date;
            DateTime date4 = (DateTime)date2; DateTime datekt = date4.Date;
            return new ActionAsPdf("Details1",new { date1=datebd,date2=datekt});
        }
    }
}