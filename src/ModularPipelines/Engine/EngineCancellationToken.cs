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
    private readonly CancellationToken _token;
    private readonly IPrimaryExceptionContainer _primaryExceptionContainer;
    private readonly object _lifetimeLock = new();

    private int _activeCancellationOperations;
    private int _cancelKeyPressReceived;
    private bool _disposeCancellationTokenSource;
    private bool _ctsDisposed;

    public string? Reason { get; private set; }

    /// <summary>
    /// Gets the cancellation token from the underlying CancellationTokenSource.
    /// </summary>
    public CancellationToken Token => _token;

    /// <summary>
    /// Gets a value indicating whether cancellation has been requested on the underlying token source.
    /// </summary>
    public bool IsCancellationRequested => _token.IsCancellationRequested;

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
        _token = _cts.Token;

        System.Console.CancelKeyPress += OnCancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    public void CancelWithReason(string? reason)
    {
        Reason = reason;
        _isCancelled = true;
        Cancel();
    }

    public void CancelWithException(Exception exception, string? reason = null)
    {
        RecordException(exception);

        Reason = reason ?? exception.Message;
        _isCancelled = true;
        Cancel();
    }

    public void RecordException(Exception exception)
    {
        _primaryExceptionContainer.SetException(exception);
    }

    /// <summary>
    /// Communicates a request for cancellation.
    /// </summary>
    public void Cancel()
    {
        if (!TryBeginCancellation())
        {
            return;
        }

        try
        {
            _cts.Cancel();
        }
        finally
        {
            EndCancellation();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        CancellationTokenSource? cancellationTokenSourceToDispose = null;

        lock (_lifetimeLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _disposeCancellationTokenSource = disposing;

            if (disposing && _activeCancellationOperations == 0)
            {
                _ctsDisposed = true;
                cancellationTokenSourceToDispose = _cts;
            }
        }

        if (disposing)
        {
            System.Console.CancelKeyPress -= OnCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            cancellationTokenSourceToDispose?.Dispose();
        }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs args)
    {
        args.Cancel = HandleCancelKeyPress();
    }

    private void OnProcessExit(object? sender, EventArgs args) => TryCancel();

    internal bool HandleCancelKeyPress()
    {
        if (Interlocked.CompareExchange(ref _cancelKeyPressReceived, 1, 0) != 0)
        {
            return false;
        }

        return TryCancel();
    }

    private bool TryCancel()
    {
        if (!TryBeginCancellation())
        {
            return false;
        }

        try
        {
            _isCancelled = true;
            _cts.Cancel();
            return true;
        }
        finally
        {
            EndCancellation();
        }
    }

    private bool TryBeginCancellation()
    {
        lock (_lifetimeLock)
        {
            if (_disposed)
            {
                return false;
            }

            _activeCancellationOperations++;
            return true;
        }
    }

    private void EndCancellation()
    {
        CancellationTokenSource? cancellationTokenSourceToDispose = null;

        lock (_lifetimeLock)
        {
            _activeCancellationOperations--;

            if (_disposed
                && _disposeCancellationTokenSource
                && !_ctsDisposed
                && _activeCancellationOperations == 0)
            {
                _ctsDisposed = true;
                cancellationTokenSourceToDispose = _cts;
            }
        }

        cancellationTokenSourceToDispose?.Dispose();
    }
}
