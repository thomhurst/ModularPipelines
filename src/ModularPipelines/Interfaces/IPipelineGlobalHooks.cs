using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

public interface IPipelineGlobalHooks
{
    Task OnStartAsync(IPipelineContext pipelineContext);
    Task OnEndAsync(IPipelineContext pipelineContext, PipelineSummary pipelineSummary);
}
