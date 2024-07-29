using Asp.Versioning;
using Microsoft.OpenApi.Models;
using VersionedEndpoints.AspNetCore.Endpoints;

namespace VersionedEndpoints.Example.Endpoints.Games;

public class ListGames : IEndpoint
{
    public ApiVersion ApiVersion => ApiVersion.Neutral;

    public string GroupName => ApiEndpoints.Games.Tag.Name;

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(ApiEndpoints.Games.ListGames, () =>
            {
                return Results.Ok(new []
                {
                    new { Id = 1, Name = "Half-Life", Developer = "Valve" },
                    new { Id = 2, Name = "Half-Life 2", Developer = "Valve" }
                });
            })
            .WithName(nameof(ListGames))
            .MapToApiVersion(ApiVersion)
            .IsApiVersionNeutral()
            .WithOpenApi(ListGamesOperation);
    }

    private static OpenApiOperation ListGamesOperation(OpenApiOperation operation)
    {
        operation.Description = "List all games.";
        operation.Summary = "List games";
        operation.Tags = new List<OpenApiTag> { ApiEndpoints.Games.Tag };

        return operation;
    }
}
