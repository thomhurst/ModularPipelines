using System.Diagnostics;

namespace ModularPipelines.Helpers;

/// <summary>
/// Reusable utility for executing tasks with timeout support that can return
/// control immediately when timeout elapses.
/// </summary>
/// <remarks>
/// This implementation is inspired by the TUnit testing framework's TimeoutHelper.
/// It uses Task.WhenAny to detect timeout even when operations ignore the cancellation token,
/// ensuring timeout enforcement regardless of operation cooperation.
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
        var result = await ExecuteWithTimeoutAndDetailsAsync(
            taskFactory,
            timeout,
            cancellationToken,
            timeoutMessage).ConfigureAwait(false);

        if (result.TimedOut)
        {
            throw new TimeoutException(
                timeoutMessage ?? $"Operation timed out after {timeout}");
        }

        return result.Value!;
    }

    /// <summary>
    /// Executes a task with an optional timeout and returns detailed execution information.
    /// Unlike <see cref="ExecuteWithTimeoutAsync{T}"/>, this method returns a result object
    /// instead of throwing an exception on timeout, providing information about whether
    /// the cancellation token was respected.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="taskFactory">Factory function that creates the task to execute.</param>
    /// <param name="timeout">The timeout duration, or null/zero to disable timeout.</param>
    /// <param name="cancellationToken">Cancellation token for external cancellation.</param>
    /// <param name="timeoutMessage">Optional message for timeout exceptions.</param>
    /// <returns>A result containing execution details including timeout status and token cooperation.</returns>
    public static async Task<TimeoutExecutionResult<T>> ExecuteWithTimeoutAndDetailsAsync<T>(
        Func<CancellationToken, Task<T>> taskFactory,
        TimeSpan? timeout,
        CancellationToken cancellationToken,
        string? timeoutMessage = null)
    {
        var stopwatch = Stopwatch.StartNew();

        // Fast path: no timeout specified
        if (!timeout.HasValue || timeout.Value == TimeSpan.Zero)
        {
            var task = taskFactory(cancellationToken);

            // If the token can't be cancelled, just await directly (avoid allocations)
            if (!cancellationToken.CanBeCanceled)
            {
                var result = await task.ConfigureAwait(false);
                return TimeoutExecutionResult<T>.Success(result, stopwatch.Elapsed);
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
            var winningResult = await await Task.WhenAny(task, tcs.Task)
                .ConfigureAwait(false);
            return TimeoutExecutionResult<T>.Success(winningResult, stopwatch.Elapsed);
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

            // Timeout occurred - the task did NOT respond to the cancellation token
            // in time (otherwise executionTask would have completed first with a
            // TaskCanceledException). Give it a brief grace period to clean up.
            var taskRespondedDuringGrace = false;
            try
            {
                await executionTask.WaitAsync(GracePeriod, CancellationToken.None).ConfigureAwait(false);

                // Task completed during grace period - it did eventually respond
                taskRespondedDuringGrace = true;
            }
            catch (TimeoutException)
            {
                // Task still didn't complete - definitely not respecting the token
                taskRespondedDuringGrace = false;
            }
            catch
            {
                // Task threw an exception (could be TaskCanceledException from
                // finally observing the token) - consider it responsive
                taskRespondedDuringGrace = true;
            }

            var elapsedTime = stopwatch.Elapsed;

            // If the task completed exactly when timeout fired (race condition),
            // we still consider it a timeout since the deadline was reached
            return taskRespondedDuringGrace
                ? TimeoutExecutionResult<T>.TimeoutWithTokenRespected(elapsedTime)
                : TimeoutExecutionResult<T>.TimeoutWithTokenIgnored(elapsedTime);
        }

        // Task completed before timeout
        try
        {
            var value = await executionTask.ConfigureAwait(false);
            return TimeoutExecutionResult<T>.Success(value, stopwatch.Elapsed);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
        {
            // The task threw OperationCanceledException/TaskCanceledException in response to
            // our timeout cancellation - this means it DID respect the token.
            // This can happen when executionTask and cancelledTcs.Task complete at nearly
            // the same time, and executionTask wins the race.
            return TimeoutExecutionResult<T>.TimeoutWithTokenRespected(stopwatch.Elapsed);
        }
    }
}
