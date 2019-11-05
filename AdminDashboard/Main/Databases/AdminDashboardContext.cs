using AdminDashboard.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Main.Databases
{
    public class AdminDashboardContext : DbContext
    {
        public DbSet<DbUser> User { get; set; }

        public DbSet<DbAccount> Account { get; set; }

        public AdminDashboardContext(DbContextOptions<AdminDashboardContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureAccount(modelBuilder);
            ConfigureUser(modelBuilder);
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Account)
                  .WithMany(p => p.Users);
            });
        }

        private static void ConfigureAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Plan).IsRequired();
            });
        }
    }
}
