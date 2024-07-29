using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using VersionedEndpoints.AspNetCore.Endpoints;
using VersionedEndpoints.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Extension methods that provide the ability to configure the request pipeline.
/// </summary>
public static class RequestPipelineExtensions
{
    /// <summary>
    /// By default, adds all endpoints that implement IEndpoint and adds swagger support.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="substituteApiVersionInUrl"></param>
    /// <param name="disableSwagger"></param>
    /// <param name="swaggerOptions"></param>
    /// <param name="swaggerUiOptions"></param>
    public static WebApplication UseVersionedEndpoints(
        this WebApplication app,
        bool substituteApiVersionInUrl = true,
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
