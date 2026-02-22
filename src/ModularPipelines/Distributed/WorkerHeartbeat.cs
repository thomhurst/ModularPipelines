namespace ModularPipelines.Distributed;

public record WorkerHeartbeat(
    int WorkerIndex,
    DateTimeOffset Timestamp,
    string? CurrentModule);
