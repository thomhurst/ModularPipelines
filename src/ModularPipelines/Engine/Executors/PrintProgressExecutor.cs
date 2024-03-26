using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class PrintProgressExecutor : IPrintProgressExecutor
{
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly ILogger<PrintProgressExecutor> _logger;
    
    private Task _printProgressTask = null!; // LateInit
    private CancellationTokenSource _printProgressCancellationTokenSource = null!; // LateInit

    public PrintProgressExecutor(EngineCancellationToken engineCancellationToken,
        IConsolePrinter consolePrinter,
        IModuleRetriever moduleRetriever,
        ILogger<PrintProgressExecutor> logger)
    {
        _engineCancellationToken = engineCancellationToken;
        _consolePrinter = consolePrinter;
        _moduleRetriever = moduleRetriever;
        _logger = logger;
    }
    
    public async Task InitializeAsync()
    {
        _printProgressCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(_engineCancellationToken.Token);

        var organizedModules = await _moduleRetriever.GetOrganizedModules();

        _printProgressTask =
            _consolePrinter.PrintProgress(organizedModules, _printProgressCancellationTokenSource.Token);
    }

    public async ValueTask DisposeAsync()
    {
        _printProgressCancellationTokenSource.CancelAfter(5000);

        await SafelyAwaitProgressPrinter();
    }

    private async Task SafelyAwaitProgressPrinter()
    {
        try
        {
            await _printProgressTask;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error while waiting for progress to update");
        }
    }
}