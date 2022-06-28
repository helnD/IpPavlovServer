using System;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using EasyData;
using EasyData.EntityFrameworkCore;
using EasyData.Services;
using Infrastructure;
using Infrastructure.Abstractions;
using Infrastructure.DataAccess;
using Infrastructure.Email;
using Infrastructure.FillingDatabase;
using Infrastructure.Images;
using Infrastructure.Implementations;
using Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using Unidecode.NET;
using UseCases.Leaders.GetLeaders;
using WebApplication.Services;
using WebApplication.Setup.Database;

namespace WebApplication;

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
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApplication", Version = "v1"});
        });

        services.AddControllersWithViews()
            .AddNewtonsoftJson(builder =>
            {
                builder.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
                builder.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                builder.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                builder.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        services.AddDbContext<AppDbContext>(
            option => option.UseNpgsql(
                Configuration.GetConnectionString("AppDatabase"),
                builder => builder.MigrationsAssembly("WebApplication")));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
        services.AddAsyncInitializer<DatabaseInitializer>();
        services.AddTransient<DataSeed>();
        services.AddTransient<ProductsDataSeed>();
        services.AddTransient<XmlSeederFacade>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IImageResizer, ImageResizer>();
        services.AddTransient<ILoggedUserAccessor, LoggedUserAccessor>();
        services.AddTransient(typeof(XmlSeeder<>), typeof(XmlSeeder<>));
        services.AddSingleton<Locker>();

        services.AddTransient<IExcelReader>(_ => new NpoiExcelReader("Files/price-list.xlsx", ""));

        services.AddTransient<Infrastructure.Abstractions.Unidecode>(_ => str => str.Unidecode());

        services.AddTransient<IImageApi>(_ => null);

        services.Configure<DatabaseInitialization>(Configuration.GetSection("DatabaseInitialization"));
        services.Configure<ImagesSettings>(Configuration.GetSection("Resources:Images"));
        services.Configure<FilesSettings>(Configuration.GetSection("Resources:Files"));
        services.Configure<SmtpConfiguration>(Configuration.GetSection("SmtpConfiguration"));

        services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication()
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromHours(3);

                options.LoginPath = "/auth";
                options.AccessDeniedPath = "/auth";
                options.SlidingExpiration = true;
            });

        services.AddMediatR(typeof(GetLeadersQuery).Assembly);

        services.AddAutoMapper(typeof(GetLeadersQuery).Assembly);

        services.AddCors();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1"));
        }

        app.UseRouting();
        app.UseStaticFiles();

        app.UseCors(builder =>
            builder.WithOrigins(Configuration.GetSection("FrontendOrigins:DevOrigin").Value,
                    Configuration.GetSection("FrontendOrigins:ReleaseOrigin").Value)
                .AllowAnyHeader()
                .AllowAnyMethod()
        );

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapEasyData(options =>
            {
                options.UseDbContext<AppDbContext>(opt =>
                {
                    opt.CustomizeModel(model =>
                    {
                        model.Entity<CooperationRequest>()
                            .SetEditable(false);

                        model.Entity<Question>()
                            .SetEditable(false);

                        model.Entity<User>()
                            .SetEditable(false);

                    });
                });
            });

            endpoints.MapControllerRoute(name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllers();
        });
    }
}