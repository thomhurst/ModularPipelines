using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class PrintProgressExecutor : IPrintProgressExecutor
{
    /// <summary>
    /// Grace period in milliseconds for progress printer to complete before forced cancellation.
    /// </summary>
    private const int ProgressPrinterGracePeriodMs = 5000;

    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly ILogger<PrintProgressExecutor> _logger;

    private Task? _printProgressTask;
    private CancellationTokenSource? _printProgressCancellationTokenSource;

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

    public async Task<IPrintProgressExecutor> InitializeAsync()
    {
        _printProgressCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(_engineCancellationToken.Token);

        var organizedModules = await _moduleRetriever.GetOrganizedModules().ConfigureAwait(false);

        _printProgressTask =
            _consolePrinter.PrintProgress(organizedModules, _printProgressCancellationTokenSource.Token);

        return this;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            _printProgressCancellationTokenSource?.CancelAfter(ProgressPrinterGracePeriodMs);
        }
        catch (ObjectDisposedException)
        {
            // Linked CancellationTokenSource may already be disposed if the engine token was cancelled
        }

        await SafelyAwaitProgressPrinter().ConfigureAwait(false);

        _printProgressCancellationTokenSource?.Dispose();
    }

    private async Task SafelyAwaitProgressPrinter()
    {
        try
        {
            if (_printProgressTask != null)
            {
                await _printProgressTask.ConfigureAwait(false);
            }
        }
        catch (Exception e) when (e is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(e, "Error while waiting for progress to update");
        }
    }
}
