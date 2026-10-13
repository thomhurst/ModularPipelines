namespace ModularPipelines.Distributed;

/// <summary>
/// Creates a distributed backend on first use and disposes it with its owner.
/// </summary>
/// <remarks>
/// Creation runs once at a time and is shared by concurrent callers, who stop waiting when their own token is
/// canceled. A creation that failed or was canceled is retried by the next caller. Disposal lets an in-flight
/// creation finish for up to <see cref="CreationGracePeriod"/> and disposes its backend. After that it cancels the
/// creation and stops waiting, so a factory that ignores cancellation cannot block host shutdown; a backend that
/// such a creation produces later is disposed as soon as it exists. Callers waiting on a creation that disposal
/// ended get <see cref="ObjectDisposedException"/>.
/// </remarks>
internal sealed class DeferredBackend<TBackend>(Func<CancellationToken, Task<TBackend>> create)
    where TBackend : class
{
    internal static readonly TimeSpan CreationGracePeriod = TimeSpan.FromSeconds(5);

    private readonly Lock _gate = new();
    private readonly CancellationTokenSource _disposeSource = new();
    private Task<TBackend>? _creation;
    private bool _disposed;

    public async ValueTask<TBackend> GetAsync(CancellationToken cancellationToken)
    {
        Task<TBackend> creation;
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_creation is null || _creation.IsFaulted || _creation.IsCanceled)
            {
                _creation = CreateAsync(_disposeSource.Token);
            }

            creation = _creation;
        }

        TBackend backend;
        try
        {
            backend = await creation.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested && IsDisposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        ObjectDisposedException.ThrowIf(IsDisposed, this);
        return backend;
    }

    private bool IsDisposed
    {
        get
        {
            lock (_gate)
            {
                return _disposed;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        var creation = BeginDispose();
        if (creation is null)
        {
            return;
        }

        try
        {
            await creation.WaitAsync(CreationGracePeriod).ConfigureAwait(false);
        }
        catch
        {
            // A failed creation leaves nothing to dispose; one still running is handled below.
        }

        await EndCreationAsync(creation).ConfigureAwait(false);
    }

    public void Dispose()
    {
        var creation = BeginDispose();
        if (creation is null)
        {
            return;
        }

        try
        {
            creation.Wait(CreationGracePeriod);
        }
        catch
        {
            // A failed creation leaves nothing to dispose; one still running is handled below.
        }

        if (creation.IsCompletedSuccessfully)
        {
            DisposeBackend(creation.Result);
            return;
        }

        _disposeSource.Cancel();
        if (!creation.IsCompleted)
        {
            _ = DisposeWhenCreatedAsync(creation);
        }
    }

    private async ValueTask EndCreationAsync(Task<TBackend> creation)
    {
        if (creation.IsCompletedSuccessfully)
        {
            await DisposeBackendAsync(creation.Result).ConfigureAwait(false);
            return;
        }

        await _disposeSource.CancelAsync().ConfigureAwait(false);
        if (!creation.IsCompleted)
        {
            _ = DisposeWhenCreatedAsync(creation);
        }
    }

    // Returns the creation to dispose, or null when already disposed or nothing was created.
    private Task<TBackend>? BeginDispose()
    {
        lock (_gate)
        {
            if (_disposed)
            {
                return null;
            }

            _disposed = true;
            return _creation;
        }
    }

    private async Task<TBackend> CreateAsync(CancellationToken cancellationToken)
    {
        // Yield so a factory that blocks synchronously does not run under the lock.
        await Task.Yield();
        return await create(cancellationToken).ConfigureAwait(false);
    }

    private static async Task DisposeWhenCreatedAsync(Task<TBackend> creation)
    {
        try
        {
            await DisposeBackendAsync(await creation.ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch
        {
            // The owner is already disposed; a failed or canceled creation leaves nothing to dispose.
        }
    }

    private static async ValueTask DisposeBackendAsync(TBackend backend)
    {
        switch (backend)
        {
            case IAsyncDisposable asyncDisposable:
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                break;
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }

    private static void DisposeBackend(TBackend backend)
    {
        switch (backend)
        {
            case IDisposable disposable:
                disposable.Dispose();
                break;
            case IAsyncDisposable asyncDisposable:
                asyncDisposable.DisposeAsync().AsTask().GetAwaiter().GetResult();
                break;
        }
    }
}
