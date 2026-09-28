namespace ModularPipelines.Distributed;

/// <summary>
/// Metadata describing an artifact to be uploaded.
/// </summary>
public sealed record ArtifactDescriptor
{
    /// <summary>Gets the artifact name declared by the producing module.</summary>
    public required string Name { get; init; }

    /// <summary>Gets the producing module.</summary>
    public required ModuleId ModuleId { get; init; }

    /// <summary>Gets the content type of the uploaded data.</summary>
    public string? ContentType { get; init; }

    /// <summary>Gets optional store-specific metadata.</summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
