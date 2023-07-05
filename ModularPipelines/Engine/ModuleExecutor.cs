using Microsoft.Extensions.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;

    public ModuleExecutor( IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        IModuleEstimatedTimeProvider moduleEstimatedTimeProvider )
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
    }

    public async Task<IEnumerable<ModuleBase>> ExecuteAsync( IEnumerable<ModuleBase> modules )
    {
        var moduleTasks = modules.Select( ExecuteAsync ).ToArray();

        if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
        {
            return await moduleTasks.WhenAllFailFast();
        }

        return await Task.WhenAll( moduleTasks );
    }

    private async Task<ModuleBase> ExecuteAsync( ModuleBase module )
    {
        await _pipelineSetupExecutor.OnBeforeModuleStartAsync( module );

        await module.StartAsync();

        await _moduleEstimatedTimeProvider.SaveModuleTimeAsync( module.GetType(), module.Duration );

        await _pipelineSetupExecutor.OnAfterModuleEndAsync( module );

        return module;
    }
}
