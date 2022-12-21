using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models.ViewModels
{
    public class XemDDH
    {
        public DON_DAT_HANG DON_DAT_HANG { get; set; }
        public List<CHI_TIET_DDH> cHI_TIET_DDHs { get; set; }
    }

}