using ModularPipelines.Console;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Coordinates all pipeline output operations including progress printing,
/// module output, results display, and exception flushing.
/// </summary>
/// <remarks>
/// <para>
/// <b>Output Phases:</b>
/// 1. Install - Set up console interception
/// 2. Progress - Show progress display while buffering module output
/// 3. Flush - End progress and flush buffered output
/// 4. Results - Print results table
/// 5. Exceptions - Print deferred exceptions
/// </para>
/// </remarks>
internal class PipelineOutputCoordinator : IPipelineOutputCoordinator
{
    private readonly IPrintProgressExecutor _printProgressExecutor;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IInternalSummaryLogger _summaryLogger;
    private readonly IExceptionBuffer _exceptionBuffer;
    private readonly IConsoleCoordinator _consoleCoordinator;
    private readonly IOutputCoordinator _outputCoordinator;

    public PipelineOutputCoordinator(
        IPrintProgressExecutor printProgressExecutor,
        IConsolePrinter consolePrinter,
        IInternalSummaryLogger summaryLogger,
        IExceptionBuffer exceptionBuffer,
        IConsoleCoordinator consoleCoordinator,
        IOutputCoordinator outputCoordinator)
    {
        _printProgressExecutor = printProgressExecutor;
        _consolePrinter = consolePrinter;
        _summaryLogger = summaryLogger;
        _exceptionBuffer = exceptionBuffer;
        _consoleCoordinator = consoleCoordinator;
        _outputCoordinator = outputCoordinator;
    }

    /// <inheritdoc />
    public async Task<IPipelineOutputScope> InitializeAsync()
    {
        // Install console coordination before starting progress
        _consoleCoordinator.Install();

        var printProgressExecutor = await _printProgressExecutor.InitializeAsync().ConfigureAwait(false);
        return new PipelineOutputScope(printProgressExecutor, _consoleCoordinator, _outputCoordinator);
    }

    /// <inheritdoc />
    public void PrintResults(PipelineSummary pipelineSummary)
    {
        _consolePrinter.PrintResults(pipelineSummary);
    }

    /// <inheritdoc />
    public void FlushExceptions()
    {
        // Use coordinator for exception output
        _consoleCoordinator.WriteExceptions();

        // Also flush the exception buffer
        _exceptionBuffer.FlushExceptions();
    }

    /// <inheritdoc />
    public void WriteLogs()
    {
        _summaryLogger.WriteLogs();
    }

    private sealed class PipelineOutputScope : IPipelineOutputScope
    {
        private readonly IPrintProgressExecutor _printProgressExecutor;
        private readonly IConsoleCoordinator _consoleCoordinator;
        private readonly IOutputCoordinator _outputCoordinator;

        public PipelineOutputScope(
            IPrintProgressExecutor printProgressExecutor,
            IConsoleCoordinator consoleCoordinator,
            IOutputCoordinator outputCoordinator)
        {
            _printProgressExecutor = printProgressExecutor;
            _consoleCoordinator = consoleCoordinator;
            _outputCoordinator = outputCoordinator;
        }

        public async ValueTask DisposeAsync()
        {
            // CRITICAL: Order matters!
            // 1. Stop progress display FIRST (ends buffering phase)
            await _printProgressExecutor.DisposeAsync().ConfigureAwait(false);

            // 2. Flush deferred module output (in completion order)
            await _outputCoordinator.FlushDeferredAsync().ConfigureAwait(false);

            // 3. Flush any unattributed output from coordinator
            _consoleCoordinator.FlushModuleOutput();
        }
    }
}
