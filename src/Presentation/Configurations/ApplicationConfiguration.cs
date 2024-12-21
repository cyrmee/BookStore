using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure;
using AutoMapper;
using Serilog.Events;
using Serilog;
using Infrastructure.Interceptors;
using Domain.Models;
using Application.DTOs;
using Application.Services;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using BookStore.Infrastructure.Seeding;
using Application.Jobs;
using Hangfire;

namespace Presentation.Configurations;

public abstract class ApplicationConfiguration
{
    // This is the main configuration method that sets up all services and dependencies for the application
    // It calls a series of other configuration methods to:
    // - Configure general app services like controllers, API explorer, CORS
    // - Set up authentication with JWT bearer tokens
    // - Register application services and repositories
    // - Configure the database connection and run migrations
    // - Set up Redis caching
    // - Configure AutoMapper for object mapping
    // - Set up Serilog logging
    // - Configure Hangfire for background jobs
    public static void Configure(WebApplicationBuilder builder)
    {
        ConfigureGeneralAppServices(builder);
        ConfigureAuthenticationServices(builder);
        ConfigureApplicationServices(builder);
        ConfigureRepositories(builder);
        ConfigureDatabaseServices(builder);
        ConfigureRedisCache(builder);
        ConfigureAutoMapper(builder);
        ConfigureSerilogServices(builder);
        ConfigureHangfireServices(builder);
    }
    private static void ConfigureGeneralAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddMemoryCache();

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
    }

    private static void ConfigureAuthenticationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreDbContext>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
                ValidAudience = builder.Configuration["JwtBearer:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        builder.Services.AddAuthorization();
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IBookCategoryService, BookCategoryService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<ITokenCleanupJob, TokenCleanupJob>();
    }

    private static void ConfigureRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRepository, Repository>();
    }

    private static void ConfigureDatabaseServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        builder.Services.AddEntityFrameworkNpgsql()
                        .AddDbContext<BookStoreDbContext>((sp, options) =>
                        {
                            options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                            options.UseInternalServiceProvider(sp);
                        });

        ApplyDatabaseMigrations(builder);
        EnsureDataPopulated(builder);
    }

    private static void ApplyDatabaseMigrations(WebApplicationBuilder builder)
    {
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var _bookStoreDbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

        _bookStoreDbContext.Database.Migrate();
    }

    private static void EnsureDataPopulated(WebApplicationBuilder builder)
    {
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var _repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        DataSeeder.SeedBooks(_repository).Wait();
        DataSeeder.SeedCategories(_repository).Wait();
        IdentitySeeder.SeedRoles(_roleManager).Wait();
        IdentitySeeder.SeedUsers(_userManager).Wait();
    }

    private static void ConfigureRedisCache(WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration["RedisCache:ConnectionString"];
            options.InstanceName = builder.Configuration["RedisCache:InstanceName"];
        });
    }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder) =>
        builder.Services.AddSingleton(_ => new MapperConfiguration(m =>
        {
            m.CreateMap<Book, BookDto>().ReverseMap();
            m.CreateMap<Book, BookWriteDto>().ReverseMap();

            m.CreateMap<BookCategory, BookCategoryDto>().ReverseMap();
            m.CreateMap<BookCategory, BookCategoryWriteDto>().ReverseMap();

            m.CreateMap<Category, CategoryDto>().ReverseMap();
            m.CreateMap<Category, CategoryWriteDto>().ReverseMap();

            m.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            m.CreateMap<OrderDetail, OrderDetailWriteDto>().ReverseMap();

            m.CreateMap<Order, OrderDto>().ReverseMap();
            m.CreateMap<Order, OrderWriteDto>().ReverseMap();

            m.CreateMap<User, UserSignupDto>().ReverseMap();
            m.CreateMap<User, UserInfoDto>().ReverseMap();
        }).CreateMapper());

    private static void ConfigureSerilogServices(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information).Enrich
                     .FromLogContext().Enrich
                     .WithProperty("OS", Environment.OSVersion).Enrich
                     .WithProperty("MachineName", Environment.MachineName).Enrich
                     .WithProperty("ProcessId", Environment.ProcessId).Enrich
                     .WithProperty("ThreadId", Environment.CurrentManagedThreadId).WriteTo
                     .Console().WriteTo
                     .PostgreSQL(builder.Configuration["ConnectionStrings:DefaultConnection"], "Logs", needAutoCreateTable: true, restrictedToMinimumLevel: LogEventLevel.Information)
                     .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }

    private static void ConfigureHangfireServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(config => config.UseInMemoryStorage());
        builder.Services.AddHangfireServer();
    }
}