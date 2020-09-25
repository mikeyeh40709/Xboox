namespace XbooxLibrary.Models.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coupons
    {
        public Guid Id { get; set; }
        [Display(Name = "優惠券名稱")]
        [StringLength(50)]
        [Required(ErrorMessage = "優惠券名稱必須輸入!")]
        public string CouponName { get; set; }
        [Display(Name = "優惠折數/金額")]
        [Required(ErrorMessage = "優惠折數/金額必須輸入!")]
        public decimal? Discount { get; set; }
        [Display(Name = "優惠碼")]
        [Required(ErrorMessage = "優惠碼必須輸入!")]
        [StringLength(50)]
        public string CouponCode { get; set; }
        [Display(Name = "優惠起始日")]
        [Required(ErrorMessage = "優惠起始日必須輸入!")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [Display(Name = "優惠截止日")]
        [Required(ErrorMessage = "優惠截止日必須輸入!")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
    }
}
