namespace XbooxCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            CartItmes = new HashSet<CartItmes>();
            ProductTags = new HashSet<ProductTags>();
            WishList = new HashSet<WishList>();
            WishList1 = new HashSet<WishList>();
        }

        public Guid ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        public int ProductImgId { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [StringLength(50)]
        public string Publisher { get; set; }

        [Required]
        [StringLength(10)]
        public string PublishedDate { get; set; }

        public string Intro { get; set; }

        public Guid CategoryId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItmes> CartItmes { get; set; }

        public virtual ProductImgs ProductImgs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTags> ProductTags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishList> WishList { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishList> WishList1 { get; set; }
    }
}
