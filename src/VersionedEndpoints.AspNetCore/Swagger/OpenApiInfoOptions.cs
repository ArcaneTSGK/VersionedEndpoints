namespace VersionedEndpoints.AspNetCore.Swagger;

/// <summary>
/// Information about the OpenAPI document which can be provided by configuration.
/// </summary>
public class OpenApiInfoOptions
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string ContactName { get; init; } = default!;
    public string ContactEmail { get; init; } = default!;
    public string AdditionalInfo { get; init; } = default!;
}
