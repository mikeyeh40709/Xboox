namespace Xboox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WishList")]
    public partial class WishList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ListId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public Guid ProductId { get; set; }

        public DateTime Datetime { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Product Product { get; set; }

        public virtual Product Product1 { get; set; }
    }
}
