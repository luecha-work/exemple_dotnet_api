using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = 1,
                    RoleCode = "R-001",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    CreatedBy = "Configure",
                    UpdatedBy = null
                },
                new Role
                {
                    Id = 2,
                    RoleCode = "R-002",
                    Name = "User",
                    NormalizedName = "USER",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    CreatedBy = "Configure",
                    UpdatedBy = null
                }
            );
        }
    }
}
