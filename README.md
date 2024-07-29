# Versioned Endpoints

## Motivation

This project was born out of repeatedly registering the same dependencies to version REST APIs.

It combines a few well known libraries to provide a simple way to version minimal APIs.

Before proceeding down this opinionated path, you may find that FastEndpoints is a better fit for your needs:

https://github.com/FastEndpoints/FastEndpoints

## Libraries Used

For versioning the API [aspnet-api-versioning](https://github.com/dotnet/aspnet-api-versioning).

For documenting the API [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Usage

Register the services in your `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration();

builder.Services.AddVersionedEndpoints(config);

var app = builder.Build();

app.UseVersionedEndpoints();

app.Run();
```

Create an endpoint and inherit from `IEndpoint`

```csharp
public class ExampleEndpoint : IEndpoint
{
    public ApiVersion ApiVersion => ApiVersion.Default;

    public string GroupName => "Example";

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/example", () => {
            return Result.Ok("Example");
        }).MapToApiVersion(ApiVersion);
    }
}
```

All endpoints that inherit from IEndpoint will be registered with the API versioning middleware.

See the example project for a complete example.
