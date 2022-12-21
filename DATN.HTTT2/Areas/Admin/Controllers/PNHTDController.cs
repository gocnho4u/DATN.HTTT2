using DATN.HTTT2.Areas.Admin.Models;
using DATN.HTTT2.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATN.HTTT2.Areas.Admin.Controllers
{
    public class PNHTDController : Controller
    {
        public string tennhacc(string key)
        {
            string ten = (from n in db.NHA_CUNG_CAP
                          where n.MaNCC.Contains(key)
                          select n.TenNCC).FirstOrDefault();
            return ten;
        }
        public string manhacc(string key)
        {
            string ten = (from n in db.NHA_CUNG_CAP
                          where n.TenNCC.Contains(key)
                          select n.MaNCC).FirstOrDefault();
            return ten;
        }

        private HTTTEntities db = new HTTTEntities();
        // GET: Admin/PNHTD
        public ActionResult Index()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                List<HANG_HOA> hht = (from n in db.HANG_HOA
                                      where n.TonKhoDuKien<n.DinhMucTK
                                      select n).ToList();
                int count = (from n in db.PHIEU_NHAP_HANG
                             select n).Count() + 1;
                List<PHIEU_NHAP_HANG> pntd = new List<PHIEU_NHAP_HANG>();
                foreach (var item in hht)
                {
                    int ktlap = 0;
                    foreach (var pnn in pntd)
                    {
                        if (item.NHA_CUNG_CAP.TenNCC == pnn.MaNCC)
                        {
                            ktlap++;
                        }
                        else continue;
                    }
                    if (ktlap == 0)
                    {
                        PHIEU_NHAP_HANG pn = new PHIEU_NHAP_HANG();
                        pn.MaPNH = "PN" + count + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        pn.NgayNhap = DateTime.Now;
                        pn.MaNCC = tennhacc(item.MaNCC);
                        pn.Gui = false;
                        pn.NhanHang = false;
                        pntd.Add(pn);
                        count++;
                    }
                    else
                    {
                        continue;
                    }
                }

                return View(pntd);
            }
        }

        // GET: Admin/PNHTD/Details/5
        public ActionResult Details(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                double tongtien = 0;
                string bien = manhacc(id);
                List<HANG_HOA> hht = (from n in db.HANG_HOA
                                      where n.TonKhoDuKien<n.DinhMucTK && n.MaNCC.Contains(bien)
                                      select n).ToList();
                List<CHI_TIET_PNH> dsct = new List<CHI_TIET_PNH>();
                int stt = 1;
                foreach (var item in hht)
                {
                    CHI_TIET_PNH ct = new CHI_TIET_PNH();
                    ct.MaPNH = bien;
                    ct.STT = stt;
                    ct.SoLuong = -(item.TonKhoDuKien)+item.DinhMucTK;
                    ct.TongTien = item.DonGia * ct.SoLuong;
                    ct.GhiChu = "";
                    ct.MaHH = item.TenHangHoa;

                    dsct.Add(ct);
                    tongtien += ct.TongTien;
                    stt++;
                }
                ViewBag.TongTien = tongtien;
                ViewBag.TenNCC = id;
                ViewBag.DiaChi = (from n in db.NHA_CUNG_CAP
                                  where n.MaNCC.Contains(bien)
                                  select n.DiaChi).FirstOrDefault();
                return View(dsct);
            }
        }

        // GET: Admin/PNHTD/Create
        public ActionResult Create(string id)
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                double tongtien = 0;
                string bien = manhacc(id);
                List<HANG_HOA> hht = (from n in db.HANG_HOA
                                      where n.TonKhoDuKien<n.DinhMucTK && n.MaNCC.Contains(bien)
                                      select n).ToList();
                List<listchitietpn> dsct = new List<listchitietpn>();
                foreach (var item in hht)
                {
                    listchitietpn ct = new listchitietpn();
                    ct.hanghoa = item.TenHangHoa;
                    ct.soluong = -(item.TonKhoDuKien)+item.DinhMucTK; 
                    ct.thanhtien = item.DonGia * ct.soluong;
                    ct.ghichu = item.DonViTinh;
                    dsct.Add(ct);
                    tongtien += ct.thanhtien;
                }
                int dem = (from n in db.PHIEU_NHAP_HANG
                           select n).Count() + 1;
                ViewBag.MaPN = "PNH" + dem + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ViewBag.TongTien = tongtien;
                PNHCt a = new PNHCt();
                a.danhsachct = dsct;
                a.nhacc = id;
                return View(a);
            }
        }

    }
}
