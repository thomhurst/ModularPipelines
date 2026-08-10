namespace ModularPipelines.Engine.Execution;

internal static class WorkerCancellationClassifier
{
    public static bool IsExpected(
        Exception exception,
        CancellationToken workerCancellationToken)
    {
        return exception is OperationCanceledException operationCanceledException
               && operationCanceledException.CancellationToken == workerCancellationToken
               && (workerCancellationToken.IsCancellationRequested
                   || exception is NormalizedWorkerCancellationException);
    }
}
