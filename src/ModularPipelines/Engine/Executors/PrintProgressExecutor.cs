using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal class PrintProgressExecutor : IPrintProgressExecutor
{
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IConsolePrinter _consolePrinter;
    private readonly ILogger<PrintProgressExecutor> _logger;

    public PrintProgressExecutor(EngineCancellationToken engineCancellationToken,
        IConsolePrinter consolePrinter,
        ILogger<PrintProgressExecutor> logger)
    {
        _engineCancellationToken = engineCancellationToken;
        _consolePrinter = consolePrinter;
        _logger = logger;
    }

    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task ExecuteWithProgress(OrganizedModules organizedModules, Func<Task> executeDelegate)
    {
        var printProgressCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(_engineCancellationToken.Token);

        var printProgressTask =
            _consolePrinter.PrintProgress(organizedModules, printProgressCancellationTokenSource.Token);

        try
        {
            await executeDelegate();
        }
        finally
        {
            printProgressCancellationTokenSource.CancelAfter(5000);

            await SafelyAwaitProgressPrinter(printProgressTask);
        }
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