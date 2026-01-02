using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

internal class ModuleContextProvider : IPipelineContextProvider, IScopeDisposer, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentBag<IServiceScope> _scopes = new();

    public ModuleContextProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPipelineContext GetModuleContext()
    {
        var serviceScope = _serviceProvider.CreateAsyncScope();

        _scopes.Add(serviceScope);

        return serviceScope.ServiceProvider.GetRequiredService<IPipelineContext>();
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        return _scopes;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var scope in _scopes)
        {
            if (scope is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                scope.Dispose();
            }
        }
    }
}