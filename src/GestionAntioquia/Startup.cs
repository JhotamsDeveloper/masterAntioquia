using AutoMapper;
using GestionAntioquia.Config.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persisten.Database;
using Service;
using Service.Commons;
using System;

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
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //Cambiar todas las duraciones del token de protección de datos
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromDays(7));

            //Para más información visite https://docs.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.identity.identityoptions?view=aspnetcore-3.1
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPlaceService, PlaceService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IGalleryService, GalleryService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IGenericServicio, GenericServicio>();
            services.AddTransient<ITouristExcursionsService, TouristExcursionsService>();

            services.AddTransient<IUploadedFile, UploadedFile>();
            services.AddTransient<IUploadedFileAzure, UploadedFileAzure>();
            services.AddTransient<IFormatString, FormatString>();

            services.AddTransient<IEmailSendGrid, EmailSendGrid>();

            services.AddControllersWithViews(option=> 
            {
                option.Filters.Add(typeof(ErrorFilter));
            });
            services.AddRazorPages();
        }

        // Este método es llamado por el tiempo de ejecución. Use este método para configurar la canalización de solicitudes HTTP.
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
                endpoints.MapRazorPages();

                //Places - Company
                endpoints.MapControllerRoute(
                name: "DetailsSouvenir",
                pattern: "souvenir/{urlName}",
                defaults: new { Controller = "Company", action = "Details" });

                endpoints.MapControllerRoute(
                name: "souvenir",
                pattern: "souvenir",
                defaults: new { Controller = "company", action = "Allies" });

                //Products
                endpoints.MapControllerRoute(
                name: "WhereToSleep",
                pattern: "donde-dormir",
                defaults: new { Controller = "products", action = "WhereToSleep" });

                endpoints.MapControllerRoute(
                name: "toursDetail",
                pattern: "tour/{urlName}",
                defaults: new { Controller = "TouristExcursions", action = "TourDetail" });

                //Tours
                endpoints.MapControllerRoute(
                name: "blog",
                pattern: "blog/{name}",
                defaults: new { Controller = "blogs", action = "blog" });
                
                //Default
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            });

        }
    }
}
