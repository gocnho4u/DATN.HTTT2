using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models.ViewModels
{
    public class XemPCK
    {
        public PHIEU_CHUYEN_KHO PHIEU_CHUYEN_KHO { get; set; }
        public List<CHI_TIET_PCK> cHI_TIET_PCKs { get; set; }
    }
}