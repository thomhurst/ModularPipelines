using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

public interface IPipelineGlobalHooks
{
    Task OnStartAsync(IPipelineContext pipelineContext);
    Task OnEndAsync(IPipelineContext pipelineContext, IReadOnlyList<ModuleBase> modules);
}
