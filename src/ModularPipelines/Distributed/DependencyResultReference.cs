namespace ModularPipelines.Distributed;

/// <summary>
/// Identifies a dependency result stored by the distributed coordinator.
/// </summary>
public record DependencyResultReference(
    string ModuleTypeName,
    bool IsAvailable);
