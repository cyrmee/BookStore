using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Presentation.Configurations;

public abstract class ServiceCollections
{
    public static void RegisterGeneralAppServices(IServiceCollection builder)
    {
        builder.AddControllers();
        builder.AddEndpointsApiExplorer();  
        builder.AddAuthorization();
        builder.AddMemoryCache();
    }

    public static void RegisterAuthenticationServices(IServiceCollection services, AppSettings? appSettings) =>
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = appSettings!.JwtBearer.Issuer,
                ValidAudience = appSettings.JwtBearer.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtBearer.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });


    public static void RegisterDatabaseServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        builder.Services.AddDbContext<BookStoreDbContext>(opt =>
                opt.UseNpgsql(connectionString));
        ApplyDatabaseMigrations(builder.Services);
    }

    private static void ApplyDatabaseMigrations(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var dbContext = serviceProvider.GetRequiredService<BookStoreDbContext>();

        dbContext.Database.Migrate();
    }

    public static void RegisterRedisCache(IServiceCollection services, AppSettings config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.RedisCache.ConnectionString;
            options.InstanceName = config.RedisCache.InstanceName;
        });
    }

    public static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookCategoryService, BookCategoryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
    }

    public static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository>();
    }

    public static void RegisterSwaggerServices(IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Store API", Version = "v1" });
        });


    public static void RegisterAutoMapper(IServiceCollection services) =>
        services.AddSingleton(_ => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Book, BookDto>().ReverseMap();
            cfg.CreateMap<Book, BookWriteDto>().ReverseMap();

            cfg.CreateMap<BookCategory, BookCategoryDto>().ReverseMap();
            cfg.CreateMap<BookCategory, BookCategoryWriteDto>().ReverseMap();
            
            cfg.CreateMap<Category, CategoryDto>().ReverseMap();
            cfg.CreateMap<Category, CategoryWriteDto>().ReverseMap();

            cfg.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            cfg.CreateMap<OrderDetail, OrderDetailWriteDto>().ReverseMap();

            cfg.CreateMap<Order, OrderDto>().ReverseMap();
            cfg.CreateMap<Order, OrderWriteDto>().ReverseMap();
        }).CreateMapper());
        

    public static void RegisterSerilogServices(IServiceCollection services, ConfigureHostBuilder hostBuilder, string connectionString)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("OS", Environment.OSVersion)
        .Enrich.WithProperty("MachineName", Environment.MachineName)
        .Enrich.WithProperty("ProcessId", Environment.ProcessId)
        .Enrich.WithProperty("ThreadId", Environment.CurrentManagedThreadId)
        .WriteTo.Console()
        .WriteTo.PostgreSQL(connectionString, "Logs", needAutoCreateTable: true, restrictedToMinimumLevel: LogEventLevel.Information)
        .CreateLogger();

        hostBuilder.UseSerilog();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}