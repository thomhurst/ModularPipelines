using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IPipelineExecutor
{
    Task<PipelineSummary> ExecuteAsync(List<IModule> runnableModules, OrganizedModules organizedModules);
}