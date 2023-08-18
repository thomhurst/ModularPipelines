using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IEnumerable<IPipelineGlobalHooks> _globalHooks;
    private readonly IEnumerable<IPipelineModuleHooks> _moduleHooks;
    private readonly IServiceProvider _serviceProvider;

    public PipelineSetupExecutor(IEnumerable<IPipelineGlobalHooks> globalHooks,
        IEnumerable<IPipelineModuleHooks> moduleHooks,
        IServiceProvider serviceProvider)
    {
        _globalHooks = globalHooks;
        _moduleHooks = moduleHooks;
        _serviceProvider = serviceProvider;
    }

    public async Task OnStartAsync()
    {
        await _globalHooks.ToAsyncProcessorBuilder()
            .ForEachAsync(async x =>
            {
                await using var serviceScope = await _serviceProvider.CreateInitializedAsyncScope();
                await x.OnStartAsync(serviceScope.ServiceProvider.GetRequiredService<IModuleContext>());
            })
            .ProcessInParallel();
    }

    public async Task OnEndAsync(IReadOnlyList<ModuleBase> modules)
    {
        await _globalHooks.ToAsyncProcessorBuilder()
            .ForEachAsync(async x =>
            {
                await using var serviceScope = await _serviceProvider.CreateInitializedAsyncScope();
                await x.OnEndAsync(serviceScope.ServiceProvider.GetRequiredService<IModuleContext>(), modules);
            })
            .ProcessInParallel();
    }

    public async Task OnBeforeModuleStartAsync(ModuleBase module)
    {
        await _moduleHooks.ToAsyncProcessorBuilder()
            .ForEachAsync(async x =>
            {
                await x.OnBeforeModuleStartAsync(module.Context, module);
            })
            .ProcessInParallel();
    }

    public async Task OnAfterModuleEndAsync(ModuleBase module)
    {
        await _moduleHooks.ToAsyncProcessorBuilder()
            .ForEachAsync(async x =>
            {
                await x.OnBeforeModuleEndAsync(module.Context, module);
            })
            .ProcessInParallel();
    }
}
