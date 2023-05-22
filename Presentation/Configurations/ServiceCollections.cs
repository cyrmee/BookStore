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

namespace Presentation.Configurations;

public abstract class ServiceCollections
{
    public static void RegisterGeneralAppServices(IServiceCollection builder)
    {
        builder.AddControllers();
        builder.AddEndpointsApiExplorer();  
        builder.AddAuthorization(options =>
        {

        });
    }

    public static void RegisterAuthenticationServices(IServiceCollection builderServices, AppSettings? appSettings) =>
        builderServices.AddAuthentication(options =>
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
        builder.Services.AddDbContext<BookStoreDbContext>(opt =>
                opt.UseNpgsql(connectionString));
        builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        ApplyDatabaseMigrations(builder.Services);
    }

    private static void ApplyDatabaseMigrations(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var dbContext = serviceProvider.GetRequiredService<BookStoreDbContext>();

        dbContext.Database.Migrate();
    }

    public static void RegisterApplicationServices(IServiceCollection builderServices)
    {
        builderServices.AddScoped<IBookService, BookService>();
        builderServices.AddScoped<IBookCategoryService, BookCategoryService>();
        builderServices.AddScoped<ICategoryService, CategoryService>();
        builderServices.AddScoped<IOrderService, OrderService>();
        builderServices.AddScoped<IOrderDetailService, OrderDetailService>();
    }

    public static void RegisterRepositories(IServiceCollection builderServices)
    {
        builderServices.AddScoped<IRepository, Repository>();
    }

    public static void RegisterSwaggerServices(IServiceCollection builderServices) =>
        builderServices.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Store API", Version = "v1" });
        });


    public static void RegisterAutoMapper(IServiceCollection builderServices) =>
        builderServices.AddSingleton(_ => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Book, BookDto>().ReverseMap();
            cfg.CreateMap<BookCategory, BookCategoryDto>().ReverseMap();
            cfg.CreateMap<Category, CategoryDto>().ReverseMap();
            cfg.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            cfg.CreateMap<Order, OrderDto>().ReverseMap();
        }).CreateMapper());
        

    public static void RegisterSerilogServices(IServiceCollection builderServices, ConfigureHostBuilder hostBuilder, string connectionString)
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

        builderServices.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}