using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persisten.Database.Config
{
    public class EventConfig
    {
        public EventConfig(EntityTypeBuilder<Event> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.EventId);

            //DE UNO A MUCHOS
            entityTypeBuilder.HasOne(x => x.Category)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CategoryId);

            //DE UNO A MUCHOS
            entityTypeBuilder.HasOne(x => x.Place)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.PlaceId);
        }
    }
}
