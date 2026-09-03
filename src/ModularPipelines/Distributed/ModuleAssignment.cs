namespace ModularPipelines.Distributed;

public record ModuleAssignment(
    string ModuleTypeName,
    string ResultTypeName,
    IReadOnlyList<Capability> RequiredCapabilities,
    DateTimeOffset AssignedAt,
    ModuleAssignmentOptions Configuration,
    IReadOnlyList<SerializedModuleResult>? DependencyResults = null)
{
    public IReadOnlyList<string> SatisfiedConditionGroups { get; init; } = [];
}
