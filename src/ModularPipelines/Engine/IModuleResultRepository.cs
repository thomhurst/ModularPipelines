using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleResultRepository
{
    Task SaveResultAsync<T>(IModule module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext);

    Task<ModuleResult<T>?> GetResultAsync<T>(IModule module, IPipelineHookContext pipelineContext);
}
