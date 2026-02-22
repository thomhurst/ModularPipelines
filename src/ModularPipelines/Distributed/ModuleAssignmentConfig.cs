namespace ModularPipelines.Distributed;

public record ModuleAssignmentConfig(
    double? TimeoutSeconds,
    int RetryCount,
    bool AlwaysRun);
