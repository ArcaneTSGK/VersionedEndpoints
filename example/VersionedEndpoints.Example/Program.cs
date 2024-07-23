using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddProblemDetails();

// Default option is UrlSegmentApiVersionReader
builder.Services.AddVersionedEndpoints(config, apiVersioningOptions: options =>
{
    //options.ApiVersionReader = new QueryStringApiVersionReader();
}, documentOptions: options =>
{
    // Optionally enable one or both methods of authentication for Swagge
    options.EnableApiKeyAuth = true;
    options.EnableJwtBearerAuth = true;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseVersionedEndpoints(substituteApiVersionInUrl: true);

app.Run();
