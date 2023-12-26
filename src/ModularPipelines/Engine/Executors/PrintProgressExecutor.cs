using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class PrintProgressExecutor : IPrintProgressExecutor
{
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IPipelineInitializer _pipelineInitializer;
    private readonly ILogger<PrintProgressExecutor> _logger;

    public PrintProgressExecutor(EngineCancellationToken engineCancellationToken,
        IConsolePrinter consolePrinter,
        IPipelineInitializer pipelineInitializer,
        ILogger<PrintProgressExecutor> logger)
    {
        _engineCancellationToken = engineCancellationToken;
        _consolePrinter = consolePrinter;
        _pipelineInitializer = pipelineInitializer;
        _logger = logger;
    }

    public async ValueTask DisposeAsync()
    {
        var printProgressCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(_engineCancellationToken.Token);

        var organizedModules = await _pipelineInitializer.Initialize();

        var printProgressTask =
            _consolePrinter.PrintProgress(organizedModules, printProgressCancellationTokenSource.Token);

        printProgressCancellationTokenSource.CancelAfter(5000);

        await SafelyAwaitProgressPrinter(printProgressTask);
    }

    private async Task SafelyAwaitProgressPrinter(Task printProgressTask)
    {
        try
        {
            await printProgressTask;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error while waiting for progress to update");
        }
    }
}