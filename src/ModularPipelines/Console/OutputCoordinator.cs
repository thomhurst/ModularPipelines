using System.Diagnostics.CodeAnalysis;
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

    private readonly object _queueLock = new();
    private readonly Queue<PendingFlush> _pendingQueue = new();
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    // Separate lock for deferred output operations to reduce contention
    // with immediate flush operations that use _queueLock
    private readonly object _deferredLock = new();

    private IProgressController _progressController = NoOpProgressController.Instance;
    private bool _isProcessingQueue;
    private volatile bool _isFlushingOutput;
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
        IServiceProvider serviceProvider)
    {
        _formatterProvider = formatterProvider ?? throw new ArgumentNullException(nameof(formatterProvider));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = loggerFactory.CreateLogger<OutputCoordinator>();
        _console = System.Console.Out;
    }

    /// <inheritdoc />
    public bool IsFlushing => _isFlushingOutput;

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
            else
            {
                // Progress ending - clean up any remaining deferred outputs that weren't flushed
                // This handles cancellation scenarios where FlushDeferredAsync wasn't called
                if (_deferredOutputs.Count > 0)
                {
                    _logger.LogWarning(
                        "Progress ended with {Count} unflushed deferred outputs. " +
                        "This indicates FlushDeferredAsync was not called (possibly due to cancellation). " +
                        "Clearing to prevent memory leak.",
                        _deferredOutputs.Count);
                    _deferredOutputs.Clear();
                }
            }
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

        await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            _isFlushingOutput = true;
            try
            {
                foreach (var output in toFlush)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    FlushBuffer(output.Buffer, formatter);
                }
            }
            finally
            {
                _isFlushingOutput = false;
            }
        }
        finally
        {
            _writeLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task EnqueueAndFlushAsync(IModuleOutputBuffer buffer, CancellationToken cancellationToken = default)
    {
        if (!buffer.HasOutput)
        {
            return;
        }

        var pending = new PendingFlush(buffer);
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
            await ProcessQueueAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            // Wait for our buffer to be flushed by the current flusher
            await pending.CompletionSource.Task.ConfigureAwait(false);
        }
    }

    private async Task ProcessQueueAsync(CancellationToken cancellationToken)
    {
        var formatter = _formatterProvider.GetFormatter();

        while (true)
        {
            PendingFlush? pending;

            lock (_queueLock)
            {
                if (_pendingQueue.Count == 0)
                {
                    _isProcessingQueue = false;
                    return;
                }

                pending = _pendingQueue.Dequeue();
            }

            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    pending.CompletionSource.TrySetCanceled(cancellationToken);
                    continue;
                }

                await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    await _progressController.PauseAsync().ConfigureAwait(false);
                    _isFlushingOutput = true;
                    try
                    {
                        FlushBuffer(pending.Buffer, formatter);
                    }
                    finally
                    {
                        _isFlushingOutput = false;
                        await _progressController.ResumeAsync().ConfigureAwait(false);
                    }
                }
                finally
                {
                    _writeLock.Release();
                }

                pending.CompletionSource.TrySetResult();
            }
            catch (OperationCanceledException ex)
            {
                pending.CompletionSource.TrySetCanceled(ex.CancellationToken);
            }
            catch (Exception ex)
            {
                // Log error but don't fail - module execution already succeeded
                _logger.LogError(ex, "Failed to flush output for {ModuleType}", pending.Buffer.ModuleType.Name);
                pending.CompletionSource.TrySetResult();
            }
        }
    }

    private void FlushBuffer(IModuleOutputBuffer buffer, IBuildSystemFormatter formatter)
    {
        var loggerType = typeof(ILogger<>).MakeGenericType(buffer.ModuleType);
        var moduleLogger = (ILogger)_serviceProvider.GetService(loggerType)
                           ?? _loggerFactory.CreateLogger(buffer.ModuleType);

        buffer.FlushTo(_console, formatter, moduleLogger);
    }

    private sealed class PendingFlush
    {
        public IModuleOutputBuffer Buffer { get; }
        public TaskCompletionSource CompletionSource { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public PendingFlush(IModuleOutputBuffer buffer)
        {
            Buffer = buffer;
        }
    }
}
