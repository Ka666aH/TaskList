using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastucture.Database
{
    public class PostgreSQLDbContext : DbContext
    {
        private IConfiguration _configuration;
        public PostgreSQLDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<User> Users {  get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Login);

            modelBuilder.Entity<Goal>().HasKey(g => g.Id);
            modelBuilder.Entity<Goal>().HasOne(g => g.User).WithMany();

            base.OnModelCreating(modelBuilder);
        }
    }
}
