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
    private readonly IConsoleCoordinator _consoleCoordinator;
    private readonly ILogger _logger;
    private readonly TextWriter _console;

    private readonly object _queueLock = new();
    private readonly Queue<PendingFlush> _pendingQueue = new();
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    private IProgressController _progressController = NoOpProgressController.Instance;
    private bool _isFlushing;

    public OutputCoordinator(
        IBuildSystemFormatterProvider formatterProvider,
        ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider,
        IConsoleCoordinator consoleCoordinator)
    {
        _formatterProvider = formatterProvider;
        _loggerFactory = loggerFactory;
        _serviceProvider = serviceProvider;
        _consoleCoordinator = consoleCoordinator;
        _logger = loggerFactory.CreateLogger<OutputCoordinator>();
        _console = System.Console.Out;
    }

    /// <inheritdoc />
    public void SetProgressController(IProgressController controller)
    {
        _progressController = controller;
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
            shouldProcess = !_isFlushing;
            if (shouldProcess)
            {
                _isFlushing = true;
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
                    _isFlushing = false;
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
                    _consoleCoordinator.SetFlushingOutput(true);
                    try
                    {
                        FlushBuffer(pending.Buffer, formatter);
                    }
                    finally
                    {
                        _consoleCoordinator.SetFlushingOutput(false);
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
