using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace VersionedEndpoints.AspNetCore.Endpoints;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpointsFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        foreach (var assembly in assemblies)
        {
            services.AddEndpointsFromAssembly(assembly, lifetime);
        }

        return services;
    }

    public static IServiceCollection AddEndpointsFromAssembly(this IServiceCollection services, Assembly assembly,
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
