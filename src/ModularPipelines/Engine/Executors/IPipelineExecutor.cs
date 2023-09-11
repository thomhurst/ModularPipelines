using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IPipelineExecutor
{
    Task<PipelineSummary> ExecuteAsync(List<ModuleBase> runnableModules, OrganizedModules organizedModules);
}
