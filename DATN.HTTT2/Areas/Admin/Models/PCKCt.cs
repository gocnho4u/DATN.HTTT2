using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models
{
    public class PCKCt
    {
        [Display(Name ="Kho hàng nhận")]
        public string MaKHN { get; set; }

        [Display(Name = "Kho hàng xuất")]
        public string MaKHX { get; set; }

        public virtual List<listchitietck> ctck { get; set; }
    }

    public class listchitietck
    {
        public string hanghoa { get; set; }
        public int SoLuong { get; set; }
        public string ghichu { get; set; }
    }
}