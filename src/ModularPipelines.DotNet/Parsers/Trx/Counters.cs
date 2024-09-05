namespace ModularPipelines.DotNet.Parsers.Trx;

public record Counters(
    int Total,
    int Executed,
    int Passed,
    int Failed,
    int Error,
    int Timeout,
    int Aborted,
    int Inconclusive,
    int PassedButRunAborted,
    int NotRunnable,
    int NotExecuted,
    int Disconnected,
    int Warning,
    int Completed,
    int InProgress,
    int Pending);