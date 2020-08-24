using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Config
{
    public class PlaceConfig
    {
        public PlaceConfig(EntityTypeBuilder<Place> entityBuilder) 
        {
            entityBuilder.HasKey(x => x.PlaceId);

            //entityBuilder.Property(x => x.Nit)
            //    .IsRequired()
            //    .HasMaxLength(15);

            //entityBuilder.Property(x => x.Name)
            //    .IsRequired()
            //    .HasMaxLength(10);


            //entityBuilder.Property(x => x.Phone)
            //.IsRequired()
            //.HasMaxLength(10);

            ////entityBuilder.Property(x => x.Admin)
            ////.IsRequired()
            ////.HasMaxLength(20);

            //entityBuilder.Property(x => x.Address)
            //.IsRequired()
            //.HasMaxLength(20);

            //entityBuilder.Property(x => x.Description)
            //.IsRequired();

            //entityBuilder.Property(x => x.CoverPage)
            //.IsRequired();
            
            //entityBuilder.Property(x => x.Logo)
            //.IsRequired();

            //entityBuilder.Property(x => x.Contract)
            //.IsRequired()
            //.HasMaxLength(10);

            //entityBuilder.Property(x => x.State)
            //.IsRequired();

            //entityBuilder.Property(x => x.CreationDate)
            //.IsRequired();

            //DE UNO A MUCHOS
            entityBuilder.HasOne(x => x.Category)
                .WithMany(x => x.Places)
                .HasForeignKey(x => x.CategoryId);

        }
    }
}
