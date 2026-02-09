using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasData(
                    new { Id = (int)RoleType.Client, Name = RoleType.Client.ToString() },
                    new { Id = (int)RoleType.Admin, Name = RoleType.Admin.ToString() }
                );
        }
    }
}
