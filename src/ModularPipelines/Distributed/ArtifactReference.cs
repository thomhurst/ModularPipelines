namespace ModularPipelines.Distributed;

/// <summary>
/// A handle to a stored artifact, returned after upload and used for download/delete operations.
/// </summary>
public sealed record ArtifactReference
{
    /// <summary>Gets the store-specific artifact identifier.</summary>
    public required string ArtifactId { get; init; }

    /// <summary>Gets the artifact name declared by the producing module.</summary>
    public required string Name { get; init; }

    /// <summary>Gets the producing module.</summary>
    public required ModuleId ModuleId { get; init; }

    /// <summary>Gets the stored size in bytes.</summary>
    public required long SizeBytes { get; init; }

    /// <summary>Gets the content type of the stored data.</summary>
    public string? ContentType { get; init; }

    /// <summary>Gets when the artifact was uploaded, according to the uploader's clock.</summary>
    public required DateTimeOffset UploadedAt { get; init; }
}
