using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models
{
    public class PNHCt
    {
        public string nhacc { get; set; }
        public string mapn { get; set; }

        public virtual List<listchitietpn> danhsachct { get; set; }
    }

    public class listchitietpn
    {
        public string hanghoa { get; set; }
        public int soluong { get; set; }
        public int chietkhau { get; set; }
        public double thanhtien { get; set; }
        public string ghichu { get; set; }
        public string dvt { get; set; }
        public int dongia { get; set; }
    }
}