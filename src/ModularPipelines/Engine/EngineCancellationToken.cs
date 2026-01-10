using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages pipeline cancellation state and token.
/// Uses composition instead of inheritance from CancellationTokenSource.
/// Exception storage is delegated to <see cref="IPrimaryExceptionContainer"/>.
/// </summary>
[ExcludeFromCodeCoverage]
internal class EngineCancellationToken : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly IPrimaryExceptionContainer _primaryExceptionContainer;

    public string? Reason { get; private set; }

    /// <summary>
    /// Gets the cancellation token from the underlying CancellationTokenSource.
    /// </summary>
    public CancellationToken Token => _cts.Token;

    /// <summary>
    /// Gets whether cancellation has been requested on the underlying token source.
    /// </summary>
    public bool IsCancellationRequested => _cts.IsCancellationRequested;

    /// <summary>
    /// Gets the original exception that caused the pipeline to fail.
    /// Delegated to <see cref="IPrimaryExceptionContainer"/>.
    /// </summary>
    public Exception? OriginalException => _primaryExceptionContainer.OriginalException;

    /// <summary>
    /// Gets the ExceptionDispatchInfo that preserves the original stack trace.
    /// Delegated to <see cref="IPrimaryExceptionContainer"/>.
    /// </summary>
    public ExceptionDispatchInfo? OriginalExceptionDispatchInfo => _primaryExceptionContainer.OriginalExceptionDispatchInfo;

    private bool _isCancelled;

    public bool IsCancelled =>
        _isCancelled || IsCancellationRequested || Token.IsCancellationRequested || Reason != null;

    private bool _disposed;

    [ExcludeFromCodeCoverage]
    public EngineCancellationToken(IPrimaryExceptionContainer primaryExceptionContainer)
    {
        _primaryExceptionContainer = primaryExceptionContainer;

        Console.CancelKeyPress += (_, args) =>
        {
            args.Cancel = true;
            TryCancel();
        };

        AppDomain.CurrentDomain.ProcessExit += (_, _) => TryCancel();
    }

    public void CancelWithReason(string? reason)
    {
        Reason = reason;
        _isCancelled = true;
        Cancel();
    }

    public void CancelWithException(Exception exception, string? reason = null)
    {
        _primaryExceptionContainer.SetException(exception);

        Reason = reason ?? exception.Message;
        _isCancelled = true;
        Cancel();
    }

    /// <summary>
    /// Communicates a request for cancellation.
    /// </summary>
    public void Cancel()
    {
        if (!_disposed)
        {
            _cts.Cancel();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _cts.Dispose();
        }

        _disposed = true;
    }

    private void TryCancel()
    {
        if (!_disposed && Token.CanBeCanceled)
        {
            _isCancelled = true;
            Cancel();
        }
    }
}
