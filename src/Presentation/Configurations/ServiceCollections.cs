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
using Microsoft.AspNetCore.Identity;

namespace Presentation.Configurations;

public abstract class ServiceCollections
{
    public static void RegisterGeneralAppServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddMemoryCache();
    }

    public static void RegisterAuthenticationServices(IServiceCollection services, AppSettings? appSettings)
    {
        services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
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
        services.AddAuthorization();
    }


    public static void RegisterDatabaseServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        builder.Services.AddEntityFrameworkNpgsql()
                        .AddDbContext<BookStoreDbContext>((sp, options) =>
                        {
                            options.UseNpgsql(connectionString);
                            options.UseInternalServiceProvider(sp);
                        });
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
        services.AddScoped<IAuthenticationService, AuthenticationService>();
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
        services.AddSingleton(_ => new MapperConfiguration(m =>
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
        }).CreateMapper());

    public static void RegisterSerilogServices(IServiceCollection services, ConfigureHostBuilder hostBuilder, string connectionString)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information).Enrich
                     .FromLogContext().Enrich
                     .WithProperty("OS", Environment.OSVersion).Enrich
                     .WithProperty("MachineName", Environment.MachineName).Enrich
                     .WithProperty("ProcessId", Environment.ProcessId).Enrich
                     .WithProperty("ThreadId", Environment.CurrentManagedThreadId).WriteTo
                     .Console().WriteTo
                     .PostgreSQL(connectionString, "Logs", needAutoCreateTable: true, restrictedToMinimumLevel: LogEventLevel.Information)
                     .CreateLogger();

        hostBuilder.UseSerilog();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}