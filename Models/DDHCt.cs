using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models
{
    public class DDHCt
    {
        public string madh { get; set; }
        public string diachi { get; set; }
        public string daily { get; set; }

        public virtual List<listchitiet> danhsachct { get; set; }
    }

    public class listchitiet
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