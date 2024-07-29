using Asp.Versioning;
using Microsoft.OpenApi.Models;
using VersionedEndpoints.AspNetCore.Endpoints;

namespace VersionedEndpoints.Example.Endpoints.Games.V1;

public class GetGame : IEndpoint
{
    public ApiVersion ApiVersion => new(ApiEndpoints.Games.V1.Version);
    public string GroupName => ApiEndpoints.Games.Tag.Name;

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(ApiEndpoints.Games.V1.GetGame, (int id) =>
        {
            return id switch
            {
                1 => Results.Ok(new { Id = 1, Name = "Half-Life" }),
                2 => Results.Ok(new { Id = 2, Name = "Half-Life 2" }),
                _ => Results.NotFound()
            };
        })
        .WithName(nameof(GetGame) + ApiVersion)
        .MapToApiVersion(ApiVersion)
        .HasDeprecatedApiVersion(1)
        .WithOpenApi(GetGameOperation);
    }

    private static OpenApiOperation GetGameOperation(OpenApiOperation operation)
    {
        operation.Description = "Get a game by its unique identifier.";
        operation.Summary = "Get a game";
        operation.Tags = new List<OpenApiTag> { ApiEndpoints.Games.Tag };

        operation.Deprecated = true;

        return operation;
    }
}
