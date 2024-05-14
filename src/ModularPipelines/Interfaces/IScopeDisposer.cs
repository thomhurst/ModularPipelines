using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Interfaces;

public interface IScopeDisposer : IDisposable
{
    IEnumerable<IServiceScope> GetScopes();
    
    void IDisposable.Dispose()
    {
        foreach (var serviceScope in GetScopes())
        {
            serviceScope.Dispose();
        }
    }
}