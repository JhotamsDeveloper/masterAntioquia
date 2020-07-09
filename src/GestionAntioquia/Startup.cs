using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persisten.Database;
using Service;
using Service.Commons;

namespace GestionAntioquia
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(
                options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPlaceService, PlaceService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IGalleryService, GalleryService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IBlogService, BlogService>();

            services.AddTransient<IUploadedFile, UploadedFile>();
            services.AddTransient<IFormatString, FormatString>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                name: "company",
                pattern: "hotel/{urlName}",
                defaults: new { Controller = "Company", action = "Details" });

                endpoints.MapControllerRoute(
                name: "blog",
                pattern: "blog/{name}",
                defaults: new { Controller = "blogs", action = "blog" });


                endpoints.MapControllerRoute(
                name: "hoteles",
                pattern: "hoteles",
                defaults: new { Controller = "company", action = "Allies" });

                
            });

        }
    }
}
