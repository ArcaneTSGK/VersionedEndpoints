using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace VersionedEndpoints.AspNetCore.Swagger;

public static class RequestPipelineExtensions
{
    public static WebApplication UseSwaggerDocument(
        this WebApplication app,
        Action<SwaggerOptions>? options = null,
        Action<SwaggerUIOptions>? uiOptions = null)
    {
        var apiVersions = app.DescribeApiVersions();

        app.UseSwagger(options)
            .UseSwaggerUI(config =>
            {
                foreach (var description in apiVersions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    config.SwaggerEndpoint(url, name);
                }

                uiOptions?.Invoke(config);
            });

        return app;
    }
}
