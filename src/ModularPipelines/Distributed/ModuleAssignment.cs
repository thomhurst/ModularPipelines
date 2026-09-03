namespace ModularPipelines.Distributed;

public record ModuleAssignment(
    string ModuleTypeName,
    string ResultTypeName,
    IReadOnlyList<Capability> RequiredCapabilities,
    DateTimeOffset AssignedAt,
    ModuleAssignmentOptions Configuration,
    IReadOnlyList<SerializedModuleResult>? DependencyResults = null)
{
    /// <summary>
    /// Gets when the assignment was enqueued for a worker.
    /// </summary>
    public DateTimeOffset EnqueuedAt { get; init; }

    public IReadOnlyList<string> SatisfiedConditionGroups { get; init; } = [];
}
