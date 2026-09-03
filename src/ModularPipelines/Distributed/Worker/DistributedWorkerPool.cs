using ModularPipelines.Helpers;

namespace ModularPipelines.Distributed.Worker;

internal static class DistributedWorkerPool
{
    private static readonly TimeSpan DequeueRetryDelay = TimeSpan.FromMilliseconds(100);

    public static int GetMaxConcurrency(
        IParallelLimitProvider parallelLimitProvider,
        DistributedOptions options)
    {
        var pipelineLimit = parallelLimitProvider.GetMaxDegreeOfParallelism();
        var nodeLimit = options.MaxParallelism ?? pipelineLimit;
        if (nodeLimit < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                nodeLimit,
                "Distributed MaxParallelism must be at least 1.");
        }

        return Math.Min(pipelineLimit, nodeLimit);
    }

    public static async Task RunAsync(
        Func<CancellationToken, Task<ModuleAssignment?>> dequeueAsync,
        Func<ModuleAssignment, CancellationToken, Task> executeAsync,
        int maxConcurrency,
        Action<Exception> onError,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxConcurrency, 1);

        using var concurrencyGate = new SemaphoreSlim(maxConcurrency, maxConcurrency);
        var running = new List<Task>();
        var pendingDequeue = DequeueAsync(dequeueAsync, onError, cancellationToken);
        while (true)
        {
            var assignment = await pendingDequeue.ConfigureAwait(false);
            if (assignment is null)
            {
                break;
            }

            await concurrencyGate.WaitAsync(CancellationToken.None).ConfigureAwait(false);
            running.Add(ExecuteAndReleaseAsync(
                assignment,
                executeAsync,
                onError,
                concurrencyGate,
                cancellationToken));
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            pendingDequeue = DequeueAsync(dequeueAsync, onError, cancellationToken);
        }

        await Task.WhenAll(running).ConfigureAwait(false);
    }

    private static async Task<ModuleAssignment?> DequeueAsync(
        Func<CancellationToken, Task<ModuleAssignment?>> dequeueAsync,
        Action<Exception> onError,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                return await dequeueAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            catch (Exception exception)
            {
                onError(exception);
                try
                {
                    await Task.Delay(DequeueRetryDelay, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }
            }
        }

        return null;
    }

    private static async Task ExecuteAsync(
        ModuleAssignment assignment,
        Func<ModuleAssignment, CancellationToken, Task> executeAsync,
        Action<Exception> onError,
        CancellationToken cancellationToken)
    {
        try
        {
            await executeAsync(assignment, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            onError(exception);
        }
    }

    private static async Task ExecuteAndReleaseAsync(
        ModuleAssignment assignment,
        Func<ModuleAssignment, CancellationToken, Task> executeAsync,
        Action<Exception> onError,
        SemaphoreSlim concurrencyGate,
        CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteAsync(assignment, executeAsync, onError, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            concurrencyGate.Release();
        }
    }
}
