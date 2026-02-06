using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Login);

            builder.Property(u => u.Login).HasMaxLength(10);
            builder.Navigation(u => u.Goals).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId);

            builder.HasData(
                new User("admin", "$2a$12$jKg1PDYGJudrCUJP7rr4FOXe.EP9s4CLJcMf23rPsSg45QKwvSFGi", (int)RoleType.Admin) //admin
                //here
                );
        }
    }
}
