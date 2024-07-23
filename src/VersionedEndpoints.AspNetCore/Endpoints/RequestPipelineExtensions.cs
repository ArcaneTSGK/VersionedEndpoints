using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace VersionedEndpoints.AspNetCore.Endpoints;

public static class RequestPipelineExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder, bool substituteApiVersionInUrl)
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
                    .MapGroup(substituteApiVersionInUrl ? "api/v{version:apiVersion}" : "api")
                    .HasApiVersion(version));

            endpoint.AddEndpoint(routeGroupBuilder);
        }

        return endpointRouteBuilder;
    }

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
