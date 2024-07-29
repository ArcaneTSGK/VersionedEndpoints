using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VersionedEndpoints.AspNetCore.Swagger;

/// <summary>
/// Common extension methods for swagger support.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add swagger support.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="documentOptions"></param>
    /// <param name="swaggerGenOptions"></param>
    internal static IServiceCollection AddSwaggerDocument(
        this IServiceCollection services,
        ConfigurationManager config,
        Action<DocumentOptions>? documentOptions = null,
        Action<SwaggerGenOptions>? swaggerGenOptions = null)
    {
        services.Configure<OpenApiInfoOptions>(config.GetSection(nameof(OpenApiInfoOptions)));

        var document = new DocumentOptions();
        documentOptions?.Invoke(document);

        services
            .AddEndpointsApiExplorer()
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                swaggerGenOptions?.Invoke(options);

                if (document.EnableJwtBearerAuth)
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter Bearer {JWT}",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }

                if (document.EnableApiKeyAuth)
                {
                    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter your API key",
                        Name = "x-api-key",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "x-api-key"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "ApiKey"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }
            });

        return services;
    }
}
