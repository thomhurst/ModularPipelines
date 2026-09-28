namespace ModularPipelines.Distributed;

/// <summary>
/// Identifies a dependency result stored by the distributed coordinator.
/// </summary>
public sealed record DependencyResultReference
{
    /// <summary>Gets the dependency module.</summary>
    public required ModuleId ModuleId { get; init; }

    /// <summary>Gets whether the master held the dependency's result when it created the assignment.</summary>
    public required bool IsAvailable { get; init; }
}
