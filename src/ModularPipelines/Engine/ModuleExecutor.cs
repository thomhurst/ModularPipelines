using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IServiceProvider serviceProvider)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<ModuleBase>> ExecuteAsync(IEnumerable<ModuleBase> modules)
    {
        var moduleTasks = modules.Select(ExecuteAsync).ToArray();

        if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
        {
            return await moduleTasks.WhenAllFailFast();
        }

        return await Task.WhenAll(moduleTasks);
    }

    private async Task<ModuleBase> ExecuteAsync(ModuleBase module)
    {
        await using var serviceScope = _serviceProvider.CreateAsyncScope();
        
        var scopedServiceProvider = serviceScope.ServiceProvider;

        await scopedServiceProvider.InitializeAsync();
        
        module.Initialize(scopedServiceProvider.GetRequiredService<IModuleContext>());
        
        await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

        try
        {
            await module.StartAsync();
        }
        finally
        {
            await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);
        }

        await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

        return module;
    }
}
