using Microsoft.Extensions.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleContextCreator _moduleContextCreator;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleContextCreator moduleContextCreator,
        IOptions<PipelineOptions> pipelineOptions)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleContextCreator = moduleContextCreator;
        _pipelineOptions = pipelineOptions;
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
        await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

        await module.StartAsync();

        await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

        return module;
    }
}