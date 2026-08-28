using System.Diagnostics.CodeAnalysis;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;

using ModularPipelines.Generated;

namespace ModularPipelines.Console;

/// <summary>
/// Coordinates immediate flushing of module output with FIFO ordering and synchronization.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class OutputCoordinator : IOutputCoordinator
{
    private readonly IBuildSystemFormatterProvider _formatterProvider;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly TextWriter _console;
    private readonly ISpectreConsoleLoggerControl _loggerControl;
    private readonly INonSpectreLoggerFactory _nonSpectreLoggerFactory;

    private readonly object _queueLock = new();
    private readonly Queue<PendingFlush> _pendingQueue = new();
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    // Separate lock for deferred output operations to reduce contention
    // with immediate flush operations that use _queueLock
    private readonly object _deferredLock = new();
    private readonly List<DeferredModuleOutput> _deferredOutputs = new();

    private IProgressController _progressController = NoOpProgressController.Instance;
    private bool _isProcessingQueue;
    private TaskCompletionSource _queueIdle = CreateCompletedQueueIdleSource();
    private volatile bool _isProgressActive;

    private readonly record struct DeferredModuleOutput(
        IModuleOutputBuffer Buffer,
        Type ModuleType,
        DateTimeOffset CompletedAt
    );

    public OutputCoordinator(
        IBuildSystemFormatterProvider formatterProvider,
        ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider,
        ISpectreConsoleLoggerControl loggerControl,
        INonSpectreLoggerFactory nonSpectreLoggerFactory)
    {
        _formatterProvider = formatterProvider ?? throw new ArgumentNullException(nameof(formatterProvider));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _loggerControl = loggerControl ?? throw new ArgumentNullException(nameof(loggerControl));
        _nonSpectreLoggerFactory = nonSpectreLoggerFactory
                                   ?? throw new ArgumentNullException(nameof(nonSpectreLoggerFactory));
        _logger = loggerFactory.CreateLogger<OutputCoordinator>();
        _console = System.Console.Out;
    }

    /// <inheritdoc />
    public void SetProgressController(IProgressController controller)
    {
        _progressController = controller;
    }

    /// <inheritdoc />
    public void SetProgressActive(bool isActive)
    {
        lock (_deferredLock)
        {
            if (isActive)
            {
                // Starting a new progress session - check for stale deferred outputs
                // from a previous run that crashed before FlushDeferredAsync was called
                if (_deferredOutputs.Count > 0)
                {
                    _logger.LogWarning(
                        "Found {Count} stale deferred outputs from a previous pipeline run. " +
                        "This indicates FlushDeferredAsync was not called. Clearing to prevent memory leak.",
                        _deferredOutputs.Count);
                    _deferredOutputs.Clear();
                }
            }

            // Note: We do NOT clear deferred outputs when progress ends.
            // FlushDeferredAsync() is called separately after progress ends and will
            // properly flush any remaining outputs. Clearing here would lose outputs
            // because the disposal order is: DisposeProgress -> FlushDeferred
        }

        _isProgressActive = isActive;
    }

    /// <inheritdoc />
    public async Task OnModuleCompletedAsync(IModuleOutputBuffer buffer, Type moduleType, CancellationToken cancellationToken = default)
    {
        if (!buffer.NeedsCompletionFlush)
        {
            return;
        }

        if (_isProgressActive)
        {
            // Progress is active - defer output until pipeline end
            // Uses separate lock to avoid contention with immediate flush operations
            lock (_deferredLock)
            {
                _deferredOutputs.Add(new DeferredModuleOutput(
                    buffer,
                    moduleType,
                    DateTimeOffset.UtcNow
                ));
            }
        }
        else
        {
            // No progress - flush immediately (existing behavior)
            await EnqueueAndFlushAsync(buffer, OutputFlushKind.Complete, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task FlushDeferredAsync(CancellationToken cancellationToken = default)
    {
        List<DeferredModuleOutput> toFlush;

        lock (_deferredLock)
        {
            if (_deferredOutputs.Count == 0)
            {
                return;
            }

            // Order by completion time to maintain consistent output ordering
            toFlush = _deferredOutputs
                .OrderBy(x => x.CompletedAt)
                .ToList();
            _deferredOutputs.Clear();
        }

        var formatter = _formatterProvider.GetFormatter();

        var nextOutputIndex = 0;
        var lockTaken = false;
        try
        {
            await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            lockTaken = true;

            for (; nextOutputIndex < toFlush.Count; nextOutputIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await FlushBufferAsync(
                        toFlush[nextOutputIndex].Buffer,
                        formatter,
                        OutputFlushKind.Complete,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
        {
            var unflushedOutputs = toFlush.Skip(nextOutputIndex).ToArray();
            ReportDeferredFlushFailure(unflushedOutputs, exception);
            RequeueDeferredOutputs(unflushedOutputs);

            throw;
        }
        catch (Exception exception)
        {
            // Preserve the current buffer too. Its own render cursor retains the
            // failed item, preferring a possible duplicate over lost diagnostics.
            var unflushedOutputs = toFlush.Skip(nextOutputIndex).ToArray();
            ReportDeferredFlushFailure(unflushedOutputs, exception);
            RequeueDeferredOutputs(unflushedOutputs);

            throw;
        }
        finally
        {
            if (lockTaken)
            {
                _writeLock.Release();
            }
        }
    }

    /// <inheritdoc />
    public async Task EnqueueAndFlushAsync(
        IModuleOutputBuffer buffer,
        OutputFlushKind flushKind,
        CancellationToken cancellationToken = default)
    {
        if (flushKind is OutputFlushKind.Incremental && buffer.IsComplete)
        {
            return;
        }

        if (flushKind is OutputFlushKind.Incremental && !buffer.HasOutput)
        {
            return;
        }

        var pending = new PendingFlush(buffer, flushKind, cancellationToken);
        bool shouldProcess;

        lock (_queueLock)
        {
            _pendingQueue.Enqueue(pending);
            shouldProcess = !_isProcessingQueue;
            if (shouldProcess)
            {
                _isProcessingQueue = true;
                _queueIdle = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            }
        }

        if (shouldProcess)
        {
            ScheduleQueueProcessing();
        }

        // Every caller observes only the outcome of its own buffer. Queue processing
        // continues independently so the owner is not held behind later buffers.
        using var cancellationRegistration = cancellationToken.Register(
            () => pending.CompletionSource.TrySetCanceled(cancellationToken));
        await pending.CompletionSource.Task.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task WaitForPendingFlushesAsync(CancellationToken cancellationToken = default)
    {
        lock (_queueLock)
        {
            return _queueIdle.Task.WaitAsync(cancellationToken);
        }
    }

    private async Task ProcessQueueAsync()
    {
        try
        {
            var formatter = _formatterProvider.GetFormatter();
            await _writeLock.WaitAsync().ConfigureAwait(false);
            try
            {
                var progressController = _progressController;
                await progressController.PauseAsync().ConfigureAwait(false);
                try
                {
                    PendingFlush? pending;
                    while ((pending = DequeuePendingFlush()) is not null)
                    {
                        await ProcessPendingFlushAsync(pending, formatter).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await progressController.ResumeAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                _writeLock.Release();
            }
        }
        catch (Exception ex)
        {
            foreach (var pending in DrainPendingFlushes())
            {
                pending.CompletionSource.TrySetException(ex);
            }

            _logger.LogError(ex, "Output queue processing terminated unexpectedly");
        }
        finally
        {
            if (ReleaseQueueOwnership())
            {
                ScheduleQueueProcessing();
            }
        }
    }

    private void ScheduleQueueProcessing()
    {
        _ = Task.Run(ProcessQueueAsync);
    }

    private PendingFlush? DequeuePendingFlush()
    {
        lock (_queueLock)
        {
            return _pendingQueue.Count == 0 ? null : _pendingQueue.Dequeue();
        }
    }

    private async Task ProcessPendingFlushAsync(PendingFlush pending, IBuildSystemFormatter formatter)
    {
        try
        {
            if (pending.FlushKind is OutputFlushKind.Incremental && pending.Buffer.IsComplete)
            {
                pending.CompletionSource.TrySetResult();
                return;
            }

            // Output sinks are not transactional. The buffer retains a failed item,
            // preferring a possible duplicate on a later retry over guaranteed data loss.
            // Surface this attempt's failure so the caller controls when to retry.
            await FlushPendingOnceAsync(pending, formatter).ConfigureAwait(false);
            pending.CompletionSource.TrySetResult();
        }
        catch (OperationCanceledException ex)
        {
            pending.CompletionSource.TrySetCanceled(ex.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to flush output for {ModuleType}", pending.Buffer.ModuleType.Name);
            pending.CompletionSource.TrySetException(ex);
        }
    }

    private async Task FlushPendingOnceAsync(PendingFlush pending, IBuildSystemFormatter formatter)
    {
        var cancellationToken = pending.CancellationToken;
        cancellationToken.ThrowIfCancellationRequested();

        await FlushBufferAsync(
                pending.Buffer,
                formatter,
                pending.FlushKind,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private PendingFlush[] DrainPendingFlushes()
    {
        lock (_queueLock)
        {
            var pending = _pendingQueue.ToArray();
            _pendingQueue.Clear();
            return pending;
        }
    }

    private void RequeueDeferredOutputs(IEnumerable<DeferredModuleOutput> outputs)
    {
        lock (_deferredLock)
        {
            _deferredOutputs.InsertRange(0, outputs);
        }
    }

    private static void ReportDeferredFlushFailure(
        IEnumerable<DeferredModuleOutput> outputs,
        Exception exception)
    {
        foreach (var output in outputs)
        {
            output.Buffer.ReportDeferredFlushFailure(exception);
        }
    }

    private bool ReleaseQueueOwnership()
    {
        lock (_queueLock)
        {
            _isProcessingQueue = false;
            if (_pendingQueue.Count == 0)
            {
                _queueIdle.TrySetResult();
                return false;
            }

            _isProcessingQueue = true;
            return true;
        }
    }

    private static TaskCompletionSource CreateCompletedQueueIdleSource()
    {
        var completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        completionSource.SetResult();
        return completionSource;
    }

    private async Task FlushBufferAsync(
        IModuleOutputBuffer buffer,
        IBuildSystemFormatter formatter,
        OutputFlushKind flushKind,
        CancellationToken cancellationToken)
    {
        await FlushBufferOnceAsync(buffer, formatter, flushKind, cancellationToken)
            .ConfigureAwait(false);

        if (flushKind is OutputFlushKind.Complete && buffer.HasStructuredDeliveryRetries)
        {
            await FlushBufferOnceAsync(buffer, formatter, flushKind, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task FlushBufferOnceAsync(
        IModuleOutputBuffer buffer,
        IBuildSystemFormatter formatter,
        OutputFlushKind flushKind,
        CancellationToken cancellationToken)
    {
        var moduleLogger = GetModuleLogger(buffer.ModuleType);

        using var directWrite = CoordinatedTextWriter.BeginDirectWrite();
        await buffer
            .FlushToAsync(
                _console,
                formatter,
                moduleLogger,
                _loggerControl,
                flushKind,
                _nonSpectreLoggerFactory.CreateLoggers(
                    OutputLoggerCategories.ForModule(buffer.ModuleType)),
                cancellationToken)
            .ConfigureAwait(false);
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; MakeGenericType is the documented fallback for dynamic modules.")]
    private ILogger GetModuleLogger(Type moduleType)
    {
        if (moduleType == typeof(void))
        {
            return _loggerFactory.CreateLogger(OutputLoggerCategories.Pipeline);
        }

        if (GeneratedModuleMetadata.TryGetRuntime(moduleType, out var runtime))
        {
            return runtime.GetOutputLogger(_serviceProvider);
        }

        var loggerType = typeof(ILogger<>).MakeGenericType(moduleType);
        return _serviceProvider.GetService(loggerType) as ILogger
               ?? _loggerFactory.CreateLogger(moduleType);
    }

    private sealed class PendingFlush
    {
        public IModuleOutputBuffer Buffer { get; }

        public CancellationToken CancellationToken { get; }

        public OutputFlushKind FlushKind { get; }

        public TaskCompletionSource CompletionSource { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public PendingFlush(
            IModuleOutputBuffer buffer,
            OutputFlushKind flushKind,
            CancellationToken cancellationToken)
        {
            Buffer = buffer;
            FlushKind = flushKind;
            CancellationToken = cancellationToken;
        }
    }
}
