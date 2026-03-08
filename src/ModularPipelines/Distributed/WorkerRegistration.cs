namespace ModularPipelines.Distributed;

public record WorkerRegistration(
    int WorkerIndex,
    HashSet<string> Capabilities,
    DateTimeOffset RegisteredAt);
