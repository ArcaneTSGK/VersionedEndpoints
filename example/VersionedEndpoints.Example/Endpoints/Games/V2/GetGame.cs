using Asp.Versioning;
using Microsoft.OpenApi.Models;
using VersionedEndpoints.AspNetCore.Endpoints;

namespace VersionedEndpoints.Example.Endpoints.Games.V2;

public class GetGame : IEndpoint
{
    public ApiVersion ApiVersion => new(ApiEndpoints.Games.V2.Version);
    public string GroupName => ApiEndpoints.Games.Tag.Name;

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(ApiEndpoints.Games.V2.GetGame, (int id) =>
        {
            return id switch
            {
                1 => Results.Ok(new { Id = 1, Name = "Half-Life", Developer = "Valve" }),
                2 => Results.Ok(new { Id = 2, Name = "Half-Life 2", Developer = "Valve" }),
                _ => Results.NotFound()
            };
        })
        .WithName(nameof(GetGame) + ApiVersion)
        .MapToApiVersion(ApiVersion)
        .WithOpenApi(GetGameOperation);
    }

    private static OpenApiOperation GetGameOperation(OpenApiOperation operation)
    {
        operation.Description = "Get a game by its unique identifier.";
        operation.Summary = "Get a game";
        operation.Tags = new List<OpenApiTag> { ApiEndpoints.Games.Tag };

        return operation;
    }
}
