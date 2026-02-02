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
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Login);
                entity.Property(u => u.Login).HasMaxLength(10);

                entity.HasMany<Goal>()
                    .WithOne()
                    .HasForeignKey(g => g.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Navigation("Goals").UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Goal>().HasKey(g => g.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
