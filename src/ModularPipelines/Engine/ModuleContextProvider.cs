using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides one scoped pipeline context per module or pipeline operation owner.
/// </summary>
/// <remarks>
/// Context creation and disposal are synchronized. Reference identity keeps distinct module
/// instances isolated, even when they implement value equality. Calls without a key share
/// the pipeline scope. Contexts remain available until the pipeline is disposed.
/// </remarks>
internal class ModuleContextProvider(IServiceProvider serviceProvider) : IPipelineContextProvider, IScopeDisposer, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly Dictionary<object, ContextScope> _scopes = [with(ReferenceEqualityComparer.Instance)];
    private readonly Lock _disposeLock = new();
    private bool _disposed;

    public IPipelineContext GetModuleContext(object? scopeKey = null)
    {
        ContextScope contextScope;
        lock (_disposeLock)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            scopeKey ??= this;
            if (!_scopes.TryGetValue(scopeKey, out contextScope!))
            {
                var scope = _serviceProvider.CreateAsyncScope();
                contextScope = new ContextScope(scope,
                    new Lazy<IPipelineContext>(() => scope.ServiceProvider.GetRequiredService<IPipelineContext>()));
                _scopes.Add(scopeKey, contextScope);
            }
        }

        return contextScope.Context.Value;
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        lock (_disposeLock)
        {
            return [.. _scopes.Values.Select(static entry => (IServiceScope) entry.Scope)];
        }
    }

    public async ValueTask DisposeAsync()
    {
        List<Exception>? exceptions = null;
        foreach (var scope in TakeScopesForDisposal())
        {
            try
            {
                await scope.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                (exceptions ??= []).Add(exception);
            }
        }

        GC.SuppressFinalize(this);
        if (exceptions is not null)
        {
            throw new AggregateException("Failed to dispose module context scopes.", exceptions);
        }
    }

    void IDisposable.Dispose()
    {
        List<Exception>? exceptions = null;
        foreach (var scope in TakeScopesForDisposal())
        {
            try
            {
                scope.Dispose();
            }
            catch (Exception exception)
            {
                (exceptions ??= []).Add(exception);
            }
        }

        GC.SuppressFinalize(this);
        if (exceptions is not null)
        {
            throw new AggregateException("Failed to dispose module context scopes.", exceptions);
        }
    }

    private AsyncServiceScope[] TakeScopesForDisposal()
    {
        lock (_disposeLock)
        {
            if (_disposed)
            {
                return [];
            }

            _disposed = true;
            var scopes = _scopes.Values.Select(static entry => entry.Scope).ToArray();
            _scopes.Clear();
            return scopes;
        }
    }

    private sealed record ContextScope(AsyncServiceScope Scope, Lazy<IPipelineContext> Context);
}
