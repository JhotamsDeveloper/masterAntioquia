using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
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

            //For more information visit https://www.learnentityframeworkcore.com/migrations/seeding

            entityBuilder.HasData(
                new Category{
                    CategoryId = 1,
                    Name = "Hotel",
                    Icono = "Hotel",
                    Stated = true
                },new Category {
                    CategoryId = 2,
                    Name = "Restaurante",
                    Icono = "Restaurante",
                    Stated = true
                }, new Category
                {
                    CategoryId = 3,
                    Name = "Tienda",
                    Icono = "Tienda",
                    Stated = true
                }, new Category
                {
                    CategoryId = 4,
                    Name = "Blog",
                    Icono = "Blog",
                    Stated = true
                });
        }
    }
}
