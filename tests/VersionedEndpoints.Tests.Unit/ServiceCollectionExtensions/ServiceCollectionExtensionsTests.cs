using System.Reflection;
using Asp.Versioning;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using VersionedEndpoints.AspNetCore.Endpoints;

namespace VersionedEndpoints.Tests.Unit.ServiceCollectionExtensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddVersionedEndpoints_ShouldAddApiVersioningConfiguration_WhenInvoked()
    {
        // Act
        var services = new ServiceCollection().AddVersionedEndpoints(new ConfigurationManager());

        // Assert
        services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(IApiVersionReader));
    }

    [Fact]
    public void AddVersionedEndpoints_ShouldAddEndpointsFromCallingAssembly_WhenAssembliesIsNull()
    {
        // Act
        var services = new ServiceCollection().AddVersionedEndpoints(new ConfigurationManager());

        // Assert
        services.Should().Contain(descriptor => descriptor.ServiceType == typeof(IEndpoint));
    }

    [Fact]
    public void AddVersionedEndpoints_ShouldAddEndpointsFromAssemblies_WhenAssembliesIsNotNull()
    {
        // Arrange
        var assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };

        // Act
        var services = new ServiceCollection().AddVersionedEndpoints(new ConfigurationManager(), assemblies);

        // Assert
        services.Should().Contain(descriptor => descriptor.ServiceType == typeof(IEndpoint));
    }

    [Fact]
    public void AddVersionedEndpoints_ShouldAddSwaggerDocument_WhenInvoked()
    {
        // Act
        var services = new ServiceCollection().AddVersionedEndpoints(new ConfigurationManager());

        // Assert
        services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(ISwaggerProvider));
        services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(ISchemaGenerator));
    }
}
