using System.Text;
using System.Text.Json;

namespace Zajezdnia.Middleware;

public class AuthMiddleware(RequestDelegate next)
{
    private const string CookieName = "ZajezdniaAuth";

    public async Task InvokeAsync(HttpContext context)
    {
        context.Items["IsLoggedIn"] = false;

        if (context.Request.Cookies.TryGetValue(CookieName, out var encoded))
        {
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                var payload = JsonSerializer.Deserialize<SessionPayload>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (payload is { Id: > 0 })
                {
                    context.Items["IsLoggedIn"]   = true;
                    context.Items["UserId"]        = payload.Id;
                    context.Items["UserLogin"]     = payload.Login;
                    context.Items["UserImie"]      = payload.Imie;
                    context.Items["UserNazwisko"]  = payload.Nazwisko;
                }
            }
            catch { }
        }

        await next(context);
    }

    private record SessionPayload(int Id, string Login, string Imie, string Nazwisko);
}

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseZajezdniaAuth(this IApplicationBuilder app)
        => app.UseMiddleware<AuthMiddleware>();
}