namespace ModularPipelines.Distributed;

public record ModuleAssignment(
    string ModuleTypeName,
    string ResultTypeName,
    IReadOnlySet<string> RequiredCapabilities,
    string? MatrixTarget,
    DateTimeOffset AssignedAt,
    ModuleAssignmentConfig Configuration);
