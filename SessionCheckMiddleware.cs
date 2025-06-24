using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

public class SessionCheckMiddleware
{
    private readonly RequestDelegate _next;

    public SessionCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Use Identity authentication instead of session for user checks
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            string currentPath = context.Request.Path.Value?.TrimEnd('/').ToLowerInvariant() ?? string.Empty;
            // Allow access to login, register, static files, and home page
            if (
                currentPath.StartsWith("/identity/account/login") ||
                currentPath.StartsWith("/identity/account/register") ||
                currentPath.StartsWith("/identity/account/externallogin") ||
                currentPath.StartsWith("/identity/account/forgotpassword") ||
                currentPath.StartsWith("/identity/account/resetpassword") ||
                currentPath.StartsWith("/identity/account/lockout") ||
                currentPath.StartsWith("/identity/account/confirmemail") ||
                currentPath.StartsWith("/identity/account/confirmemailchange") ||
                currentPath.StartsWith("/identity/account/logout") ||
                currentPath.StartsWith("/static") ||
                currentPath.StartsWith("/css") ||
                currentPath.StartsWith("/js") ||
                currentPath.StartsWith("/lib") ||
                currentPath.StartsWith("/images") ||
                currentPath.Equals("/home/index") ||
                currentPath.Equals("/")
            )
            {
                // Allow these pages
                await _next(context);
                return;
            }
            // Redirect unauthenticated users to Identity login page
            context.Response.Redirect("/Identity/Account/Login?returnUrl=" + context.Request.Path + context.Request.QueryString);
            return;
        }
        await _next(context);
    }
}
