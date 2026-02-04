using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class EFCDbContext : DbContext
    {
        public EFCDbContext(DbContextOptions<EFCDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
    $"Host={Environment.GetEnvironmentVariable("POSTGRESQL_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("POSTGRESQL_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE")};" +
    $"Username={Environment.GetEnvironmentVariable("POSTGRESQL_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")}";

            optionsBuilder.UseNpgsql(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Login);
                entity.Property(u => u.Login).HasMaxLength(10);
                entity.Navigation(u => u.Goals).UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.UserLogin)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.HasOne<User>()
                    .WithMany(u => u.Goals)
                    .HasForeignKey(g => g.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Goal>().HasKey(g => g.Id);


            base.OnModelCreating(modelBuilder);
        }
    }
}
