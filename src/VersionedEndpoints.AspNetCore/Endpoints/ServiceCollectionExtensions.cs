using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace VersionedEndpoints.AspNetCore.Endpoints;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// For each assembly, adds all endpoints that implement IEndpoint to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <param name="lifetime"></param>
    internal static IServiceCollection AddEndpointsFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        foreach (var assembly in assemblies)
        {
            services.AddEndpointsFromAssembly(assembly, lifetime);
        }

        return services;
    }

    /// <summary>
    /// For a given assembly, adds all endpoints that implement IEndpoint to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="lifetime"></param>
    internal static IServiceCollection AddEndpointsFromAssembly(this IServiceCollection services, Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var endpoints = assembly
            .GetTypes()
            .Where(type =>
                typeof(IEndpoint).IsAssignableFrom(type) &&
                type != typeof(IEndpoint) &&
                type is { IsAbstract: false });

        foreach (var endpoint in endpoints)
        {
            services.Add(new ServiceDescriptor(typeof(IEndpoint), endpoint, lifetime));
        }

        return services;
    }
}
