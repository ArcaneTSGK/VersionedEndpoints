namespace VersionedEndpoints.AspNetCore.Swagger;

/// <summary>
/// Options for the swagger document.
/// </summary>
public class DocumentOptions
{
    /// <summary>
    /// A value indicating whether to enable JWT bearer authentication within swagger.
    /// </summary>
    public bool EnableJwtBearerAuth { get; set; }

    /// <summary>
    /// A value indicating whether to enable API key authentication within swagger.
    /// </summary>
    public bool EnableApiKeyAuth { get; set; }
}
