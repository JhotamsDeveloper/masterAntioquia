using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using Persisten.Database.Config;
using Service.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persisten.Database
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        //Validaciones
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new CategoryConfig(builder.Entity<Category>());
            new PlaceConfig(builder.Entity<Place>());
            new ProductConfig(builder.Entity<Product>());
            new GalleryConfig(builder.Entity<Gallery>());
        }
    }
}
