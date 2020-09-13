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

        [StringLength(50)]
        public string CouponName { get; set; }

        public decimal? Discount { get; set; }

        [StringLength(50)]
        public string CouponCode { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
