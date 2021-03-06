namespace XbooxLibrary.Models.DataTable
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class XbooxLibraryDBContext : DbContext
    {
        public XbooxLibraryDBContext()
            : base("name=XbooxLibraryDBContext")
        {
        }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Coupons> Coupons { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductImgs> ProductImgs { get; set; }
        public virtual DbSet<ProductTags> ProductTags { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.WishList)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cart>()
                .HasMany(e => e.CartItems)
                .WithRequired(e => e.Cart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Coupons>()
                .Property(e => e.Discount)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Order>()
                .Property(e => e.EcpayOrderNumber)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .Property(e => e.PurchaserName)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.CartItems)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductTags)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.WishList)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
