using Microsoft.EntityFrameworkCore;
using PensionManagement.Domain.Entities;

namespace PensionManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
            .HasIndex(m => m.Email)
            .IsUnique();

            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Contributions)
                .HasForeignKey(c => c.MemberId);

            modelBuilder.Entity<Benefit>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Benefits)
                .HasForeignKey(b => b.MemberId);

            modelBuilder.Entity<TransactionHistory>()
                .Property(t => t.ChangeDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
