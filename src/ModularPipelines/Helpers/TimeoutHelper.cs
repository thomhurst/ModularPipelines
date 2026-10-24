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
            cancellationToken).ConfigureAwait(false);

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
    /// <param name="cancellationGracePeriod">Maximum wait for task completion after external cancellation.</param>
    /// <param name="timeProvider">Clock used to schedule the attempt deadline and cancellation grace periods. Defaults to the system clock.</param>
    /// <returns>A result containing execution details including timeout status and token cooperation.</returns>
    public static async Task<TimeoutExecutionResult<T>> ExecuteWithTimeoutAndDetailsAsync<T>(
        Func<CancellationToken, Task<T>> taskFactory,
        TimeSpan? timeout,
        CancellationToken cancellationToken,
        TimeSpan cancellationGracePeriod = default,
        TimeProvider? timeProvider = null)
    {
        timeProvider ??= TimeProvider.System;
        var stopwatch = Stopwatch.StartNew();

        // Fast path: no timeout specified
        if (!timeout.HasValue || timeout.Value == TimeSpan.Zero)
        {
            return await ExecuteWithoutTimeoutAsync(taskFactory, stopwatch, cancellationGracePeriod, cancellationToken, timeProvider)
                .ConfigureAwait(false);
        }

        // Keep the attempt token separate so signal ordering is recorded before
        // cancellation is propagated to the executing task.
        using var deadlineCts = new CancellationTokenSource(timeout.Value, timeProvider);
        using var attemptCts = new CancellationTokenSource();

        var cancellationSignals = new CancellationSignals<T>(attemptCts);
        var deadlineState = cancellationSignals.Deadline;
        var externalCancellationState = cancellationSignals.ExternalCancellation;
        using var deadlineRegistration = deadlineCts.Token.Register(
            static state => ((CancellationSignalState<T>) state!).SignalCancellation(),
            deadlineState);
        using var externalCancellationRegistration = cancellationToken.Register(
            static state => ((CancellationSignalState<T>) state!).SignalCancellation(),
            externalCancellationState);

        // The timeout is armed before starting the factory. If cancellation occurs while
        // the factory is returning its task, signal resolution waits for publication
        // of that real task instead of making a decision from a provisional wrapper.
        var executionTask = taskFactory(attemptCts.Token);
        cancellationSignals.PublishExecutionTask(executionTask);

        await Task.WhenAny(
                executionTask,
                deadlineState.Signal.Task,
                externalCancellationState.Signal.Task)
            .ConfigureAwait(false);

        if (externalCancellationState.Signal.Task.IsCompletedSuccessfully
            && !await externalCancellationState.Signal.Task.ConfigureAwait(false))
        {
            await DidTaskRespondDuringGracePeriodAsync(executionTask, cancellationGracePeriod, CancellationToken.None, timeProvider).ConfigureAwait(false);
            throw new OperationCanceledException(cancellationToken);
        }

        if (deadlineState.Signal.Task.IsCompletedSuccessfully
            && !await deadlineState.Signal.Task.ConfigureAwait(false))
        {
            return await CreateTimeoutResultAsync(executionTask, stopwatch, cancellationGracePeriod, cancellationToken, timeProvider)
                .ConfigureAwait(false);
        }

        var value = await executionTask.ConfigureAwait(false);
        return TimeoutExecutionResult<T>.Success(value, stopwatch.Elapsed);
    }

    private static async Task<TimeoutExecutionResult<T>> ExecuteWithoutTimeoutAsync<T>(
        Func<CancellationToken, Task<T>> taskFactory,
        Stopwatch stopwatch,
        TimeSpan cancellationGracePeriod,
        CancellationToken cancellationToken,
        TimeProvider timeProvider)
    {
        var task = taskFactory(cancellationToken);

        if (!cancellationToken.CanBeCanceled)
        {
            var result = await task.ConfigureAwait(false);
            return TimeoutExecutionResult<T>.Success(result, stopwatch.Elapsed);
        }

        var cancellationTaskSource = new TaskCompletionSource<T>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var registration = cancellationToken.Register(
            static state => ((TaskCompletionSource<T>) state!).TrySetCanceled(),
            cancellationTaskSource);

        var winner = await Task.WhenAny(task, cancellationTaskSource.Task).ConfigureAwait(false);
        if (winner != task)
        {
            await DidTaskRespondDuringGracePeriodAsync(task, cancellationGracePeriod, CancellationToken.None, timeProvider).ConfigureAwait(false);
            throw new OperationCanceledException(cancellationToken);
        }

        var resultValue = await winner.ConfigureAwait(false);
        return TimeoutExecutionResult<T>.Success(resultValue, stopwatch.Elapsed);
    }

    private static async Task<TimeoutExecutionResult<T>> CreateTimeoutResultAsync<T>(
        Task<T> executionTask,
        Stopwatch stopwatch,
        TimeSpan cancellationGracePeriod,
        CancellationToken cancellationToken,
        TimeProvider timeProvider)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var taskRespondedDuringGrace = await DidTaskRespondDuringGracePeriodAsync(
                    executionTask, GracePeriod, cancellationToken, timeProvider)
                .ConfigureAwait(false);
            var elapsedTime = stopwatch.Elapsed;

            return taskRespondedDuringGrace
                ? TimeoutExecutionResult<T>.TimeoutWithTokenRespected(elapsedTime)
                : TimeoutExecutionResult<T>.TimeoutWithTokenIgnored(elapsedTime);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Pipeline cancellation during the deadline grace period gets its own cleanup budget.
            await DidTaskRespondDuringGracePeriodAsync(executionTask, cancellationGracePeriod, CancellationToken.None, timeProvider).ConfigureAwait(false);
            throw new OperationCanceledException(cancellationToken);
        }
    }

    private static async Task<bool> DidTaskRespondDuringGracePeriodAsync(
        Task executionTask,
        TimeSpan gracePeriod,
        CancellationToken cancellationToken,
        TimeProvider timeProvider)
    {
        try
        {
            await executionTask.WaitAsync(gracePeriod, timeProvider, cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (TimeoutException)
        {
            var taskRespondedDuringGrace = executionTask.IsCompleted;
            // Completion can race grace expiry, so observe faults even if the task just finished.
            TaskObservation.ObserveFault(executionTask);

            return taskRespondedDuringGrace;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            return true;
        }
        catch (Exception)
        {
            return true;
        }
    }

    internal sealed class CancellationSignals<T>(CancellationTokenSource attemptCts)
    {
        public CancellationSignalState<T> Deadline { get; } = new CancellationSignalState<T>(
                attemptCts,
                completedTaskWinsAfterSignal: false);

        public CancellationSignalState<T> ExternalCancellation { get; } = new CancellationSignalState<T>(
                attemptCts,
                completedTaskWinsAfterSignal: true);

        public void PublishExecutionTask(Task<T> executionTask)
        {
            Deadline.PublishExecutionTask(executionTask);
            ExternalCancellation.PublishExecutionTask(executionTask);
        }
    }

    internal sealed class CancellationSignalState<T>(
        CancellationTokenSource attemptCts,
        bool completedTaskWinsAfterSignal)
    {
        private readonly Lock _lock = new();
        private bool _cancellationSignaled;
        private Task<T>? _executionTask;

        public TaskCompletionSource<bool> Signal { get; } = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        internal void PublishExecutionTask(Task<T> executionTask)
        {
            bool? taskOwnsOutcome = null;
            lock (_lock)
            {
                _executionTask = executionTask;
                if (_cancellationSignaled)
                {
                    taskOwnsOutcome = TaskOwnsOutcome(
                        executionTask,
                        cancellationWasSignaledBeforePublication: true);
                }
            }

            if (taskOwnsOutcome.HasValue)
            {
                Signal.TrySetResult(taskOwnsOutcome.Value);
            }
        }

        public void SignalCancellation()
        {
            Task<T>? executionTask;
            lock (_lock)
            {
                _cancellationSignaled = true;
                executionTask = _executionTask;
            }

            if (executionTask is not null)
            {
                // Record ordering before propagating cancellation to the attempt.
                Signal.TrySetResult(TaskOwnsOutcome(
                    executionTask,
                    cancellationWasSignaledBeforePublication: false));
            }

            attemptCts.Cancel();
        }

        private bool TaskOwnsOutcome(
            Task executionTask,
            bool cancellationWasSignaledBeforePublication)
        {
            if (!cancellationWasSignaledBeforePublication)
            {
                return executionTask.IsCompleted;
            }

            // A signal that fired before publication may itself have caused the task
            // to cancel or fault. Only an independently successful synchronous result
            // can retain ownership, and only for signals that permit that behavior.
            return completedTaskWinsAfterSignal && executionTask.IsCompletedSuccessfully;
        }
    }
}
