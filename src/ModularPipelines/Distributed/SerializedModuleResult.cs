namespace ModularPipelines.Distributed;

public record SerializedModuleResult(
    string ModuleTypeName,
    string ResultTypeName,
    int WorkerIndex,
    string Payload,
    DateTimeOffset CompletedAt,
    IReadOnlyList<ArtifactReference>? Artifacts = null)
{
    /// <summary>
    /// Gets the number of commands attributed to the module on its worker.
    /// </summary>
    public int CommandCount { get; init; }
}
