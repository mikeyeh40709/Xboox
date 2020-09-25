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
        [Display(Name = "�u�f��W��")]
        [StringLength(50)]
        [Required(ErrorMessage = "�u�f��W�٥�����J!")]
        public string CouponName { get; set; }
        [Display(Name = "�u�f���/���B")]
        [Required(ErrorMessage = "�u�f���/���B������J!")]
        public decimal? Discount { get; set; }
        [Display(Name = "�u�f�X")]
        [Required(ErrorMessage = "�u�f�X������J!")]
        [StringLength(50)]
        public string CouponCode { get; set; }
        [Display(Name = "�u�f�_�l��")]
        [Required(ErrorMessage = "�u�f�_�l�饲����J!")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [Display(Name = "�u�f�I���")]
        [Required(ErrorMessage = "�u�f�I��饲����J!")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
    }
}
