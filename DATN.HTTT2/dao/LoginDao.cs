using DATN.HTTT2.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.dao
{
    public class LoginDao
    {
        private static LoginDao instance;

        public static LoginDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoginDao();
                return instance;
            }
        }
        private LoginDao() { }

        HTTTEntities db = new HTTTEntities();

        public DAI_LY login_kh(string tentk,string mk)
        {
            DAI_LY count = (from n in db.DAI_LY
                         where n.MaDL.Contains(tentk) && n.SoDienThoai.Equals(mk)
                         select n).FirstOrDefault();
            return count;
        }

        public NHAN_VIEN login_nv(string tentk, string mk)
        {
            NHAN_VIEN count = (from n in db.NHAN_VIEN
                         where n.MaNV.Contains(tentk) && n.MatKhau.Equals(mk)
                         select n).FirstOrDefault();
            return count;
        }

        public string tendl(string madl)
        {
            string ten = (from n in db.DAI_LY
                          where n.MaDL.Contains(madl)
                          select n.Ten).FirstOrDefault();
            return ten;
        }
    }
}