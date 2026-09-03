using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Distributed;

public record ModuleAssignment(
    string ModuleTypeName,
    string ResultTypeName,
    [property: JsonConverter(typeof(ReadOnlySetJsonConverter))]
    IReadOnlySet<Capability> RequiredCapabilities,
    DateTimeOffset AssignedAt,
    ModuleAssignmentConfiguration Configuration,
    IReadOnlyList<SerializedModuleResult>? DependencyResults = null)
{
    public IReadOnlyList<string> SatisfiedConditionGroups { get; init; } = [];
}
