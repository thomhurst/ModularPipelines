using System.Diagnostics.CodeAnalysis;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;

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

    private readonly object _queueLock = new();
    private readonly Queue<PendingFlush> _pendingQueue = new();
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    // Separate lock for deferred output operations to reduce contention
    // with immediate flush operations that use _queueLock
    private readonly object _deferredLock = new();

    private IProgressController _progressController = NoOpProgressController.Instance;
    private bool _isProcessingQueue;
    private volatile bool _isProgressActive;
    private readonly List<DeferredModuleOutput> _deferredOutputs = new();

    private readonly record struct DeferredModuleOutput(
        IModuleOutputBuffer Buffer,
        Type ModuleType,
        DateTimeOffset CompletedAt
    );

    public OutputCoordinator(
        IBuildSystemFormatterProvider formatterProvider,
        ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider,
        ISpectreConsoleLoggerControl loggerControl)
    {
        _formatterProvider = formatterProvider ?? throw new ArgumentNullException(nameof(formatterProvider));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _loggerControl = loggerControl ?? throw new ArgumentNullException(nameof(loggerControl));
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
        if (!buffer.HasOutput)
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
            await EnqueueAndFlushAsync(buffer, cancellationToken).ConfigureAwait(false);
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
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            RequeueDeferredOutputs(toFlush.Skip(nextOutputIndex));

            throw;
        }
        catch
        {
            // A sink may have accepted part of the current buffer before throwing.
            // Retrying that buffer could duplicate delivered output, so only retain
            // buffers that have not started rendering.
            RequeueDeferredOutputs(toFlush.Skip(nextOutputIndex + 1));

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
    public async Task EnqueueAndFlushAsync(IModuleOutputBuffer buffer, CancellationToken cancellationToken = default)
    {
        if (!buffer.HasOutput)
        {
            return;
        }

        var pending = new PendingFlush(buffer, cancellationToken);
        bool shouldProcess;

        lock (_queueLock)
        {
            _pendingQueue.Enqueue(pending);
            shouldProcess = !_isProcessingQueue;
            if (shouldProcess)
            {
                _isProcessingQueue = true;
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

    private async Task ProcessQueueAsync()
    {
        try
        {
            var formatter = _formatterProvider.GetFormatter();
            PendingFlush? pending;

            while ((pending = DequeuePendingFlush()) is not null)
            {
                await ProcessPendingFlushAsync(pending, formatter).ConfigureAwait(false);
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
            // Output sinks are not transactional. A provider can deliver an event and
            // then throw, so retrying here could duplicate output already accepted by
            // that provider. Preserve the buffer's unrendered tail and surface failure
            // to the caller instead.
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

        await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _progressController.PauseAsync().ConfigureAwait(false);
            try
            {
                await FlushBufferAsync(pending.Buffer, formatter, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                await _progressController.ResumeAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            _writeLock.Release();
        }
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

    private bool ReleaseQueueOwnership()
    {
        lock (_queueLock)
        {
            _isProcessingQueue = false;
            if (_pendingQueue.Count == 0)
            {
                return false;
            }

            _isProcessingQueue = true;
            return true;
        }
    }

    private async Task FlushBufferAsync(
        IModuleOutputBuffer buffer,
        IBuildSystemFormatter formatter,
        CancellationToken cancellationToken)
    {
        var loggerType = typeof(ILogger<>).MakeGenericType(buffer.ModuleType);
        var moduleLogger = (ILogger) _serviceProvider.GetService(loggerType)
                           ?? _loggerFactory.CreateLogger(buffer.ModuleType);

        using var directWrite = CoordinatedTextWriter.BeginDirectWrite();
        await buffer
            .FlushToAsync(_console, formatter, moduleLogger, _loggerControl, cancellationToken)
            .ConfigureAwait(false);
    }

    private sealed class PendingFlush
    {
        public IModuleOutputBuffer Buffer { get; }

        public CancellationToken CancellationToken { get; }

        public TaskCompletionSource CompletionSource { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public PendingFlush(IModuleOutputBuffer buffer, CancellationToken cancellationToken)
        {
            Buffer = buffer;
            CancellationToken = cancellationToken;
        }
    }
}
