using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IPipelineConsolePrinter _pipelineConsolePrinter;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly EngineCancellationToken _engineCancellationToken;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IPipelineConsolePrinter pipelineConsolePrinter,
        IRequirementChecker requirementsChecker,
        IModuleRetriever moduleRetriever,
        IModuleExecutor moduleExecutor,
        EngineCancellationToken engineCancellationToken)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineConsolePrinter = pipelineConsolePrinter;
        _requirementsChecker = requirementsChecker;
        _moduleRetriever = moduleRetriever;
        _moduleExecutor = moduleExecutor;
        _engineCancellationToken = engineCancellationToken;
    }
    
    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync()
    {
        await _pipelineSetupExecutor.OnStartAsync();

        await _requirementsChecker.CheckRequirementsAsync();

        var organizedModules = await _moduleRetriever.GetOrganizedModules();

        _pipelineConsolePrinter.PrintProgress(organizedModules, _engineCancellationToken.Token);

        var runnableModules = organizedModules.RunnableModules.Select(x => x.Module).ToList();
        
        try
        {
            await _moduleExecutor.ExecuteAsync(runnableModules);
        }
        catch
        {
            // Give time for the console to update modules to Failed
            await Task.Delay(100);
            _engineCancellationToken.Cancel();
            throw;
        }
        finally
        {
            await WaitForAlwaysRunModules(runnableModules);
            
            await Dispose(runnableModules);
            
            await _pipelineSetupExecutor.OnEndAsync(organizedModules.AllModules);

            await Task.Delay(200);
        }

        return organizedModules.AllModules;
    }

    private async Task WaitForAlwaysRunModules(IEnumerable<ModuleBase> runnableModules)
    {
        try
        {
            await Task.WhenAll(runnableModules.Where(m => m.ModuleRunType == ModuleRunType.AlwaysRun).Select(m => m.ResultTaskInternal));
        }
        catch
        {
            // Ignored
        }
    }

    private async Task Dispose(IEnumerable<ModuleBase> modulesToProcess)
    {
        foreach (var module in modulesToProcess)
        {
            await Dispose(module);
        }
    }

    private static async Task Dispose(ModuleBase module)
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