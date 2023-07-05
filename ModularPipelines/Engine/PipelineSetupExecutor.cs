using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IModuleContext _moduleContext;
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;

    public PipelineSetupExecutor( IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IModuleContext moduleContext )
    {
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
        _moduleContext = moduleContext;
    }

    public Task OnStartAsync()
    {
        return Task.WhenAll( _globalHooks.Select( x => x.OnStartAsync( _moduleContext ) ) );
    }

    public Task OnEndAsync( IReadOnlyList<ModuleBase> modules )
    {
        return Task.WhenAll( _globalHooks.Select( x => x.OnEndAsync( _moduleContext, modules ) ) );
    }

    public Task OnBeforeModuleStartAsync( ModuleBase module )
    {
        return Task.WhenAll( _moduleHooks.Select( x => x.OnBeforeModuleStartAsync( _moduleContext, module ) ) );
    }

    public Task OnAfterModuleEndAsync( ModuleBase module )
    {
        return Task.WhenAll( _moduleHooks.Select( x => x.OnBeforeModuleEndAsync( _moduleContext, module ) ) );
    }
}
