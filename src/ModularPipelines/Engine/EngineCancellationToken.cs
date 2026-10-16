using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages pipeline cancellation state and token.
/// Uses composition instead of inheritance from CancellationTokenSource.
/// Exception storage is delegated to <see cref="IPrimaryExceptionContainer"/>.
/// </summary>
[ExcludeFromCodeCoverage]
internal class EngineCancellationToken : IDisposable
{
    private readonly CancellationTokenSource _cts;
    private readonly CancellationTokenSource _explicitCancellationTokenSource = new();
    private readonly CancellationTokenSource _nonFailureCancellationTokenSource;
    private readonly CancellationToken _token;
    private readonly CancellationToken _nonFailureCancellationToken;
    private readonly CancellationToken _explicitCancellationToken;
    private readonly IPrimaryExceptionContainer _primaryExceptionContainer;
    private readonly Lock _lifetimeLock = new();
    private readonly PosixSignalRegistration? _sigtermRegistration;

    private int _activeCancellationOperations;
    private int _shutdownSignalReceived;
    private bool _disposeCancellationTokenSources;
    private bool _cancellationTokenSourcesDisposed;

    public string? Reason { get; private set; }

    /// <summary>
    /// Gets the cancellation token from the underlying CancellationTokenSource.
    /// </summary>
    public CancellationToken Token => _token;

    /// <summary>
    /// Gets a token that is canceled for user, console, and process-exit cancellation,
    /// but not for pipeline failure cancellation.
    /// </summary>
    public CancellationToken NonFailureCancellationToken => _nonFailureCancellationToken;

    /// <summary>
    /// Gets a token canceled by explicit pipeline cancellation, but not failures or shutdown signals.
    /// Report finalization uses this token so shutdown can still persist the run report.
    /// </summary>
    public CancellationToken ExplicitCancellationToken => _explicitCancellationToken;

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

    private bool _isCanceled;

    public bool IsCanceled =>
        _isCanceled || IsCancellationRequested || Token.IsCancellationRequested || Reason != null;

    private bool _disposed;

    internal Func<bool>? IsRunActive { private get; set; }

    [ExcludeFromCodeCoverage]
    public EngineCancellationToken(IPrimaryExceptionContainer primaryExceptionContainer)
    {
        _primaryExceptionContainer = primaryExceptionContainer;
        _explicitCancellationToken = _explicitCancellationTokenSource.Token;
        _nonFailureCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_explicitCancellationToken);
        _nonFailureCancellationToken = _nonFailureCancellationTokenSource.Token;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(_nonFailureCancellationToken);
        _token = _cts.Token;

        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS() || OperatingSystem.IsFreeBSD())
        {
            _sigtermRegistration = PosixSignalRegistration.Create(
                PosixSignal.SIGTERM,
                context => context.Cancel = HandleSigterm());
        }

        System.Console.CancelKeyPress += OnCancelKeyPress;
    }

    public void CancelWithReason(string? reason)
    {
        Reason = reason;
        _isCanceled = true;
        Cancel(_explicitCancellationTokenSource);
    }

    public void CancelWithException(Exception exception, string? reason = null)
    {
        RecordException(exception);

        Reason = reason ?? exception.Message;
        _isCanceled = true;
        Cancel(_cts);
    }

    public void RecordException(Exception exception)
    {
        _primaryExceptionContainer.SetException(exception);
    }

    /// <summary>
    /// Communicates a request for cancellation.
    /// </summary>
    public void Cancel() => Cancel(_explicitCancellationTokenSource);

    private void Cancel(CancellationTokenSource cancellationTokenSource)
    {
        if (!TryBeginCancellation())
        {
            return;
        }

        try
        {
            cancellationTokenSource.Cancel();
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
        var disposeCancellationTokenSources = false;

        lock (_lifetimeLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _disposeCancellationTokenSources = disposing;

            if (disposing && _activeCancellationOperations == 0)
            {
                _cancellationTokenSourcesDisposed = true;
                disposeCancellationTokenSources = true;
            }
        }

        if (disposing)
        {
            System.Console.CancelKeyPress -= OnCancelKeyPress;
            _sigtermRegistration?.Dispose();
            if (disposeCancellationTokenSources)
            {
                DisposeCancellationTokenSources();
            }
        }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs args)
    {
        args.Cancel = HandleCancelKeyPress();
    }

    internal bool HandleSigterm() => IsRunActive?.Invoke() == true && HandleCancelKeyPress();

    internal bool HandleCancelKeyPress()
    {
        if (Interlocked.CompareExchange(ref _shutdownSignalReceived, 1, 0) != 0)
        {
            return false;
        }

        return RequestShutdown();
    }

    internal bool RequestShutdown()
    {
        if (!TryBeginCancellation())
        {
            return false;
        }

        try
        {
            _isCanceled = true;
            _nonFailureCancellationTokenSource.Cancel();
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
        var disposeCancellationTokenSources = false;

        lock (_lifetimeLock)
        {
            _activeCancellationOperations--;

            if (_disposed
                && _disposeCancellationTokenSources
                && !_cancellationTokenSourcesDisposed
                && _activeCancellationOperations == 0)
            {
                _cancellationTokenSourcesDisposed = true;
                disposeCancellationTokenSources = true;
            }
        }

        if (disposeCancellationTokenSources)
        {
            DisposeCancellationTokenSources();
        }
    }

    private void DisposeCancellationTokenSources()
    {
        _cts.Dispose();
        _nonFailureCancellationTokenSource.Dispose();
        _explicitCancellationTokenSource.Dispose();
    }
}
