using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VersionedEndpoints.AspNetCore.ApiVersioning;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services,
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
