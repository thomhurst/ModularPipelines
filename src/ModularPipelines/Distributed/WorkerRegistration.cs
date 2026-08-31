using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Distributed;

public record WorkerRegistration(
    int WorkerIndex,
    [property: JsonConverter(typeof(ReadOnlySetJsonConverter))]
    IReadOnlySet<string> Capabilities,
    DateTimeOffset RegisteredAt)
{
    /// <summary>
    /// Gets the pipeline execution this registration belongs to, when available.
    /// </summary>
    public string? RunIdentifier { get; init; }

    /// <summary>
    /// Gets the worker's final count of commands executed outside a module context, when available.
    /// </summary>
    public int? UnattributedCommandCount { get; init; }

    /// <summary>
    /// Gets the worker's final command counts by stable module type identifier, when available.
    /// </summary>
    public IReadOnlyDictionary<string, int>? ModuleCommandCounts { get; init; }
}
