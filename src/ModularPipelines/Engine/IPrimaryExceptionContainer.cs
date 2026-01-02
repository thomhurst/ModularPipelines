using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores the primary/first exception that caused the pipeline to fail.
/// This is the exception that should be rethrown when the pipeline execution completes.
/// Secondary exceptions (from AlwaysRun modules, etc.) are stored in <see cref="ISecondaryExceptionContainer"/>.
/// </summary>
internal interface IPrimaryExceptionContainer
{
    /// <summary>
    /// Gets the original exception that caused the pipeline to fail.
    /// </summary>
    Exception? OriginalException { get; }

    /// <summary>
    /// Gets the ExceptionDispatchInfo that preserves the original stack trace.
    /// </summary>
    ExceptionDispatchInfo? OriginalExceptionDispatchInfo { get; }

    /// <summary>
    /// Sets the primary exception. Only the first exception is stored; subsequent calls are ignored.
    /// </summary>
    /// <param name="exception">The exception to store.</param>
    void SetException(Exception exception);

    /// <summary>
    /// Throws the stored exception if one exists, preserving the original stack trace.
    /// </summary>
    void ThrowIfHasException();
}
