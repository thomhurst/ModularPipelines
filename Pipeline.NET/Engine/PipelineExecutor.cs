using Microsoft.Extensions.Options;
using Pipeline.NET.Helpers;
using Pipeline.NET.Modules;
using Pipeline.NET.Options;
using Status = Pipeline.NET.Enums.Status;

namespace Pipeline.NET.Engine;

public class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleIgnoreHandler _moduleIgnoreHandler;
    private readonly IPipelineConsolePrinter _pipelineConsolePrinter;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly List<IModule> _modules;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleIgnoreHandler moduleIgnoreHandler,
        IPipelineConsolePrinter pipelineConsolePrinter,
        IOptions<PipelineOptions> pipelineOptions,
        IEnumerable<IModule> modules,
        IRequirementChecker requirementsChecker)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleIgnoreHandler = moduleIgnoreHandler;
        _pipelineConsolePrinter = pipelineConsolePrinter;
        _pipelineOptions = pipelineOptions;
        _requirementsChecker = requirementsChecker;
        _modules = modules.ToList();
    }
    
    public async Task<IModule[]> ExecuteAsync()
    {
        await _pipelineSetupExecutor.OnStartAsync();

        await _requirementsChecker.CheckRequirements();

        var modulesToIgnore = _modules.Where(m => _moduleIgnoreHandler.ShouldIgnore(m)).ToList();
        
        foreach (var module in modulesToIgnore)
        {
            module.Status = Status.Ignored;
        }

        var modulesToProcess = _modules
            .Except(modulesToIgnore)
            .ToList();
        
        _pipelineConsolePrinter.PrintProgress(modulesToProcess, modulesToIgnore);
        
        var moduleProcessingTasks = modulesToProcess
            .Select(x => x.StartProcessingModule())
            .ToList();

        try
        {
            if (_pipelineOptions.Value.StopOnFirstException)
            {
                while (moduleProcessingTasks.Any())
                {
                    var finished = await Task.WhenAny(moduleProcessingTasks);
                    moduleProcessingTasks.Remove(finished);
                }
            }
            else
            {
                await Task.WhenAll(moduleProcessingTasks);
            }
        }
        finally
        {
            await Dispose(modulesToProcess);
        }
        
        await _pipelineSetupExecutor.OnEndAsync();

        await Task.Delay(200);

        return _modules.ToArray();
    }

    private async Task Dispose(List<IModule> modulesToProcess)
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