using Serilog;
using System.Diagnostics;
using System.Text;

namespace Presentation.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            context.Request.EnableBuffering();
            string requestBody;
            using (var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            await _next(context);

            var request = context.Request;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers["User-Agent"].ToString();
            // var user = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = context.User.Identity!.Name;

            if (context.Request.Method is "PUT" or "PATCH" or "DELETE")
                Log.Information("HTTP {RequestMethod} {RequestPath} received from {IpAddress} ({UserAgent}), " +
                    "Username: {user}, Request Body: {requestBody}",
                    request.Method, request.Path, ipAddress, userAgent, user, requestBody);
            else
                Log.Information("HTTP {RequestMethod} {RequestPath} received from {IpAddress} ({UserAgent}), " +
                    "Username: {user}",
                    request.Method, request.Path, ipAddress, userAgent, user);

            stopwatch.Stop();

            var response = context.Response;

            Log.Information("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms " +
                "to {IpAddress} ({UserAgent}), Username: {user}",
                request.Method, request.Path, response.StatusCode, stopwatch.ElapsedMilliseconds,
                ipAddress, userAgent, user);

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        context.Response.Body = originalBodyStream;
    }
}