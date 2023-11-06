using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
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

    public async Task<IEnumerable<ModuleBase>> ExecuteAsync(IReadOnlyList<ModuleBase> modules)
    {
        var moduleResults = new List<ModuleBase>();
        
        var nonParallelModules = modules
            .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>() != null)
            .ToList();
        
        foreach (var nonParallelModule in nonParallelModules.OrderBy(x => x.DependentModules.Count))
        {
            moduleResults.Add(await ExecuteAsync(nonParallelModule));
        }
        
        var moduleTasks = modules.Except(nonParallelModules).Select(ExecuteAsync).ToArray();
        
        if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
        {
            moduleResults.AddRange(await moduleTasks.WhenAllFailFast());
        }
        else
        {
            moduleResults.AddRange(await Task.WhenAll(moduleTasks));
        }

        return moduleResults;
    }

    public async Task<ModuleBase> ExecuteAsync(ModuleBase module)
    {
        if (module.IsStarted)
        {
            return module;
        }

        await module.Lock!.WaitAsync();
        
        try
        {
            if (module.IsStarted)
            {
                return module;
            }

            await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

            await module.StartAsync();

            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

            await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

            return module;
        }
        finally
        {
            module.Lock?.Release();
            
            if (!AnsiConsole.Profile.Capabilities.Interactive || !_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(module);
            }
        }
    }
}
