using Asp.Versioning;
using Microsoft.AspNetCore.Routing;

namespace VersionedEndpoints.AspNetCore.Endpoints;

/// <summary>
/// The common interface to use for all Minimal API Endpoints.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// The version of the endpoint.
    /// </summary>
    ApiVersion ApiVersion { get; }

    /// <summary>
    /// The name of the group the endpoint belongs to.
    /// </summary>
    string GroupName { get; }

    /// <summary>
    /// Allows the endpoint to be added to the endpoint route builder.
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}
