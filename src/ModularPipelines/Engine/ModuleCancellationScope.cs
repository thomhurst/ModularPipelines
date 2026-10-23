namespace ModularPipelines.Engine;

/// <summary>
/// Flows the active module or attempt token to commands invoked through shared services.
/// </summary>
internal sealed class ModuleCancellationScope : IDisposable
{
    private static readonly AsyncLocal<ModuleCancellationScope?> Storage = new();
    private readonly ModuleCancellationScope? _previous;
    private readonly CancellationToken _token;
    private int _disposed;

    public ModuleCancellationScope(CancellationToken token)
    {
        _previous = Storage.Value;
        _token = token;
        Storage.Value = this;
    }

    public static CancellationToken CurrentToken => Storage.Value?._token ?? default;

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        // Flowed children retain this token, including cancellation from an abandoned attempt.
        // Restore only this execution context; never substitute a later attempt's token in children.
        if (ReferenceEquals(Storage.Value, this))
        {
            Storage.Value = _previous;
        }
    }
}
