using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models.ViewModels
{
    public class XemPNH
    {
        public PHIEU_NHAP_HANG PHIEU_NHAP_HANG { get; set; }
        public List<CHI_TIET_PNH> cHI_TIET_PNHs { get; set; }
    }
}