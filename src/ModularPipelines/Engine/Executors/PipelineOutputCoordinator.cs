using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Coordinates all pipeline output operations including progress printing,
/// module output, results display, and exception flushing.
/// </summary>
internal class PipelineOutputCoordinator : IPipelineOutputCoordinator
{
    private readonly IPrintProgressExecutor _printProgressExecutor;
    private readonly IPrintModuleOutputExecutor _printModuleOutputExecutor;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IAfterPipelineLogger _afterPipelineLogger;
    private readonly IExceptionBuffer _exceptionBuffer;

    public PipelineOutputCoordinator(
        IPrintProgressExecutor printProgressExecutor,
        IPrintModuleOutputExecutor printModuleOutputExecutor,
        IConsolePrinter consolePrinter,
        IAfterPipelineLogger afterPipelineLogger,
        IExceptionBuffer exceptionBuffer)
    {
        _printProgressExecutor = printProgressExecutor;
        _printModuleOutputExecutor = printModuleOutputExecutor;
        _consolePrinter = consolePrinter;
        _afterPipelineLogger = afterPipelineLogger;
        _exceptionBuffer = exceptionBuffer;
    }

    /// <inheritdoc />
    public async Task<IPipelineOutputScope> InitializeAsync()
    {
        var printProgressExecutor = await _printProgressExecutor.InitializeAsync().ConfigureAwait(false);
        return new PipelineOutputScope(_printModuleOutputExecutor, printProgressExecutor);
    }

    /// <inheritdoc />
    public void PrintResults(PipelineSummary pipelineSummary)
    {
        _consolePrinter.PrintResults(pipelineSummary);
    }

    /// <inheritdoc />
    public void FlushExceptions()
    {
        _exceptionBuffer.FlushExceptions();
    }

    /// <inheritdoc />
    public void WriteLogs()
    {
        _afterPipelineLogger.WriteLogs();
    }

    private sealed class PipelineOutputScope : IPipelineOutputScope
    {
        private readonly IPrintModuleOutputExecutor _printModuleOutputExecutor;
        private readonly IPrintProgressExecutor _printProgressExecutor;

        public PipelineOutputScope(
            IPrintModuleOutputExecutor printModuleOutputExecutor,
            IPrintProgressExecutor printProgressExecutor)
        {
            _printModuleOutputExecutor = printModuleOutputExecutor;
            _printProgressExecutor = printProgressExecutor;
        }

        public async ValueTask DisposeAsync()
        {
            _printModuleOutputExecutor.Dispose();
            await _printProgressExecutor.DisposeAsync().ConfigureAwait(false);
        }
    }
}
