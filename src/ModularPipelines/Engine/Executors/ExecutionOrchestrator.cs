using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Helpers;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal class ExecutionOrchestrator : IExecutionOrchestrator
{
    private readonly IPipelineInitializer _pipelineInitializer;
    private readonly IModuleDisposeExecutor _moduleDisposeExecutor;
    private readonly IPrintModuleOutputExecutor _printModuleOutputExecutor;
    private readonly IPrintProgressExecutor _printProgressExecutor;
    private readonly IPipelineExecutor _pipelineExecutor;
    private readonly IConsolePrinter _consolePrinter;
    
    private readonly object _lock = new();

    private bool _hasRun;

    public ExecutionOrchestrator(IPipelineInitializer pipelineInitializer,
        IModuleDisposeExecutor moduleDisposeExecutor,
        IPrintModuleOutputExecutor printModuleOutputExecutor,
        IPrintProgressExecutor printProgressExecutor,
        IPipelineExecutor pipelineExecutor,
        IConsolePrinter consolePrinter)
    {
        _pipelineInitializer = pipelineInitializer;
        _moduleDisposeExecutor = moduleDisposeExecutor;
        _printModuleOutputExecutor = printModuleOutputExecutor;
        _printProgressExecutor = printProgressExecutor;
        _pipelineExecutor = pipelineExecutor;
        _consolePrinter = consolePrinter;
    }

    public async Task<PipelineSummary> ExecuteAsync()
    {
        lock (_lock)
        {
            if (_hasRun)
            {
                throw new Exception("This pipeline has already been triggerred.");
            }

            _hasRun = true;
        }

        var organizedModules = await _pipelineInitializer.Initialize();

        var runnableModules = organizedModules.RunnableModules.Select(x => x.Module).ToList();

        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        PipelineSummary? pipelineSummary = null;
        try
        {
            await _moduleDisposeExecutor.ExecuteAndDispose(runnableModules,
                async () =>
                {
                    await _printModuleOutputExecutor.ExecuteAndPrintModuleOutput(async () =>
                    {
                        await _printProgressExecutor.ExecuteWithProgress(organizedModules,
                            async () =>
                            {
                                pipelineSummary =
                                    await _pipelineExecutor.ExecuteAsync(runnableModules, organizedModules);
                            });
                    });
                });
            
            return await CreateAndPrintPipelineSummary(organizedModules, stopWatch, start);
        }
        catch
        {
            await CreateAndPrintPipelineSummary(organizedModules, stopWatch, start);
            throw;
        }
    }

    private async Task<PipelineSummary> CreateAndPrintPipelineSummary(OrganizedModules organizedModules, Stopwatch stopWatch, DateTimeOffset start)
    {
        var end = DateTimeOffset.UtcNow;
        
        var pipelineSummary = new PipelineSummary(organizedModules.AllModules, stopWatch.Elapsed, start, end);

        await WriteResults(pipelineSummary);

        return pipelineSummary;
    }

    private async Task WriteResults(PipelineSummary pipelineSummary)
    {
        _consolePrinter.PrintResults(pipelineSummary);

        await Console.Out.FlushAsync();
    }
}
