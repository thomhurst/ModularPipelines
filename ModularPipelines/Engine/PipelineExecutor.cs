using Microsoft.Extensions.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IPipelineConsolePrinter _pipelineConsolePrinter;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly IServiceProvider _serviceProvider;
    

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IPipelineConsolePrinter pipelineConsolePrinter,
        IOptions<PipelineOptions> pipelineOptions,
        IRequirementChecker requirementsChecker,
        IModuleRetriever moduleRetriever,
        IServiceProvider serviceProvider)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineConsolePrinter = pipelineConsolePrinter;
        _pipelineOptions = pipelineOptions;
        _requirementsChecker = requirementsChecker;
        _moduleRetriever = moduleRetriever;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync()
    {
        await _pipelineSetupExecutor.OnStartAsync();

        await _requirementsChecker.CheckRequirements();

        var organizedModules = _moduleRetriever.GetOrganizedModules();

        _pipelineConsolePrinter.PrintProgress(organizedModules);
        
        var moduleProcessingTasks = organizedModules.RunnableModules
            .Select(x => x.StartAsync(_serviceProvider))
            .ToList();

        try
        {
            if (_pipelineOptions.Value.StopOnFirstException)
            {
                await moduleProcessingTasks.WhenAllFailFast();
            }
            else
            {
                await Task.WhenAll(moduleProcessingTasks);
            }
        }
        finally
        {
            await Dispose(organizedModules.RunnableModules);
            
            await _pipelineSetupExecutor.OnEndAsync(organizedModules.AllModules);

            await Task.Delay(200);
        }

        return organizedModules.AllModules;
    }

    private async Task Dispose(IEnumerable<ModuleBase> modulesToProcess)
    {
        foreach (var module in modulesToProcess)
        {
            if (module is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }

            if (module is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}