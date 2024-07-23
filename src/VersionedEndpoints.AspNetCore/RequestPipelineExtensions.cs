using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using VersionedEndpoints.AspNetCore.Endpoints;
using VersionedEndpoints.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class RequestPipelineExtensions
{
    public static WebApplication UseVersionedEndpoints(
        this WebApplication app,
        bool substituteApiVersionInUrl = false,
        bool disableSwagger = false,
        Action<SwaggerOptions>? swaggerOptions = null,
        Action<SwaggerUIOptions>? swaggerUiOptions = null)
    {
        app.MapEndpoints(substituteApiVersionInUrl);

        if (!disableSwagger)
        {
            app.UseSwaggerDocument(swaggerOptions, swaggerUiOptions);
        }

        return app;
    }
}
