﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DATN.HTTT2.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class CHI_TIET_DDH
    {
        [Display(Name = "Mã đơn đặt hàng")]
        public string MaDDH { get; set; }
        public int STT { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Chiết khấu")]
        public int ChietKhau { get; set; }

        [Display(Name = "Tổng tiền")]
        public double TongTien { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Mã hàng hóa")]
        public string MaHH { get; set; }

        public virtual DON_DAT_HANG DON_DAT_HANG { get; set; }
        public virtual HANG_HOA HANG_HOA { get; set; }
    }
}
