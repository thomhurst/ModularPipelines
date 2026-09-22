namespace ModularPipelines.Distributed;

/// <summary>
/// A handle to a stored artifact, returned after upload and used for download/delete operations.
/// </summary>
public record ArtifactReference(
    string ArtifactId,
    string Name,
    ModuleId ModuleId,
    long SizeBytes,
    string? ContentType,
    DateTimeOffset UploadedAt);
