using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATN.HTTT2.Areas.Admin.Models
{
    public class TonKhoDK
    {
        
        [Display(Name ="Tên hàng hóa")]
        [Key]
        public string tenhh { get; set; }

        [Display(Name = "Tồn kho hiện tại")]
        public int soluong { get; set; }

        [Display(Name = "Tồn kho kế hoạch")]
        public int tonkhodk { get; set; }
    }
}