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
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
