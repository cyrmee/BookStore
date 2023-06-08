using Presentation.Configurations;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.Get<AppSettings>();

ServiceCollections.RegisterGeneralAppServices(builder.Services);
ServiceCollections.RegisterAuthenticationServices(builder.Services, config);
ServiceCollections.RegisterDatabaseServices(builder, config!.ConnectionStrings.DefaultConnection);
ServiceCollections.RegisterRedisCache(builder.Services, config);
ServiceCollections.RegisterApplicationServices(builder.Services);
ServiceCollections.RegisterRepositories(builder.Services);
ServiceCollections.RegisterSwaggerServices(builder.Services);
ServiceCollections.RegisterAutoMapper(builder.Services);
ServiceCollections.RegisterSerilogServices(builder.Services, builder.Host, config.ConnectionStrings.DefaultConnection);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
