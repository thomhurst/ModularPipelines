using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnStartAsync();
    Task OnEndAsync( IReadOnlyList<ModuleBase> modules );

    Task OnBeforeModuleStartAsync( ModuleBase module );
    Task OnAfterModuleEndAsync( ModuleBase module );
}
