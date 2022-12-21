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
    public class GiaoHangController : Controller
    {
        private HTTTEntities db = new HTTTEntities();
        // GET: GiaoNhan
        public ActionResult QLGiaoHang()
        {
            if (Session["Nhanvien"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View(db.DON_DAT_HANG.OrderByDescending(m => m.NgayDat));
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
    }
}