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
        Func<ModuleAssignment, DateTimeOffset, CancellationToken, Task> executeAsync,
        int maxConcurrency,
        Action<Exception> onError,
        CancellationToken cancellationToken,
        TimeProvider? timeProvider = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxConcurrency, 1);
        var clock = timeProvider ?? TimeProvider.System;

        using var concurrencyGate = new SemaphoreSlim(maxConcurrency, maxConcurrency);
        var running = new List<Task>();
        var pendingDequeue = DequeueAsync(dequeueAsync, onError, clock, cancellationToken);
        while (true)
        {
            var (assignment, claimedAt) = await pendingDequeue.ConfigureAwait(false);
            if (assignment is null)
            {
                break;
            }

            // The coordinator has already transferred ownership. Drain this assignment even
            // after cancellation so its execution path can publish a terminal result.
            await concurrencyGate.WaitAsync(CancellationToken.None).ConfigureAwait(false);

            running.RemoveAll(static task => task.IsCompletedSuccessfully);
            running.Add(ExecuteAndReleaseAsync(
                assignment,
                claimedAt,
                executeAsync,
                onError,
                concurrencyGate,
                cancellationToken));
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            pendingDequeue = DequeueAsync(dequeueAsync, onError, clock, cancellationToken);
        }

        await Task.WhenAll(running).ConfigureAwait(false);
    }

    private static async Task<(ModuleAssignment? Assignment, DateTimeOffset ClaimedAt)> DequeueAsync(
        Func<CancellationToken, Task<ModuleAssignment?>> dequeueAsync,
        Action<Exception> onError,
        TimeProvider clock,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var assignment = await dequeueAsync(cancellationToken).ConfigureAwait(false);
                return (assignment, clock.GetUtcNow());
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return (null, default);
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
                    return (null, default);
                }
            }
        }

        return (null, default);
    }

    private static async Task ExecuteAsync(
        ModuleAssignment assignment,
        DateTimeOffset claimedAt,
        Func<ModuleAssignment, DateTimeOffset, CancellationToken, Task> executeAsync,
        Action<Exception> onError,
        CancellationToken cancellationToken)
    {
        try
        {
            await executeAsync(assignment, claimedAt, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
            when (cancellationToken.IsCancellationRequested ||
                  exception.CancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            onError(exception);
        }
    }

    private static async Task ExecuteAndReleaseAsync(
        ModuleAssignment assignment,
        DateTimeOffset claimedAt,
        Func<ModuleAssignment, DateTimeOffset, CancellationToken, Task> executeAsync,
        Action<Exception> onError,
        SemaphoreSlim concurrencyGate,
        CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteAsync(assignment, claimedAt, executeAsync, onError, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            concurrencyGate.Release();
        }
    }
}
