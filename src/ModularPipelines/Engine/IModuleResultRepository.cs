using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleResultRepository
{
    Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext);

    Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module, IPipelineContext pipelineContext);
}
