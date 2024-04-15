using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

internal class ModuleContextProvider : IPipelineContextProvider, IScopeDisposer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IServiceScope> _scopes = new();

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
}