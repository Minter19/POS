using Domain.Entities;
using Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>().HasData(ProductExampleSeed.GetProductsExample()); //melakukan seeding data pada awal dijalankan 

            //default template schema
            //modelBuilder.HasDefaultSchema("IE"); //default entity schema untuk semua entity
            base.OnModelCreating(modelBuilder);
        }
    }
}
