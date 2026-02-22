namespace ModularPipelines.Distributed;

public record WorkerRegistration(
    int WorkerIndex,
    IReadOnlySet<string> Capabilities,
    DateTimeOffset RegisteredAt,
    WorkerStatus Status,
    string? CurrentModule);
