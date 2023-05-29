using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IModuleContext _moduleContext;
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;

    public PipelineSetupExecutor(IModuleContext moduleContext, 
        IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks)
    {
        _moduleContext = moduleContext;
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
    }
    
    public Task OnStartAsync()
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnStartAsync(_moduleContext)));
    }

    public Task OnEndAsync()
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnEndAsync(_moduleContext)));
    }
    
    public Task OnBeforeModuleStartAsync(IModule module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleStartAsync(_moduleContext, module)));
    }

    public Task OnAfterModuleEndAsync(IModule module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleEndAsync(_moduleContext, module)));
    }
}