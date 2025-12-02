using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

[ExcludeFromCodeCoverage]
internal class NoOpModuleResultRepository : IModuleResultRepository
{
    public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
    {
        return Task.CompletedTask;
    }

    public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineHookContext pipelineContext)
    {
        return Task.FromResult<ModuleResult<T>?>(null);
    }
}