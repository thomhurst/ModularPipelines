using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

public interface IPipelineModuleHooks
{
    Task OnBeforeModuleStartAsync(IPipelineContext pipelineContext, ModuleBase module);

    Task OnBeforeModuleEndAsync(IPipelineContext pipelineContext, ModuleBase module);
}
