using _03_APIVersioning.OpenApi;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(opts => opts.OperationFilter<SwaggerDefaultValues>());

builder.Services
    .AddApiVersioning(o =>
    {
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.ApiVersionReader = new QueryStringApiVersionReader();
        o.ApiVersionReader = new HeaderApiVersionReader("api-version");
        o.ApiVersionReader = new MediaTypeApiVersionReader("apiVersion");
        o.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(o =>
    {
        o.GroupNameFormat = "v'VVV";
        //o.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(1,0)
    .HasApiVersion(2,0)
    .ReportApiVersions() //response set eder
    .Build();

app
    .MapGet("hello", (HttpContext context) => 
    {
        var apiVersion = context.GetRequestedApiVersion();
        return $"hello world v{apiVersion!.ToString()}";
    })
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1);

app
    .MapGet("v{version:apiVersion}/hello", (HttpContext context) =>
    {
        var apiVersion = context.GetRequestedApiVersion();
        return $"hello world v{apiVersion!.ToString()}";
    })
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1);


app.MapGet("v1/hello", () => "hello world")
    .WithApiVersionSet(versionSet);

app.MapGet("v2/hello", () => "hello world from v2")
    .WithApiVersionSet(versionSet);

app.Run();