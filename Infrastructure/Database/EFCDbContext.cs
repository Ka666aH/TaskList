using Domain.Entities;
using Infrastructure.Database.Configurations;
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
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GoalConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
