namespace VersionedEndpoints.AspNetCore.Swagger;

public class OpenApiInfoOptions
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string ContactName { get; init; } = default!;
    public string ContactEmail { get; init; } = default!;
    public string AdditionalInfo { get; init; } = default!;
}
