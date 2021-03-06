﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Config
{
    class CategoryConfig
    {

        public CategoryConfig(EntityTypeBuilder<Category> entityBuilder) 
        {
            entityBuilder.HasKey(x => x.CategoryId);

            entityBuilder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);
            
            entityBuilder.Property(x => x.Stated)
                .IsRequired();

        }
    }
}
