using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IModuleContext<PipelineSetupExecutor> _globalModuleContext;
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;
    private readonly IServiceProvider _serviceProvider;

    public PipelineSetupExecutor(IModuleContext<PipelineSetupExecutor> globalModuleContext, 
        IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IServiceProvider serviceProvider)
    {
        _globalModuleContext = globalModuleContext;
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
        _serviceProvider = serviceProvider;
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
        return (IModuleContext) _serviceProvider.GetRequiredService(typeof(IModuleContext<>).MakeGenericType(moduleType));
    }
}