namespace Xboox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public Guid OrderId { get; set; }

        [Required]
        [StringLength(10)]
        public string ProductName { get; set; }

        public Guid ProductId { get; set; }

        public decimal UnitPrice { get; set; }

        public int? ImageId { get; set; }

        [Required]
        [StringLength(10)]
        public string Quantity { get; set; }

        public virtual Order Order { get; set; }
    }
}
