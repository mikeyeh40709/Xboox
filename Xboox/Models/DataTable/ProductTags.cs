namespace Xboox.Models.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductTags
    {
        public Guid ProductId { get; set; }

        public Guid Id { get; set; }

        public Guid? TagId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Tags Tags { get; set; }
    }
}
