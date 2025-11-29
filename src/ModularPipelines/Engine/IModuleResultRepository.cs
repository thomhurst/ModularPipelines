using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleResultRepository
{
    Task SaveResultAsync<T>(IModule<T> module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext);

    Task<ModuleResult<T>?> GetResultAsync<T>(IModule<T> module, IPipelineHookContext pipelineContext);
}