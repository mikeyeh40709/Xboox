namespace XbooxCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CartItmes
    {
        [Key]
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public Guid Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
