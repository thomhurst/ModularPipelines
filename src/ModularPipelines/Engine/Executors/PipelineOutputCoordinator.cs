using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
/// 5. Exceptions - Print deferred exceptions.
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
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger<PipelineOutputCoordinator> _logger;
    private readonly TimeProvider _timeProvider;

    public PipelineOutputCoordinator(
        IPrintProgressExecutor printProgressExecutor,
        IConsolePrinter consolePrinter,
        IInternalSummaryLogger summaryLogger,
        IExceptionBuffer exceptionBuffer,
        IConsoleCoordinator consoleCoordinator,
        IOutputCoordinator outputCoordinator,
        IOptions<PipelineOptions> options,
        ILogger<PipelineOutputCoordinator> logger,
        TimeProvider timeProvider)
    {
        _printProgressExecutor = printProgressExecutor;
        _consolePrinter = consolePrinter;
        _summaryLogger = summaryLogger;
        _exceptionBuffer = exceptionBuffer;
        _consoleCoordinator = consoleCoordinator;
        _outputCoordinator = outputCoordinator;
        _options = options;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc />
    public async Task<IPipelineOutputScope> InitializeAsync()
    {
        var liveFlushInterval = _options.Value.Console.ModuleOutputFlushInterval;
        ValidateLiveFlushInterval(liveFlushInterval);

        // Install console coordination before starting progress
        _consoleCoordinator.Install();

        // CRITICAL: Enable output buffering BEFORE starting the progress display task.
        // This prevents a race condition where modules could start executing and logging
        // before the progress display has fully initialized and set the buffering flags.
        // Without this, early module output would be written directly to console instead
        // of being deferred until the progress display ends.
        _consoleCoordinator.EnableOutputBuffering();

        var printProgressExecutor = await _printProgressExecutor.InitializeAsync().ConfigureAwait(false);
        return new PipelineOutputScope(
            printProgressExecutor,
            _consoleCoordinator,
            _outputCoordinator,
            liveFlushInterval,
            _logger,
            _timeProvider);
    }

    /// <inheritdoc />
    public async Task FlushConsoleAsync()
    {
        await FlushWritersAsync(System.Console.Out, System.Console.Error).ConfigureAwait(false);
    }

    internal static async Task FlushWritersAsync(TextWriter output, TextWriter error)
    {
        var exceptions = new List<Exception>();
        await CaptureExceptionAsync(
            () => output.FlushAsync(),
            exceptions).ConfigureAwait(false);
        if (!ReferenceEquals(error, output))
        {
            await CaptureExceptionAsync(
                () => error.FlushAsync(),
                exceptions).ConfigureAwait(false);
        }

        if (exceptions.Count == 1)
        {
            ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
        }

        if (exceptions.Count > 1)
        {
            throw new AggregateException("Console flush failed.", exceptions);
        }
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

    private static void ValidateLiveFlushInterval(TimeSpan liveFlushInterval)
    {
        if (liveFlushInterval < TimeSpan.Zero
            || liveFlushInterval > PipelineConsoleOptions.MaximumModuleOutputFlushInterval)
        {
            throw new ArgumentOutOfRangeException(
                nameof(liveFlushInterval),
                liveFlushInterval,
                $"The interval must be between {TimeSpan.Zero} and " +
                $"{PipelineConsoleOptions.MaximumModuleOutputFlushInterval}.");
        }
    }

    private static async Task CaptureExceptionAsync(
        Func<Task> operation,
        ICollection<Exception> exceptions)
    {
        try
        {
            await operation().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            exceptions.Add(exception);
        }
    }

    private sealed class PipelineOutputScope : IPipelineOutputScope
    {
        private readonly IPrintProgressExecutor _printProgressExecutor;
        private readonly IConsoleCoordinator _consoleCoordinator;
        private readonly IOutputCoordinator _outputCoordinator;
        private readonly ILogger _logger;
        private readonly TimeProvider _timeProvider;
        private readonly CancellationTokenSource _liveFlushCancellation = new();
        private readonly Task _liveFlushTask;

        public PipelineOutputScope(
            IPrintProgressExecutor printProgressExecutor,
            IConsoleCoordinator consoleCoordinator,
            IOutputCoordinator outputCoordinator,
            TimeSpan liveFlushInterval,
            ILogger logger,
            TimeProvider timeProvider)
        {
            _printProgressExecutor = printProgressExecutor;
            _consoleCoordinator = consoleCoordinator;
            _outputCoordinator = outputCoordinator;
            _logger = logger;
            _timeProvider = timeProvider;

            _liveFlushTask = liveFlushInterval > TimeSpan.Zero
                ? FlushPeriodicallyAsync(liveFlushInterval)
                : Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            var exceptions = new List<Exception>();

            await CaptureExceptionAsync(
                () => _liveFlushCancellation.CancelAsync(),
                exceptions).ConfigureAwait(false);
            await CaptureExceptionAsync(
                () => _liveFlushTask,
                exceptions).ConfigureAwait(false);

            // A canceled caller can finish before its queue processor releases the buffer.
            // Final drains must not start until that processor has quiesced.
            await CaptureExceptionAsync(
                () => _outputCoordinator.WaitForPendingFlushesAsync(),
                exceptions).ConfigureAwait(false);

            IReadOnlyList<IModuleOutputBuffer> newlyPopulatedBuffers = [];
            await CaptureExceptionAsync(
                async () =>
                {
                    // CRITICAL: Order matters!
                    // 1. Flush retained console fragments while progress deferral is still active.
                    newlyPopulatedBuffers = await _consoleCoordinator
                        .FlushPendingWritesAsync()
                        .ConfigureAwait(false);
                },
                exceptions).ConfigureAwait(false);

            // 2. Schedule buffers populated after their modules completed.
            foreach (var buffer in newlyPopulatedBuffers)
            {
                await CaptureExceptionAsync(
                    () => _outputCoordinator
                        .OnModuleCompletedAsync(buffer, buffer.ModuleType),
                    exceptions).ConfigureAwait(false);
            }

            // 3. Stop progress display (ends buffering phase).
            await CaptureExceptionAsync(
                () => _printProgressExecutor.DisposeAsync().AsTask(),
                exceptions).ConfigureAwait(false);

            // 4. Flush deferred module output (in completion order).
            await CaptureExceptionAsync(
                () => _outputCoordinator.FlushDeferredAsync(),
                exceptions).ConfigureAwait(false);

            // 5. Flush any unattributed output from coordinator.
            await CaptureExceptionAsync(
                () => _consoleCoordinator.FlushModuleOutputAsync(),
                exceptions).ConfigureAwait(false);

            try
            {
                _liveFlushCancellation.Dispose();
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }

            if (exceptions.Count == 1)
            {
                ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
            }

            if (exceptions.Count > 1)
            {
                throw new AggregateException("Pipeline output teardown failed.", exceptions);
            }
        }

        private async Task FlushPeriodicallyAsync(TimeSpan interval)
        {
            using var timer = new PeriodicTimer(interval, _timeProvider);

            try
            {
                while (await timer
                           .WaitForNextTickAsync(_liveFlushCancellation.Token)
                           .ConfigureAwait(false))
                {
                    try
                    {
                        await _consoleCoordinator
                            .FlushInProgressModuleOutputAsync(_liveFlushCancellation.Token)
                            .ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) when (_liveFlushCancellation.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogWarning(exception, "Failed to flush in-progress module output");
                    }
                }
            }
            catch (OperationCanceledException) when (_liveFlushCancellation.IsCancellationRequested)
            {
                // Normal scope teardown.
            }
        }
    }
}
