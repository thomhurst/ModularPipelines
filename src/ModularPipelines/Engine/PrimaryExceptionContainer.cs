using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores the primary/first exception that caused the pipeline to fail.
/// This is the exception that should be rethrown when the pipeline execution completes.
/// Secondary exceptions (from AlwaysRun modules, etc.) are stored in <see cref="ISecondaryExceptionContainer"/>.
/// </summary>
internal class PrimaryExceptionContainer : IPrimaryExceptionContainer
{
    private readonly object _exceptionLock = new();

    /// <inheritdoc />
    public Exception? OriginalException { get; private set; }

    /// <inheritdoc />
    public ExceptionDispatchInfo? OriginalExceptionDispatchInfo { get; private set; }

    /// <inheritdoc />
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
    public void ThrowIfHasException()
    {
        OriginalExceptionDispatchInfo?.Throw();
    }
}
