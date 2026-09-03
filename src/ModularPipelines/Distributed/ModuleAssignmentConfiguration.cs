namespace ModularPipelines.Distributed;

public record ModuleAssignmentConfiguration(
    double? TimeoutSeconds,
    bool AlwaysRun);
