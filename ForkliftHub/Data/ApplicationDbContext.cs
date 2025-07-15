using ForkliftHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<MastType> MastTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Brands
            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Toyota" },
                new Brand { Id = 2, Name = "Hyster" },
                new Brand { Id = 3, Name = "Linde" },
                new Brand { Id = 4, Name = "BT" }
            );

            // Seed Models 
            modelBuilder.Entity<Model>().HasData(
                new Model { Id = 1, Name = "8FBEKT20", BrandId = 1 },
                new Model { Id = 2, Name = "H3.0FT", BrandId = 2 },
                new Model { Id = 3, Name = "H25D", BrandId = 3 },
                new Model { Id = 4, Name = "02-8FGF25", BrandId = 1 }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electric Pallet Truck" },
                new Category { Id = 2, Name = "Stacker" },
                new Category { Id = 3, Name = "Reach Truck" },
                new Category { Id = 4, Name = "Forklift Truck" }
            );

            // Seed Engines
            modelBuilder.Entity<Engine>().HasData(
                new Engine { Id = 1, Type = "Diesel" },
                new Engine { Id = 2, Type = "Electric" },
                new Engine { Id = 3, Type = "LPG" }
            );

            // Seed MastTypes
            modelBuilder.Entity<MastType>().HasData(
                new MastType { Id = 1, Name = "Simplex" },
                new MastType { Id = 2, Name = "2-Stage" },
                new MastType { Id = 3, Name = "3-Stage" }
            );

            // Seed ProductTypes
            modelBuilder.Entity<ProductType>().HasData(
                 new ProductType { Id = 1, Name = "New" },
                 new ProductType { Id = 2, Name = "Used" }
             );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Toyota Electric Forklift",
                    Description = "High efficiency 3-wheel electric forklift.",
                    Price = 32000,
                    BrandId = 1,
                    CategoryId = 4,
                    EngineId = 2,
                    MastTypeId = 2,
                    ProductTypeId = 1,
                    ModelId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Used Hyster Diesel Forklift",
                    Description = "Reliable used diesel forklift for heavy lifting.",
                    Price = 14000,
                    BrandId = 2,
                    CategoryId = 4,
                    EngineId = 1,
                    MastTypeId = 1,
                    ProductTypeId = 2,
                    ModelId = 2
                }
            );

            // Configure Delete Behaviors and FK via FluentAPI
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Model)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Engine)
                .WithMany()
                .HasForeignKey(p => p.EngineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.MastType)
                .WithMany()
                .HasForeignKey(p => p.MastTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}

