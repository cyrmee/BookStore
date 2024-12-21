using Hangfire;
using Scalar.AspNetCore;
using Presentation.Configurations;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ApplicationConfiguration.Configure(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    // app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();
app.UseCors();

JobConfiguration.Configure();

app.Run();
