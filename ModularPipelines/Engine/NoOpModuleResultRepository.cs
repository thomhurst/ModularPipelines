using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class NoOpModuleResultRepository : IModuleResultRepository
{
    public Task SaveResultAsync<T>( ModuleBase module, ModuleResult<T> moduleResult )
    {
        return Task.CompletedTask;
    }

    public Task<ModuleResult<T>?> GetResultAsync<T>( ModuleBase module )
    {
        return Task.FromResult<ModuleResult<T>?>( null );
    }
}
