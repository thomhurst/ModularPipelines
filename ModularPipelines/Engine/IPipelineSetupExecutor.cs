using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IPipelineSetupExecutor
{
    Task OnStartAsync();
    Task OnEndAsync();
    
    Task OnBeforeModuleStartAsync(IModule module);
    Task OnAfterModuleEndAsync(IModule module);
}