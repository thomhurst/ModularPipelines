namespace ModularPipelines.Distributed;

public record WorkerRegistration(
    int WorkerIndex,
    IReadOnlyList<Capability> Capabilities,
    DateTimeOffset RegisteredAt)
{
    /// <summary>
    /// Gets the deterministic schema version for the worker's registered module set.
    /// </summary>
    public string PipelineSchemaVersion { get; init; } = string.Empty;

    /// <summary>
    /// Gets the pipeline execution this registration belongs to, when available.
    /// </summary>
    public string? RunId { get; init; }
}
