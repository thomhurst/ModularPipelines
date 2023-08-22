using Microsoft.Extensions.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly IModuleDisposer _moduleDisposer;

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
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
        try
        {
            await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

            await module.StartAsync();

            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

            await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

            return module;
        }
        finally
        {
            if (!AnsiConsole.Profile.Capabilities.Interactive || !_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(module);
            }
        }
    }
}
