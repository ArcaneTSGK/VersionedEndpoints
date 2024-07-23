using System.Reflection;
using Asp.Versioning;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using VersionedEndpoints.AspNetCore.ApiVersioning;
using VersionedEndpoints.AspNetCore.Endpoints;
using VersionedEndpoints.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVersionedEndpoints(
        this IServiceCollection services,
        ConfigurationManager config,
        IEnumerable<Assembly>? assemblies = null,
        Action<ApiVersioningOptions>? apiVersioningOptions = null,
        Action<DocumentOptions>? documentOptions = null,
        Action<SwaggerGenOptions>? swaggerGenOptions = null)
    {
        services.AddApiVersioningConfiguration(apiVersioningOptions);

        if (assemblies is not null)
        {
            services.AddEndpointsFromAssemblies(assemblies);
        }
        else
        {
            services.AddEndpointsFromAssembly(Assembly.GetCallingAssembly());
        }

        services.AddSwaggerDocument(config, documentOptions, swaggerGenOptions);

        return services;
    }
}
