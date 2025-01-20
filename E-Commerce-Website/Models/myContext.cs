﻿using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Website.Models
{
    public class myContext : DbContext
    {
        public myContext(DbContextOptions <myContext> options) : base(options)
        { 
        }
        //DBset for the Model and to create Database Name (tbl_admins)
        public DbSet<Admin> tbl_admins { get; set; }
        public DbSet<Customer> tbl_customer { get; set; }
        public DbSet<Category> tbl_category { get; set; }
        public DbSet<Product> tbl_product { get; set; }
        public DbSet<Cart> tbl_cart{ get; set; }
        public DbSet<Feedback> tbl_feedback { get; set; }
        public DbSet<Faqs> tbl_faqs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring one-to-many relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) // Product has one Category
                .WithMany(c => c.Product) // Category has many Products
                .HasForeignKey(p => p.cat_id) // Foreign key in Product
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete

            base.OnModelCreating(modelBuilder);
        }
    }
}
