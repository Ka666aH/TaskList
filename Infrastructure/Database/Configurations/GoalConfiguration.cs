using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Database.Configurations
{
    public class GoalConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.UserLogin)
                    .HasMaxLength(10)
                    .IsRequired();

            builder.HasOne<User>()
                    .WithMany(u => u.Goals)
                    .HasForeignKey(g => g.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(g => g.UserLogin);
        }
    }
}
