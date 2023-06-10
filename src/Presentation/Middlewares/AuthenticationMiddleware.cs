using Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var _authenticationService = context.RequestServices.GetRequiredService<IAuthenticationService>();

        var authorizeData = endpoint?.Metadata?.GetMetadata<IAuthorizeData>();
        var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>();

        if (authorizeData != null && allowAnonymous == null)
        {
            // The endpoint requires authorization
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (await _authenticationService.IsTokenRevoked(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}
