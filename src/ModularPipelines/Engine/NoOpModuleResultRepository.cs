using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

[ExcludeFromCodeCoverage]
internal class NoOpModuleResultRepository : IModuleResultRepository
{
    public Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext)
    {
        return Task.CompletedTask;
    }

    public Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module, IPipelineContext pipelineContext)
    {
        return Task.FromResult<ModuleResult<T>?>(null);
    }
}
