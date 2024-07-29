using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VersionedEndpoints.AspNetCore.ApiVersioning;

/// <summary>
/// Extension methods that provide the ability to configure API versioning.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Sets up API versioning, by default will use UrlSegmentApiVersionReader, override by passing your own ApiVersioningOptions.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="apiVersioningOptions"></param>
    internal static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services,
        Action<ApiVersioningOptions>? apiVersioningOptions = null)
    {
        IApiVersionReader apiVersionReader = new UrlSegmentApiVersionReader();

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.UnsupportedApiVersionStatusCode = StatusCodes.Status404NotFound;
            options.ApiVersionReader = apiVersionReader;
            apiVersioningOptions?.Invoke(options);
            if (apiVersionReader is not UrlSegmentApiVersionReader)
            {
                apiVersionReader = options.ApiVersionReader;
            }
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";

            if (apiVersionReader is UrlSegmentApiVersionReader)
            {
                options.SubstituteApiVersionInUrl = true;
                options.ApiVersionParameterSource = apiVersionReader;
            }
        })
        .EnableApiVersionBinding();

        return services;
    }
}
