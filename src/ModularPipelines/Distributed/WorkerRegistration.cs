namespace ModularPipelines.Distributed;

public record WorkerRegistration(
    int WorkerIndex,
    IReadOnlyList<Capability> Capabilities,
    DateTimeOffset RegisteredAt)
{
    /// <summary>
    /// Gets the pipeline execution this registration belongs to, when available.
    /// </summary>
    public string? RunIdentifier { get; init; }
}
