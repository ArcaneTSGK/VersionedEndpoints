using System.Reflection;
using Asp.Versioning;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using VersionedEndpoints.AspNetCore.ApiVersioning;
using VersionedEndpoints.AspNetCore.Endpoints;
using VersionedEndpoints.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods that provide the ability to configure API versioning and add endpoints.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// By default, adds all endpoints inherited from IEndpoint in the calling assembly with swagger support.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="assemblies"></param>
    /// <param name="apiVersioningOptions"></param>
    /// <param name="documentOptions"></param>
    /// <param name="swaggerGenOptions"></param>
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
