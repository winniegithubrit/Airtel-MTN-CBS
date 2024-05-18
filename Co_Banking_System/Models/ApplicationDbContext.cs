using Microsoft.EntityFrameworkCore;

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
    }
  }
}
