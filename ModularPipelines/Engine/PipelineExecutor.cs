using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IPipelineConsolePrinter _pipelineConsolePrinter;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly IModuleExecutor _moduleExecutor;

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IPipelineConsolePrinter pipelineConsolePrinter,
        IRequirementChecker requirementsChecker,
        IModuleRetriever moduleRetriever,
        IModuleExecutor moduleExecutor)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineConsolePrinter = pipelineConsolePrinter;
        _requirementsChecker = requirementsChecker;
        _moduleRetriever = moduleRetriever;
        _moduleExecutor = moduleExecutor;
    }
    
    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync()
    {
        await _pipelineSetupExecutor.OnStartAsync();

        await _requirementsChecker.CheckRequirementsAsync();

        var organizedModules = _moduleRetriever.GetOrganizedModules();

        _pipelineConsolePrinter.PrintProgress(organizedModules, _cancellationTokenSource.Token);

        try
        {
            await _moduleExecutor.ExecuteAsync(organizedModules.RunnableModules);
        }
        catch
        {
            // Give time for the console to update modules to Failed
            await Task.Delay(100);
            _cancellationTokenSource.Cancel();
            throw;
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