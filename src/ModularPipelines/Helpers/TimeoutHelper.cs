namespace ModularPipelines.Helpers;

/// <summary>
/// Reusable utility for executing tasks with timeout support that can return
/// control immediately when timeout elapses.
/// </summary>
/// <remarks>
/// This implementation is inspired by the TUnit testing framework's TimeoutHelper.
/// </remarks>
internal static class TimeoutHelper
{
    /// <summary>
    /// Grace period to allow tasks to handle cancellation before throwing
    /// timeout exception.
    /// </summary>
    private static readonly TimeSpan GracePeriod = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Executes a task with an optional timeout. If the timeout elapses before
    /// the task completes, control is returned to the caller immediately with a
    /// TimeoutException.
    /// </summary>
    public static async Task ExecuteWithTimeoutAsync(
        Func<CancellationToken, Task> taskFactory,
        TimeSpan? timeout,
        CancellationToken cancellationToken,
        string? timeoutMessage = null)
    {
        await ExecuteWithTimeoutAsync(
            async ct =>
            {
                await taskFactory(ct).ConfigureAwait(false);
                return true;
            },
            timeout,
            cancellationToken,
            timeoutMessage).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a task with an optional timeout and returns a result. If the
    /// timeout elapses before the task completes, control is returned to the
    /// caller immediately with a TimeoutException.
    /// </summary>
    public static async Task<T> ExecuteWithTimeoutAsync<T>(
        Func<CancellationToken, Task<T>> taskFactory,
        TimeSpan? timeout,
        CancellationToken cancellationToken,
        string? timeoutMessage = null)
    {
        // Fast path: no timeout specified
        if (!timeout.HasValue || timeout.Value == TimeSpan.Zero)
        {
            var task = taskFactory(cancellationToken);

            // If the token can't be cancelled, just await directly (avoid allocations)
            if (!cancellationToken.CanBeCanceled)
            {
                return await task.ConfigureAwait(false);
            }

            // Race against cancellation - TrySetCanceled makes the TCS throw
            // OperationCanceledException when awaited
            var tcs = new TaskCompletionSource<T>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            using var reg = cancellationToken.Register(
                static state => ((TaskCompletionSource<T>)state!).TrySetCanceled(),
                tcs);

            // await await: first gets winning task, then awaits it
            // (propagates result or exception)
            return await await Task.WhenAny(task, tcs.Task)
                .ConfigureAwait(false);
        }

        // Timeout path: create linked token so task can observe both timeout
        // and external cancellation.
        using var timeoutCts = CancellationTokenSource
            .CreateLinkedTokenSource(cancellationToken);

        // Set up cancellation detection BEFORE scheduling timeout to avoid race
        // condition where timeout fires before registration completes
        // (with very small timeouts)
        var cancelledTcs = new TaskCompletionSource<T>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var registration = timeoutCts.Token.Register(
            static state => ((TaskCompletionSource<T>)state!)
                .TrySetCanceled(),
            cancelledTcs);

        // Now schedule the timeout - registration is guaranteed to catch it
        timeoutCts.CancelAfter(timeout.Value);

        var executionTask = taskFactory(timeoutCts.Token);

        var winner = await Task.WhenAny(executionTask, cancelledTcs.Task)
            .ConfigureAwait(false);

        if (winner == cancelledTcs.Task)
        {
            // Determine if it was external cancellation or timeout
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            // Timeout occurred - give the execution task a brief grace period
            // to clean up
            try
            {
                await executionTask.WaitAsync(GracePeriod, CancellationToken.None).ConfigureAwait(false);
            }
            catch
            {
                // Ignore all exceptions - task was cancelled, we're just giving
                // it time to clean up
            }

            // Even if task completed during grace period, timeout already elapsed
            // so we throw
            throw new TimeoutException(
                timeoutMessage ?? $"Operation timed out after {timeout.Value}");
        }

        return await executionTask.ConfigureAwait(false);
    }
}
