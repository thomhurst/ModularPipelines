using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleResultRepository
{
    Task PersistResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult);

    Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module);
}