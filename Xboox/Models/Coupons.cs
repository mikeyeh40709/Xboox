namespace Xboox.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coupons
    {
        [Key]
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CouponName { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal Discount { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string CouponCode { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime StartTime { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime EndTime { get; set; }
    }
}
