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
    IReadOnlyList<DependencyResultReference>? DependencyResultReferences = null)
{
    /// <summary>
    /// Gets the user-configured scheduling priority.
    /// </summary>
    public ModulePriority Priority { get; init; } = ModulePriority.Normal;

    /// <summary>
    /// Gets the estimated duration of the longest downstream path starting at this module.
    /// </summary>
    public TimeSpan CriticalPathWeight { get; init; }

    public IReadOnlyList<string> SatisfiedConditionGroups { get; init; } = [];
}
