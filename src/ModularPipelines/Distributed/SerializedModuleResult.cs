namespace ModularPipelines.Distributed;

/// <summary>
/// A module result in its transport format.
/// </summary>
public sealed record SerializedModuleResult
{
    /// <summary>Gets the module that produced the result.</summary>
    public required ModuleId ModuleId { get; init; }

    /// <summary>Gets the worker that produced the result.</summary>
    public required WorkerId WorkerId { get; init; }

    /// <summary>Gets the serialized module result.</summary>
    public required string Payload { get; init; }

    /// <summary>Gets when the worker finished the module, according to the worker's clock.</summary>
    public required DateTimeOffset CompletedAt { get; init; }

    /// <summary>
    /// Gets the artifacts the module uploaded. Consumers download these references rather than
    /// searching the artifact store.
    /// </summary>
    public IReadOnlyList<ArtifactReference> Artifacts { get; init; } = [];

    /// <summary>Gets the number of commands attributed to the module on its worker.</summary>
    public int CommandCount { get; init; }

    /// <summary>Gets worker-side distributed execution timing, when supplied by the worker.</summary>
    public DistributedModuleExecutionTelemetry? ExecutionTelemetry { get; init; }
}
