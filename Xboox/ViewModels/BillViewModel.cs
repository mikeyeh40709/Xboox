using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xboox.Models.ViewModels
{
    public class BillViewModel
    {
        public string PurchaserName { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaserAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaserEmail { get; set; }

        [StringLength(50)]
        public string PurchaserPhone { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}