namespace ModularPipelines.Distributed;

/// <summary>
/// Metadata describing an artifact to be uploaded.
/// </summary>
public record ArtifactDescriptor(
    string Name,
    ModuleId ModuleId,
    string? ContentType = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
