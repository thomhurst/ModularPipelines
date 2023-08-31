using Microsoft.Extensions.Options;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Engine;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IPipelineConsolePrinter _pipelineConsolePrinter;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IDependencyDetector _dependencyDetector;
    private readonly IModuleLoggerContainer _moduleLoggerContainer;
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogoPrinter _logoPrinter;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IPipelineConsolePrinter pipelineConsolePrinter,
        IRequirementChecker requirementsChecker,
        IModuleRetriever moduleRetriever,
        IModuleExecutor moduleExecutor,
        EngineCancellationToken engineCancellationToken,
        IDependencyDetector dependencyDetector,
        IModuleLoggerContainer moduleLoggerContainer,
        IModuleDisposer moduleDisposer,
        IOptions<PipelineOptions> options,
        ILogoPrinter logoPrinter)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineConsolePrinter = pipelineConsolePrinter;
        _requirementsChecker = requirementsChecker;
        _moduleRetriever = moduleRetriever;
        _moduleExecutor = moduleExecutor;
        _engineCancellationToken = engineCancellationToken;
        _dependencyDetector = dependencyDetector;
        _moduleLoggerContainer = moduleLoggerContainer;
        _moduleDisposer = moduleDisposer;
        _options = options;
        _logoPrinter = logoPrinter;
    }

    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync()
    {
        _logoPrinter.PrintLogo();
        
        _dependencyDetector.Check();

        await _pipelineSetupExecutor.OnStartAsync();

        await _requirementsChecker.CheckRequirementsAsync();

        var organizedModules = await _moduleRetriever.GetOrganizedModules();

        var printProgressCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(_engineCancellationToken.Token);

        var printProgressTask = _pipelineConsolePrinter.PrintProgress(organizedModules, printProgressCancellationTokenSource.Token);

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

            printProgressCancellationTokenSource.CancelAfter(1500);
            
            await printProgressTask;

            await Dispose(runnableModules);

            await _pipelineSetupExecutor.OnEndAsync(organizedModules.AllModules);

            _moduleLoggerContainer.PrintAllLoggers();
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
        if (!AnsiConsole.Profile.Capabilities.Interactive || !_options.Value.ShowProgressInConsole)
        {
            // If not an interactive console, we'll dispose each module as it finishes, to print its output
            // Otherwise we'll do it here, so we don't miss up the Progress printer
            return;
        }
        
        foreach (var module in modulesToProcess)
        {
            await _moduleDisposer.DisposeAsync(module);
        }
    }
}
