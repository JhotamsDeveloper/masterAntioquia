using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persisten.Database.Config
{
    public class ApplicationRoleConfig
    {
        public ApplicationRoleConfig(EntityTypeBuilder<ApplicationRole> entityBuilder)
        {
            entityBuilder.HasMany(e => e.UserRoles)
                         .WithOne(e => e.Role)
                         .HasForeignKey(e => e.RoleId)
                         .IsRequired();

            entityBuilder.HasData(
                new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin"
                },
                new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "UserApp",
                    NormalizedName = "UserApp"
                }
            );
        }

    }
}
