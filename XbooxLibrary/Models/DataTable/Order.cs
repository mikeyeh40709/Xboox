namespace XbooxLibrary.Models.DataTable
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

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(10)]
        public string PurchaserName { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaserAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaserEmail { get; set; }

        [StringLength(50)]
        public string PurchaserPhone { get; set; }

        public int StateId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
