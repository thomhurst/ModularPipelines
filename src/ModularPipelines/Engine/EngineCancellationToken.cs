using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages pipeline cancellation state and token.
/// Exception storage is delegated to <see cref="IPrimaryExceptionContainer"/>.
/// </summary>
[ExcludeFromCodeCoverage]
internal class EngineCancellationToken : CancellationTokenSource
{
    private readonly IPrimaryExceptionContainer _primaryExceptionContainer;

    public string? Reason { get; private set; }

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

    protected override void Dispose(bool disposing)
    {
        _disposed = true;
        base.Dispose(disposing);
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
