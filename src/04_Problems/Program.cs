using _04_Problems;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<OpenWeatherMapService>();
builder.Services.AddHttpClient();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

builder.Services.AddExceptionHandler<ProblemExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/weather", async (string city, string units, OpenWeatherMapService weatherMapService) =>
{
    var degreesUnit = units switch
    {
        "c" => "metric",
        "f" => "imperial",
        "k" => "standard",
        _ => "invalid"
    };

    if (degreesUnit.Equals("invalid"))
    {
        // return Results.Problem(type: "Bad Request",
        //  title: "Invalid units",
        //  detail: "Units can be only c, f or k",
        //  statusCode: StatusCodes.Status400BadRequest);

        throw new ProblemException("Invalid units!", "Units can only be c, f or k");
    }

    var weather = await weatherMapService.GetWeatherForCityAsync(city,units);

    return Results.Ok(weather);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

