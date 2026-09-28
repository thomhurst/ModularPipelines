using System.Runtime.ExceptionServices;

namespace ModularPipelines.Distributed;

/// <summary>
/// Represents an exception thrown by a module on a remote distributed worker when the
/// original exception type cannot be reconstructed safely in the current process.
/// </summary>
public sealed class RemoteModuleException : Exception
{
    /// <summary>
    /// Initialises a new instance of the <see cref="RemoteModuleException"/> class.
    /// </summary>
    /// <param name="originalExceptionType">The fully qualified type name of the original exception.</param>
    /// <param name="originalMessage">The original exception message.</param>
    /// <param name="remoteStackTrace">The stack trace captured on the remote worker.</param>
    /// <param name="workerId">The distributed worker that threw the exception, when known.</param>
    /// <param name="innerException">The reconstructed inner exception, when the original had one.</param>
    public RemoteModuleException(
        string originalExceptionType,
        string originalMessage,
        string? remoteStackTrace,
        WorkerId? workerId = null,
        Exception? innerException = null)
        : base(originalMessage, innerException)
    {
        OriginalExceptionType = originalExceptionType;
        OriginalMessage = originalMessage;
        RemoteStackTrace = remoteStackTrace;
        WorkerId = workerId;
        if (!string.IsNullOrEmpty(remoteStackTrace))
        {
            ExceptionDispatchInfo.SetRemoteStackTrace(this, remoteStackTrace);
        }
    }

    /// <summary>
    /// Gets the fully qualified type name of the exception thrown by the worker.
    /// </summary>
    public string OriginalExceptionType { get; }

    /// <summary>
    /// Gets the original exception message returned by the worker.
    /// </summary>
    public string OriginalMessage { get; }

    /// <summary>
    /// Gets the stack trace captured on the worker.
    /// </summary>
    public string? RemoteStackTrace { get; }

    /// <summary>
    /// Gets the distributed worker that returned the failure, when known.
    /// </summary>
    public WorkerId? WorkerId { get; private set; }

    /// <inheritdoc />
    public override string Message => WorkerId is { } workerId
        ? $"Remote worker {workerId} threw {OriginalExceptionType}: {OriginalMessage}"
        : $"Remote execution threw {OriginalExceptionType}: {OriginalMessage}";

    internal void AttachWorkerId(WorkerId workerId)
    {
        WorkerId ??= workerId;
        if (InnerException is RemoteModuleException inner)
        {
            inner.AttachWorkerId(workerId);
        }
    }
}
