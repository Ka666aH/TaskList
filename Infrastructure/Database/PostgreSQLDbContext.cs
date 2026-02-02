using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class PostgreSQLDbContext : DbContext
    {
        public PostgreSQLDbContext(DbContextOptions<PostgreSQLDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Login);
            modelBuilder.Entity<User>().Property(u => u.Login).HasMaxLength(10);

            modelBuilder.Entity<Goal>().HasKey(g => g.Id);
            modelBuilder.Entity<Goal>().HasOne(g => g.User).WithMany().HasForeignKey(g => g.UserLogin).IsRequired().OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
