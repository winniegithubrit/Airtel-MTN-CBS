using Microsoft.EntityFrameworkCore;
using System;

namespace Co_Banking_System.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<AdditionalInfo> AdditionalInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.StatusId);

            modelBuilder.Entity<AdditionalInfo>()
                .HasOne(ai => ai.Transaction)
                .WithMany(t => t.AdditionalInfos)
                .HasForeignKey(ai => ai.TransactionId);

            // Seed data
            modelBuilder.Entity<TransactionStatus>().HasData(
                new TransactionStatus { StatusId = 1, StatusCode = "Pending", Description = "Transaction is pending" },
                new TransactionStatus { StatusId = 2, StatusCode = "Completed", Description = "Transaction is completed" },
                new TransactionStatus { StatusId = 3, StatusCode = "Failed", Description = "Transaction has failed" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Wendy",
                    LastName = "Jomo",
                    Email = "wendy.jomo@gmail.com",
                    PasswordHash = "password",
                    PhoneNumber = "0797594751",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 2,
                    FirstName = "Lexxie",
                    LastName = "Wanjiru",
                    Email = "lexxnjiru@gmail.com",
                    PasswordHash = "lexx",
                    PhoneNumber = "0797594745",
                    CreatedAt = DateTime.UtcNow
                },
                new User{
                    Id = 3,
                    FirstName = "Johnson",
                    LastName = "Njichi",
                    Email = "johnchi@gmail.com",
                    PasswordHash = "john",
                    PhoneNumber = "0797794745",
                    CreatedAt = DateTime.UtcNow
                },
                new User{
                    Id = 4,
                    FirstName = "Wesley",
                    LastName = "Kamau",
                    Email = "kamawe@gmail.com",
                    PasswordHash = "kama",
                    PhoneNumber = "0700594745",
                    CreatedAt = DateTime.UtcNow
                }
            );
                modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    TransactionId = 1,
                    UserId = 1,
                    StatusId = 2, 
                    Amount = 100.00m,
                    Reference = "TXN10001",
                    CreatedAt = DateTime.UtcNow
                },
                new Transaction
                {
                    TransactionId = 2,
                    UserId = 1,
                    StatusId = 1,
                    Amount = 200.00m,
                    Reference = "TXN10002",
                    CreatedAt = DateTime.UtcNow
                },
                new Transaction
                {
                    TransactionId = 3,
                    UserId = 1,
                    StatusId = 3, 
                    Amount = 300.00m,
                    Reference = "TXN10003",
                    CreatedAt = DateTime.UtcNow
                },
                new Transaction
                {
                    TransactionId = 4,
                    UserId = 1,
                    StatusId = 2, 
                    Amount = 400.00m,
                    Reference = "TXN10004",
                    CreatedAt = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<AdditionalInfo>().HasData(
                new AdditionalInfo
                {
                    AdditionalInfoId = 1,
                    TransactionId = 1,
                    InfoKey = "Note",
                    InfoValue = "Payment for services"
                },
                new AdditionalInfo
                {
                    AdditionalInfoId = 2,
                    TransactionId = 2,
                    InfoKey = "Note",
                    InfoValue = "Pending payment"
                },
                new AdditionalInfo
                {
                    AdditionalInfoId = 3,
                    TransactionId = 3,
                    InfoKey = "Note",
                    InfoValue = "Failed payment"
                },
                new AdditionalInfo
                {
                    AdditionalInfoId = 4,
                    TransactionId = 4,
                    InfoKey = "Note",
                    InfoValue = "Successful payment"
                }
            );
        }
    }
}
