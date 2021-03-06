﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persisten.Database.Config
{
    public class GalleryConfig
    {
        public GalleryConfig(EntityTypeBuilder<Gallery> entityBuilder)
        {
            entityBuilder.HasKey(x => x.GalleryId);

            entityBuilder.HasOne(x => x.Products)
                .WithMany(x => x.Galleries)
                .HasForeignKey(x => x.ProducId);

            entityBuilder.HasOne(x => x.Reviews)
            .WithMany(x => x.Galleries)
            .HasForeignKey(x => x.ReviewsId);

        }
    }
}
