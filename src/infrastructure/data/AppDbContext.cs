using Microsoft.EntityFrameworkCore;
using MiniPaymentApi.Domain.Entities;

namespace MiniPaymentApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bank>().HasData(
                new Bank { Id = 1, Name = "Akbank" },
                new Bank { Id = 2, Name = "Garanti" },
                new Bank { Id = 3, Name = "YapiKredi" }
            );
        }
    }
}
