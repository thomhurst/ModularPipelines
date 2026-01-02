using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

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
        ObjectDisposedException.ThrowIf(_disposed, this);

        var serviceScope = _serviceProvider.CreateAsyncScope();

        _scopes.Add(serviceScope);

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
