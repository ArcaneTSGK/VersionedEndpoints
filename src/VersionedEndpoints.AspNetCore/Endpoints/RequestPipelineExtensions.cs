using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace VersionedEndpoints.AspNetCore.Endpoints;

internal static class RequestPipelineExtensions
{
    /// <summary>
    /// Maps inheritors of IEndpoint assigning them to their respective groups and versions.
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    /// <param name="substituteApiVersionInUrl"></param>
    internal static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder, bool substituteApiVersionInUrl)
    {
        var endpoints = endpointRouteBuilder
            .ServiceProvider
            .GetServices<IEndpoint>();

        var versionedApiGroups = new Dictionary<string, IVersionedEndpointRouteBuilder>();
        var routes = new Dictionary<string, RouteGroupBuilder>();

        foreach (var endpoint in endpoints)
        {
            var version = endpoint.ApiVersion;
            var groupName = endpoint.GroupName;

            var versionedApi = versionedApiGroups
                .GetOrAdd(groupName, () => endpointRouteBuilder
                    .NewVersionedApi());

            var compositeRouteKey = groupName + version;

            var routeGroupBuilder = routes
                .GetOrAdd(compositeRouteKey, () => versionedApi
                    .MapGroup(substituteApiVersionInUrl && version != ApiVersion.Neutral ? "api/v{version:apiVersion}" : "api")
                    .HasApiVersion(version));

            endpoint.AddEndpoint(routeGroupBuilder);
        }

        return endpointRouteBuilder;
    }

    /// <summary>
    /// Gets the value associated with the specified key or adds a new value if the key does not exist.
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    private static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key, Func<TValue> valueFactory)
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = valueFactory();
            dictionary[key] = value;
        }

        return value;
    }
}
