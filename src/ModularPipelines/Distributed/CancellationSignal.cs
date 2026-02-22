namespace ModularPipelines.Distributed;

public record CancellationSignal(
    string Reason,
    DateTimeOffset Timestamp);
