namespace XbooxLibrary.Models.DataTable
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
            CartItems = new HashSet<CartItems>();
            OrderDetails = new HashSet<OrderDetails>();
            ProductTags = new HashSet<ProductTags>();
            WishList = new HashSet<WishList>();
        }

        public Guid ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public int UnitInStock { get; set; }

        public decimal Price { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        [StringLength(50)]
        public string ProductImgId { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [StringLength(50)]
        public string Publisher { get; set; }

        public string Intro { get; set; }

        public Guid CategoryId { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string Specification { get; set; }

        public DateTime PublishedDate { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItems> CartItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTags> ProductTags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishList> WishList { get; set; }
    }
}
