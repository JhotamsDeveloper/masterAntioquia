using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persisten.Database.Config
{
    public class ApplicationUserConfig
    {
        public ApplicationUserConfig(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entityBuilder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entityBuilder.Property(e => e.Country).IsRequired().HasMaxLength(100);

            entityBuilder.HasMany(e => e.UserRoles)
                         .WithOne(e => e.User)
                         .HasForeignKey(e => e.UserId)
                         .IsRequired();
        }
    }
}
