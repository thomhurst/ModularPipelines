using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IExecutionOrchestrator
{
    Task<PipelineSummary> ExecuteAsync();
}