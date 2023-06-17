using Hangfire;
using Presentation.Configurations;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ApplicationConfiguration.Configure(builder);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();

JobConfiguration.Configure();

app.Run();