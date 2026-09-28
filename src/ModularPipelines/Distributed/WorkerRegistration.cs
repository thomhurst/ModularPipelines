namespace ModularPipelines.Distributed;

/// <summary>
/// Announces a worker and the capabilities it offers.
/// </summary>
public sealed record WorkerRegistration
{
    /// <summary>Gets the registering worker.</summary>
    public required WorkerId WorkerId { get; init; }

    /// <summary>Gets the capabilities the worker advertises.</summary>
    public required IReadOnlyList<Capability> Capabilities { get; init; }

    /// <summary>
    /// Gets when the worker process registered. Together with <see cref="WorkerId"/> it identifies
    /// one worker session: repeating a registration with the same value is a reconnect, while a
    /// different value for a live worker identifier is a duplicate worker and is rejected.
    /// </summary>
    public required DateTimeOffset RegisteredAt { get; init; }

    /// <summary>Gets the maximum number of assignments the worker executes concurrently.</summary>
    public int MaxParallelism { get; init; } = 1;

    /// <summary>Gets the deterministic schema version for the worker's registered module set.</summary>
    public string PipelineSchemaVersion { get; init; } = string.Empty;

    /// <summary>Gets the pipeline execution this registration belongs to.</summary>
    public string? RunId { get; init; }
}
