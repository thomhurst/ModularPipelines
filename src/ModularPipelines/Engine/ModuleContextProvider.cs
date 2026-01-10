using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides scoped pipeline contexts for module execution.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="GetModuleContext"/>
/// method can be called concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// <b>Synchronization Strategy:</b> Uses a lock (<c>_disposeLock</c>) to coordinate between
/// context creation and disposal. The lock ensures that:
/// - No new scopes are created after disposal begins
/// - All scopes created before disposal are captured for cleanup
/// ConcurrentBag is used because we only need add (during creation) and enumerate (during disposal),
/// and it provides efficient lock-free adds while allowing enumeration via snapshot.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class ModuleContextProvider : IPipelineContextProvider, IScopeDisposer, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentBag<IServiceScope> _scopes = new();
    private readonly object _disposeLock = new();
    private bool _disposed;

    public ModuleContextProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPipelineContext GetModuleContext()
    {
        AsyncServiceScope serviceScope;

        lock (_disposeLock)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            // Create and add scope inside lock to ensure atomicity with disposal.
            // This prevents a race where disposal could take a snapshot between
            // the _disposed check and the Add operation, leaving the scope undisposed.
            serviceScope = _serviceProvider.CreateAsyncScope();
            _scopes.Add(serviceScope);
        }

        return serviceScope.ServiceProvider.GetRequiredService<IPipelineContext>();
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        // Return empty if disposed to prevent IScopeDisposer.Dispose from double-disposing
        if (_disposed)
        {
            return [];
        }

        return _scopes;
    }

    public async ValueTask DisposeAsync()
    {
        IServiceScope[] scopesToDispose;

        lock (_disposeLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            // Take snapshot inside lock to ensure atomicity.
            // Any thread that passed the _disposed check in GetModuleContext
            // before we set it will have already added to _scopes.
            scopesToDispose = _scopes.ToArray();
        }

        foreach (var scope in scopesToDispose)
        {
            // AsyncServiceScope always implements IAsyncDisposable
            await ((IAsyncDisposable)scope).DisposeAsync().ConfigureAwait(false);
        }

        GC.SuppressFinalize(this);
    }

    // Explicit implementation to coordinate with DisposeAsync and prevent double disposal
    void IDisposable.Dispose()
    {
        IServiceScope[] scopesToDispose;

        lock (_disposeLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            // Take snapshot inside lock to ensure atomicity
            scopesToDispose = _scopes.ToArray();
        }

        foreach (var scope in scopesToDispose)
        {
            scope.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
