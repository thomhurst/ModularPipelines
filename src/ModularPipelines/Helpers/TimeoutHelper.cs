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
            return await ExecuteWithoutTimeoutAsync(taskFactory, cancellationToken, stopwatch)
                .ConfigureAwait(false);
        }

        // Keep the attempt token separate so signal ordering is recorded before
        // cancellation is propagated to the executing task.
        using var deadlineCts = new CancellationTokenSource();
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

        // Arm the timeout before starting the factory. If cancellation occurs while
        // the factory is returning its task, signal resolution waits for publication
        // of that real task instead of making a decision from a provisional wrapper.
        deadlineCts.CancelAfter(timeout.Value);

        Task<T> executionTask;
        try
        {
            executionTask = taskFactory(attemptCts.Token);
        }
        catch (OperationCanceledException exception)
        {
            var cancellationTaskSource = new TaskCompletionSource<T>();
            cancellationTaskSource.SetCanceled(exception.CancellationToken);
            executionTask = cancellationTaskSource.Task;
        }
        catch (Exception exception)
        {
            executionTask = Task.FromException<T>(exception);
        }

        cancellationSignals.PublishExecutionTask(executionTask);

        await Task.WhenAny(
                executionTask,
                deadlineState.Signal.Task,
                externalCancellationState.Signal.Task)
            .ConfigureAwait(false);

        if (externalCancellationState.Signal.Task.IsCompletedSuccessfully
            && !await externalCancellationState.Signal.Task.ConfigureAwait(false))
        {
            TaskObservation.ObserveFault(executionTask);
            throw new OperationCanceledException(cancellationToken);
        }

        if (deadlineState.Signal.Task.IsCompletedSuccessfully
            && !await deadlineState.Signal.Task.ConfigureAwait(false))
        {
            return await CreateTimeoutResultAsync(executionTask, cancellationToken, stopwatch)
                .ConfigureAwait(false);
        }

        var value = await executionTask.ConfigureAwait(false);
        return TimeoutExecutionResult<T>.Success(value, stopwatch.Elapsed);
    }

    private static async Task<TimeoutExecutionResult<T>> ExecuteWithoutTimeoutAsync<T>(
        Func<CancellationToken, Task<T>> taskFactory,
        CancellationToken cancellationToken,
        Stopwatch stopwatch)
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
            TaskObservation.ObserveFault(task);
        }

        var resultValue = await winner.ConfigureAwait(false);
        return TimeoutExecutionResult<T>.Success(resultValue, stopwatch.Elapsed);
    }

    private static async Task<TimeoutExecutionResult<T>> CreateTimeoutResultAsync<T>(
        Task<T> executionTask,
        CancellationToken cancellationToken,
        Stopwatch stopwatch)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            TaskObservation.ObserveFault(executionTask);
            throw new OperationCanceledException(cancellationToken);
        }

        var taskRespondedDuringGrace = await DidTaskRespondDuringGracePeriodAsync(executionTask)
            .ConfigureAwait(false);
        var elapsedTime = stopwatch.Elapsed;

        return taskRespondedDuringGrace
            ? TimeoutExecutionResult<T>.TimeoutWithTokenRespected(elapsedTime)
            : TimeoutExecutionResult<T>.TimeoutWithTokenIgnored(elapsedTime);
    }

    private static async Task<bool> DidTaskRespondDuringGracePeriodAsync(Task executionTask)
    {
        try
        {
            await executionTask.WaitAsync(GracePeriod, CancellationToken.None).ConfigureAwait(false);
            return true;
        }
        catch (TimeoutException)
        {
            var taskRespondedDuringGrace = executionTask.IsCompleted;
            if (!taskRespondedDuringGrace)
            {
                TaskObservation.ObserveFault(executionTask);
            }

            return taskRespondedDuringGrace;
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

    internal sealed class CancellationSignals<T>
    {
        private Task<T>? _executionTask;

        public CancellationSignals(CancellationTokenSource attemptCts)
        {
            Deadline = new CancellationSignalState<T>(attemptCts, this);
            ExternalCancellation = new CancellationSignalState<T>(attemptCts, this);
        }

        public CancellationSignalState<T> Deadline { get; }

        public CancellationSignalState<T> ExternalCancellation { get; }

        internal Task<T>? ExecutionTask => Volatile.Read(ref _executionTask);

        public void PublishExecutionTask(Task<T> executionTask)
        {
            // Both signals read one atomic task reference, so neither can observe
            // a provisional wrapper after the actual factory task is available.
            Volatile.Write(ref _executionTask, executionTask);
            Deadline.ExecutionTaskPublished();
            ExternalCancellation.ExecutionTaskPublished();
        }
    }

    internal sealed class CancellationSignalState<T>(
        CancellationTokenSource attemptCts,
        CancellationSignals<T> cancellationSignals)
    {
        private readonly Lock _lock = new();
        private bool _cancellationSignaled;

        public TaskCompletionSource<bool> Signal { get; } = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        internal void ExecutionTaskPublished()
        {
            bool cancellationSignaled;
            lock (_lock)
            {
                cancellationSignaled = _cancellationSignaled;
            }

            if (cancellationSignaled)
            {
                var executionTask = cancellationSignals.ExecutionTask!;
                // A completed value or fault existed by the time publication caught up
                // with the signal. A cancelled task still belongs to the signal that
                // cancelled the attempt token.
                Signal.TrySetResult(executionTask.IsCompleted && !executionTask.IsCanceled);
            }
        }

        public void SignalCancellation()
        {
            Task<T>? executionTask;
            lock (_lock)
            {
                _cancellationSignaled = true;
                executionTask = cancellationSignals.ExecutionTask;
            }

            if (executionTask is not null)
            {
                // Record ordering before propagating cancellation to the attempt.
                Signal.TrySetResult(executionTask.IsCompleted);
            }

            attemptCts.Cancel();
        }
    }
}
