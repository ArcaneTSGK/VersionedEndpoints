using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VersionedEndpoints.AspNetCore.Endpoints;

namespace VersionedEndpoints.Tests.Unit.RequestPipelineExtensions;

public class RequestPipelineExtensionsTests
{
    [Fact]
    public void MapEndpoints_ShouldMapEndpoints_WhenInvoked()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddVersionedEndpoints(new ConfigurationManager());

        var app = builder.Build();

        // Act
        var result = app.MapEndpoints(true);

        // Assert
        result.DataSources.First().Endpoints.First().DisplayName.Should().Be("NeutralDummy");
        result.DataSources.First().Endpoints.Last().DisplayName.Should().Be("VersionedDummy");
    }
}
