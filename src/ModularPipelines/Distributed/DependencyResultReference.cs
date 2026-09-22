namespace ModularPipelines.Distributed;

/// <summary>
/// Identifies a dependency result stored by the distributed coordinator.
/// </summary>
public record DependencyResultReference(
    ModuleId ModuleId,
    bool IsAvailable);
