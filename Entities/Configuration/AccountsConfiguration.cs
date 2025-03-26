using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class AccountsConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasData(
                new Account
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Active = true,
                    Title = "Mr.",
                    Language = "English",
                    CreatedBy = "system",
                    UpdatedBy = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                }
            );
        }
    }
}
