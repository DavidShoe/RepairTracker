using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

public class SessionCheckMiddleware
{
    private readonly RequestDelegate _next;

    public SessionCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if session is initialized by a custom session flag
        if (context.Session.GetString("IsSessionInitialized") == null)
        {
            // First-time entry, set session flag
            context.Session.SetString("IsSessionInitialized", "true");
            Debug.WriteLine($"SessionCheckMiddleware: First entry - initializing session: {context.Request.Path.Value}");

            // Proceed without showing session expired message or redirecting
            await _next(context);
            return;
        }


        var userName = context.Session.GetString("UserName");
        Debug.WriteLine($"SessionCheckMiddleware: UserName: {userName}");

        // Check if the session has expired
        if (userName == null)
        {
            Debug.WriteLine($"SessionCheckMiddleware: Session expired: {context.Request.Path.Value}");

            // Normalize and check if one of the pass through pages (Home / login)
            string currentPath = context.Request.Path.Value?.TrimEnd('/').ToLowerInvariant() ?? string.Empty;
            if (
                currentPath.Equals("/home/index") ||
                currentPath.Equals("/api/loginowner") ||
                currentPath.Equals("/api/logintech")
                )
            {
                Debug.WriteLine("SessionCheckMiddleware: passthrough page");
            }
            else
            {
                Debug.WriteLine($"SessionCheckMiddleware: Not on home page {context.Request.Path.Value}");

                // Redirect to the login page with a query parameter indicating session expiration
                context.Response.Redirect("/Home/Index?sessionExpired=true");
                return;
            }
        }

        await _next(context);
    }
}
