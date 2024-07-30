using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VersionedEndpoints.AspNetCore.Endpoints;

namespace VersionedEndpoints.Tests.Unit.Endpoints;

public class DummyVersionedEndpoint : IEndpoint
{
    public ApiVersion ApiVersion => ApiVersion.Default;
    public string GroupName => "Dummy";

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/dummy", () => Results.Ok("Dummy")).WithDisplayName("VersionedDummy");
    }
}
