using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

public interface IPipelineModuleHooks
{
    Task OnBeforeModuleStartAsync(IModuleContext moduleContext, ModuleBase module);
    Task OnBeforeModuleEndAsync(IModuleContext moduleContext, ModuleBase module);
}
