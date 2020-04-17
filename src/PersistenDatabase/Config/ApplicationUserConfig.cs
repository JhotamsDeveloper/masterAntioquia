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
        }
    }
}
