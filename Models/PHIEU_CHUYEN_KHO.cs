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

    public partial class PHIEU_CHUYEN_KHO
    {
        [Display(Name = "Mã phiếu chuyển")]
        public string MaPCK { get; set; }

        [Display(Name = "Ngày chuyển")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public System.DateTime NgayChuyen { get; set; }

        [Display(Name = "Mã nhân viên")]
        public string MaNV { get; set; }

        [Display(Name = "Mã kho hàng nhận")]
        public string MaKHN { get; set; }

        [Display(Name = "Mã kho hàng xuất")]
        public string MaKHX { get; set; }
    
        public virtual KHO_HANG KHO_HANG { get; set; }
        public virtual KHO_HANG KHO_HANG1 { get; set; }
        public virtual NHAN_VIEN NHAN_VIEN { get; set; }
    }
}
