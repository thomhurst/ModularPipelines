using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
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
            .OrderBy(x => x.DependentModules.Count)
            .ToList();

        var unKeyedNonParallelModules = nonParallelModules
            .Where(x => string.IsNullOrEmpty(x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKey))
            .ToList();
        
        foreach (var nonParallelModule in unKeyedNonParallelModules)
        {
            moduleResults.Add(await ExecuteAsync(nonParallelModule));
        }

        var keyedNonParallelModules = nonParallelModules
            .Where(x => !string.IsNullOrEmpty(x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKey))
            .ToList();
        
        var groupResults = await keyedNonParallelModules
            .Concat(modules.Except(unKeyedNonParallelModules))
            .GroupBy(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()?.ConstraintKey)
            .SelectAsync(x => ProcessGroup(x, moduleResults))
            .ProcessInParallel();
        
        moduleResults.AddRange(groupResults.SelectMany(x => x));
        
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

    private async Task<IEnumerable<ModuleBase>> ProcessGroup(
        IGrouping<string?, ModuleBase> moduleBases,
        ICollection<ModuleBase> moduleResults)
    {
        var executionProcessor = moduleBases.SelectAsync(ExecuteAsync);

        if (string.IsNullOrEmpty(moduleBases.Key))
        {
            return await executionProcessor.ProcessInParallel();
        }

        return await executionProcessor.ProcessOneAtATime();
    }

    public async Task<ModuleBase> ExecuteAsync(ModuleBase module)
    {
        if (module.IsStarted)
        {
            return module;
        }

        await module.Lock!.WaitAsync();

        if (module.IsStarted)
        {
            return module;
        }
        
        try
        {
            await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

            var startTask = module.StartAsync();

            while (!module.IsStarted)
            {
                await Task.Delay(100);
            }

            module.Lock.Release();

            await startTask;

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
