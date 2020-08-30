namespace Xboox.Models.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductImgs
    {
        [Key]
        public long ProductImgId { get; set; }

        public Guid ProductId { get; set; }

        [Required]
        public string imgLink { get; set; }
    }
}
