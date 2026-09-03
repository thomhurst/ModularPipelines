using ModularPipelines.Helpers;

namespace ModularPipelines.Distributed.Worker;

internal static class DistributedWorkerPool
{
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

        var running = new HashSet<Task>();
        var pendingDequeue = DequeueAsync(dequeueAsync, onError, cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            var assignment = await pendingDequeue.ConfigureAwait(false);
            if (assignment is null)
            {
                break;
            }

            running.RemoveWhere(task => task.IsCompleted);
            if (running.Count >= maxConcurrency)
            {
                var completed = await Task.WhenAny(running).ConfigureAwait(false);
                running.Remove(completed);
            }

            running.Add(ExecuteAsync(assignment, executeAsync, onError, cancellationToken));
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
}
