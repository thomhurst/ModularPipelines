using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Distributed;

public record WorkerRegistration(
    int WorkerIndex,
    [property: JsonConverter(typeof(ReadOnlySetJsonConverter))]
    IReadOnlySet<string> Capabilities,
    DateTimeOffset RegisteredAt);
