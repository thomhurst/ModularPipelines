namespace ModularPipelines.Helpers;

/// <summary>
/// Represents the result of executing a task with timeout enforcement.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
internal readonly struct TimeoutExecutionResult<T>
{
    private TimeoutExecutionResult(
        T? value,
        bool timedOut,
        bool wasCancellationTokenRespected,
        TimeSpan elapsedTime)
    {
        Value = value;
        TimedOut = timedOut;
        WasCancellationTokenRespected = wasCancellationTokenRespected;
        ElapsedTime = elapsedTime;
    }

    /// <summary>
    /// Gets the result value if the operation completed successfully.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets a value indicating whether the operation timed out.
    /// </summary>
    public bool TimedOut { get; }

    /// <summary>
    /// Gets a value indicating whether the cancellation token was properly observed.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, the operation responded to the cancellation token and exited gracefully.
    /// When <c>false</c>, the operation ignored the cancellation token and had to be forcibly
    /// abandoned by the timeout enforcement mechanism.
    /// </remarks>
    public bool WasCancellationTokenRespected { get; }

    /// <summary>
    /// Gets the actual time elapsed during execution.
    /// </summary>
    public TimeSpan ElapsedTime { get; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static TimeoutExecutionResult<T> Success(T? value, TimeSpan elapsedTime)
        => new(value, timedOut: false, wasCancellationTokenRespected: true, elapsedTime);

    /// <summary>
    /// Creates a timeout result where the cancellation token was respected.
    /// </summary>
    public static TimeoutExecutionResult<T> TimeoutWithTokenRespected(TimeSpan elapsedTime)
        => new(default, timedOut: true, wasCancellationTokenRespected: true, elapsedTime);

    /// <summary>
    /// Creates a timeout result where the cancellation token was ignored.
    /// </summary>
    public static TimeoutExecutionResult<T> TimeoutWithTokenIgnored(TimeSpan elapsedTime)
        => new(default, timedOut: true, wasCancellationTokenRespected: false, elapsedTime);
}
