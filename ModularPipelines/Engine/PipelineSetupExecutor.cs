using ModularPipelines.Context;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IModuleContext<PipelineSetupExecutor> _globalModuleContext;
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;
    private readonly IModuleContextCreator _moduleContextCreator;

    public PipelineSetupExecutor(IModuleContext<PipelineSetupExecutor> globalModuleContext, 
        IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IModuleContextCreator moduleContextCreator)
    {
        _globalModuleContext = globalModuleContext;
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
        _moduleContextCreator = moduleContextCreator;
    }
    
    public Task OnStartAsync()
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnStartAsync(_globalModuleContext)));
    }

    public Task OnEndAsync(IReadOnlyList<ModuleBase> modules)
    {
        return Task.WhenAll(_globalHooks.Select(x => x.OnEndAsync(_globalModuleContext, modules)));
    }
    
    public Task OnBeforeModuleStartAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleStartAsync(CreateModuleContext(module.GetType()), module)));
    }

    public Task OnAfterModuleEndAsync(ModuleBase module)
    {
        return Task.WhenAll(_moduleHooks.Select(x => x.OnBeforeModuleEndAsync(CreateModuleContext(module.GetType()), module)));
    }

    private IModuleContext CreateModuleContext(Type moduleType)
    {
        return _moduleContextCreator.CreateContext(moduleType);
    }
}