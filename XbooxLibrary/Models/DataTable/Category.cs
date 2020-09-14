namespace XbooxLibrary.Models.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Guid CategoryId { get; set; }
    }
}
