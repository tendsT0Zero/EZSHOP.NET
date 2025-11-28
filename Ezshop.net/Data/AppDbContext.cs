using EzShop.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EzShop.Api.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10.99m, Stock = 100, Category = "Category A", Description = "Description for Product 1" },
                new Product { Id = 2, Name = "Product 2", Price = 15.49m, Stock = 200, Category = "Category B", Description = "Description for Product 2" },
                new Product { Id = 3, Name = "Product 3", Price = 7.99m, Stock = 150, Category = "Category A", Description = "Description for Product 3" }
            );

            var readerRoleId = "2021219";
            var writerRoleId = "20212100";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    ConcurrencyStamp=readerRoleId,
                    NormalizedName = "Reader"
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    ConcurrencyStamp=writerRoleId,
                    NormalizedName = "Writer"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
