using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal interface IExecutionOrchestrator
{
    Task<PipelineSummary> ExecuteAsync();
}