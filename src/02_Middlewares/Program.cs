using _02_Middlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    await next.Invoke();
});



app.RegisterMiddleware();

app.MapGet("/", (HttpContext context) => "test");

app.Run();
