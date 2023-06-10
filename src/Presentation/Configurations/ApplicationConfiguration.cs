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
using System.Reflection;
using Presentation.Filters;
using BookStore.Infrastructure.Seeding;

namespace Presentation.Configurations;

public abstract class ApplicationConfiguration
{
    public static void Configure(WebApplicationBuilder builder, AppSettings appSettings)
    {
        ConfigureGeneralAppServices(builder.Services);
        ConfigureAuthenticationServices(builder.Services, appSettings);
        ConfigureApplicationServices(builder.Services);
        ConfigureRepositories(builder.Services);
        ConfigureDatabaseServices(builder.Services, appSettings);
        ConfigureRedisCache(builder.Services, appSettings);
        ConfigureSwaggerServices(builder.Services);
        ConfigureAutoMapper(builder.Services);
        ConfigureSerilogServices(builder.Services, builder.Host, appSettings);
    }

    private static void ConfigureGeneralAppServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddMemoryCache();
    }

    private static void ConfigureAuthenticationServices(IServiceCollection services, AppSettings? appSettings)
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

    private static void ConfigureApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookCategoryService, BookCategoryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository>();
    }

    private static void ConfigureDatabaseServices(IServiceCollection services, AppSettings appSettings)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddEntityFrameworkNpgsql()
                        .AddDbContext<BookStoreDbContext>((sp, options) =>
                        {
                            options.UseNpgsql(appSettings!.ConnectionStrings.DefaultConnection);
                            options.UseInternalServiceProvider(sp);
                        });

        ApplyDatabaseMigrations(services);
        EnsureDataPopulated(services);
    }

    private static void ApplyDatabaseMigrations(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var _bookStoreDbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

        _bookStoreDbContext.Database.Migrate();
    }

    private static void EnsureDataPopulated(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var _repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        DataSeeder.SeedBooks(_repository).Wait();
        DataSeeder.SeedCategories(_repository).Wait();
        IdentitySeeder.SeedAspNetRoles(_roleManager).Wait();
        IdentitySeeder.SeedAdminUser(_userManager).Wait();
    }

    private static void ConfigureRedisCache(IServiceCollection services, AppSettings config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.RedisCache.ConnectionString;
            options.InstanceName = config.RedisCache.InstanceName;
        });
    }


    private static void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var solutionName = Assembly.GetEntryAssembly()?.GetName().Name;
            c.SwaggerDoc("v1", new OpenApiInfo { Title = solutionName, Version = "v1" });

            // Add the JWT bearer authentication scheme to Swagger
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            };
            c.AddSecurityDefinition("Bearer", securityScheme);

            // Add the JWT bearer authentication requirement to Swagger operations
            var securityRequirement = new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
            };
            c.AddSecurityRequirement(securityRequirement);

            // Add the JwtBearer token filter
            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }

    private static void ConfigureAutoMapper(IServiceCollection services) =>
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
            m.CreateMap<User, UserInfoDto>().ReverseMap();
        }).CreateMapper());

    private static void ConfigureSerilogServices(IServiceCollection services, ConfigureHostBuilder hostBuilder, AppSettings appSettings)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information).Enrich
                     .FromLogContext().Enrich
                     .WithProperty("OS", Environment.OSVersion).Enrich
                     .WithProperty("MachineName", Environment.MachineName).Enrich
                     .WithProperty("ProcessId", Environment.ProcessId).Enrich
                     .WithProperty("ThreadId", Environment.CurrentManagedThreadId).WriteTo
                     .Console().WriteTo
                     .PostgreSQL(appSettings.ConnectionStrings.DefaultConnection, "Logs", needAutoCreateTable: true, restrictedToMinimumLevel: LogEventLevel.Information)
                     .CreateLogger();

        hostBuilder.UseSerilog();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}