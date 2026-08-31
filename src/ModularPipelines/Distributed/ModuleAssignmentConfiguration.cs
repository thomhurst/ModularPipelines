namespace ModularPipelines.Distributed;

public record ModuleAssignmentConfiguration(
    double? TimeoutSeconds,
    int RetryCount,
    bool AlwaysRun);
