namespace _02_Middlewares;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;

    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = context.Request.Query["api_key"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            await context.Response.WriteAsync("API Key is required");
            return;
        }

        context.Response.Headers.Append("X-Transaction-Id", Guid.NewGuid().ToString());

        await _next(context);
    }
}

public static class TransactionMiddlewareExtensions
{
    public static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TransactionMiddleware>();
    }
}
