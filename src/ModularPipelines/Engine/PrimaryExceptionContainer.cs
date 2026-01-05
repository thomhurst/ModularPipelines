using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores the primary/first exception that caused the pipeline to fail.
/// This is the exception that should be rethrown when the pipeline execution completes.
/// Secondary exceptions (from AlwaysRun modules, etc.) are stored in <see cref="ISecondaryExceptionContainer"/>.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// Uses lock-based synchronization to ensure only the first exception is stored,
/// with proper memory barriers for reads after the lock is released.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class PrimaryExceptionContainer : IPrimaryExceptionContainer
{
    private readonly object _exceptionLock = new();

    /// <inheritdoc />
    /// <remarks>
    /// This property is safe to read concurrently. The lock in <see cref="SetException"/>
    /// provides the necessary memory barrier for visibility.
    /// </remarks>
    public Exception? OriginalException { get; private set; }

    /// <inheritdoc />
    /// <remarks>
    /// This property is safe to read concurrently. The lock in <see cref="SetException"/>
    /// provides the necessary memory barrier for visibility.
    /// </remarks>
    public ExceptionDispatchInfo? OriginalExceptionDispatchInfo { get; private set; }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Only the first exception set will be stored;
    /// subsequent calls are ignored. Uses lock-based synchronization to ensure
    /// atomicity of the check-then-set operation.
    /// </remarks>
    public void SetException(Exception exception)
    {
        lock (_exceptionLock)
        {
            // Only store the first exception
            if (OriginalException == null)
            {
                OriginalException = exception;
                OriginalExceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
            }
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is thread-safe. Reads the exception dispatch info set by <see cref="SetException"/>.
    /// If an exception was set, this method will rethrow it preserving the original stack trace.
    /// </remarks>
    public void ThrowIfHasException()
    {
        OriginalExceptionDispatchInfo?.Throw();
    }
}
