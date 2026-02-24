namespace ModularPipelines.Distributed;

public record ModuleAssignment(
    string ModuleTypeName,
    string ResultTypeName,
    HashSet<string> RequiredCapabilities,
    string? MatrixTarget,
    DateTimeOffset AssignedAt,
    ModuleAssignmentConfig Configuration,
    IReadOnlyList<SerializedModuleResult>? DependencyResults = null);
