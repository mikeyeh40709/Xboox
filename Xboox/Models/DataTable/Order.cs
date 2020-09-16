namespace Xboox.Models.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public Guid OrderId { get; set; }

        [StringLength(20)]
        public string EcpayOrderNumber { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(10)]
        public string PurchaserName { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaserEmail { get; set; }

        [StringLength(50)]
        public string PurchaserPhone { get; set; }

        [Required]
        [StringLength(20)]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        public string District { get; set; }

        [Required]
        [StringLength(50)]
        public string Road { get; set; }

        [StringLength(10)]
        public string Payment { get; set; }

        public bool Paid { get; set; }

        public bool Build { get; set; }

        public bool Remember { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
