namespace ModularPipelines.Distributed;

public record ModuleAssignment(
    string ModuleTypeName,
    string ResultTypeName,
    IReadOnlyList<Capability> RequiredCapabilities,
    DateTimeOffset AssignedAt,
    ModuleAssignmentOptions Configuration,
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

    /// <summary>Gets when the assignment was enqueued for a worker.</summary>
    public DateTimeOffset EnqueuedAt { get; init; }

    public IReadOnlyList<string> SatisfiedConditionGroups { get; init; } = [];
}
