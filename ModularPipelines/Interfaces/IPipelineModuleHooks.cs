using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

public interface IPipelineModuleHooks
{
    Task OnBeforeModuleStartAsync(IModuleContext moduleContext, IModule module);
    Task OnBeforeModuleEndAsync(IModuleContext moduleContext, IModule module);
}