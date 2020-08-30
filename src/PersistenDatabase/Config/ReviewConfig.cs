using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persisten.Database.Config
{
    public class ReviewConfig
    {
        public ReviewConfig(EntityTypeBuilder<Review> entityBuilder)
        {
            entityBuilder.HasKey(x => x.ReviewID);

            entityBuilder.HasOne(x => x.Place)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}
