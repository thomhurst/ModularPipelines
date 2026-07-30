using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Caching;

internal interface IModuleCacheResultRepository
{
    Task SaveResultAsync<T>(
        Module<T> module,
        ModuleResult<T> moduleResult,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken);

    Task<ModuleResult<T>?> GetResultAsync<T>(
        Module<T> module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken);

    void DiscardFingerprint(IModule module);
}
