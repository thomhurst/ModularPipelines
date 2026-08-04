using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Console;

/// <summary>
/// Manages an active progress display session using Spectre.Console.
/// </summary>
/// <remarks>
/// <para>
/// <b>Lifecycle:</b> The session is active from Start() until DisposeAsync().
/// During this time, the progress display renders and module status updates
/// are reflected in the progress bars.
/// </para>
/// <para>
/// <b>Critical:</b> DisposeAsync() MUST be called before flushing module output.
/// It ensures the progress display has fully stopped before any other console output.
/// </para>
/// <para>
/// <b>Thread Safety:</b> All methods are thread-safe and can be called concurrently.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal class ProgressSession : IProgressSession, IProgressController
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromMilliseconds(100);

    private readonly ConsoleCoordinator _coordinator;
    private readonly OrganizedModules _modules;
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger _logger;
    private readonly IAnsiConsole _ansiConsole;
    private readonly CancellationTokenSource _progressLoopCancellationTokenSource;
    private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly ConcurrentDictionary<IModule, ProgressTask> _moduleTasks = new();
    private readonly ConcurrentDictionary<SubModuleBase, ProgressTask> _subModuleTasks = new();
    private readonly Dictionary<ProgressTask, ProgressTicker> _progressTickers = [];
    private readonly object _progressLock = new();

    private ProgressContext? _progressContext;
    private ProgressTask? _totalTask;
    private Task? _progressLoopTask;
    private Task? _disposeTask;
    private int _totalModuleCount;
    private int _completedModuleCount;

    // Pause coordination - uses signals instead of locks held during slow operations
    private readonly object _pauseStateLock = new();
    private bool _isPaused;
    private bool _inRefresh; // True while ctx.Refresh() is executing
    private TaskCompletionSource? _resumeSignal;
    private TaskCompletionSource? _refreshCompleted; // Signaled when in-flight refresh completes

    public ProgressSession(
        ConsoleCoordinator coordinator,
        OrganizedModules modules,
        IOptions<PipelineOptions> options,
        ILoggerFactory loggerFactory,
        IAnsiConsole ansiConsole,
        CancellationToken cancellationToken)
    {
        _coordinator = coordinator;
        _modules = modules;
        _options = options;
        _logger = loggerFactory.CreateLogger<ProgressSession>();
        _ansiConsole = ansiConsole;
        _progressLoopCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    }

    /// <summary>
    /// Starts the progress display loop.
    /// </summary>
    public Task StartAsync()
    {
        lock (_progressLock)
        {
            if (_disposeTask is not null)
            {
                return Task.CompletedTask;
            }

            if (_progressLoopTask is null)
            {
                _totalModuleCount = _modules.RunnableModules.Count;
                _progressLoopTask = RunProgressLoopAsync();
            }

            return _started.Task;
        }
    }

    private async Task RunProgressLoopAsync()
    {
        var cancellationToken = _progressLoopCancellationTokenSource.Token;
        try
        {
            await _ansiConsole.Progress()
                .AutoRefresh(false) // Disable auto-refresh so we can pause rendering during output writes
                .AutoClear(false)
                .HideCompleted(false)
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new ElapsedTimeColumn(),
                    new SpinnerColumn(ProgressDisplayStyle.Spinner)
                    {
                        CompletedText = ProgressDisplayStyle.CompletedSpinnerText,
                        PendingText = ProgressDisplayStyle.PendingMarker,
                    })
                .StartAsync(async ctx =>
                {
                    _progressContext = ctx;

                    // Total row with bold styling to stand out
                    _totalTask = ctx.AddTask("[bold green]Pipeline[/]");

                    // Register ignored modules immediately
                    RegisterIgnoredModules(ctx);

                    // Register all pending modules upfront so users can see what's coming
                    RegisterPendingModules(ctx);

                    ctx.Refresh();
                    _started.TrySetResult();

                    // Keep alive until all modules complete or cancellation
                    while (!ctx.IsFinished)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        // Check pause state and prepare for refresh
                        bool shouldRefresh;
                        TaskCompletionSource? resumeSignal = null;

                        lock (_pauseStateLock)
                        {
                            if (_isPaused)
                            {
                                // We're paused - signal that any in-flight refresh is done
                                _refreshCompleted?.TrySetResult();
                                resumeSignal = _resumeSignal;
                                shouldRefresh = false;
                            }
                            else
                            {
                                // Mark that we're about to refresh
                                _inRefresh = true;
                                shouldRefresh = true;
                            }
                        }

                        if (resumeSignal != null)
                        {
                            // Wait for resume signal with timeout to prevent stuck UI
                            try
                            {
                                await resumeSignal.Task
                                    .WaitAsync(TimeSpan.FromSeconds(60), cancellationToken)
                                    .ConfigureAwait(false);
                            }
                            catch (TimeoutException)
                            {
                                _logger.LogWarning(
                                    "Progress pause timed out after 60 seconds. Forcing resume to prevent stuck UI.");

                                lock (_pauseStateLock)
                                {
                                    _isPaused = false;
                                    _inRefresh = false; // Reset for state consistency
                                    _resumeSignal?.TrySetResult();
                                    _resumeSignal = null;
                                    _refreshCompleted = null;
                                }
                            }
                            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                        }
                        else if (shouldRefresh)
                        {
                            try
                            {
                                UpdateProgressTickers();
                                ctx.Refresh();
                            }
                            finally
                            {
                                // Mark refresh complete and check if pause was requested during refresh
                                lock (_pauseStateLock)
                                {
                                    _inRefresh = false;
                                    if (_isPaused)
                                    {
                                        // Pause was requested while we were refreshing - signal completion
                                        _refreshCompleted?.TrySetResult();
                                    }
                                }
                            }
                        }

                        try
                        {
                            await Task.Delay(RefreshInterval, cancellationToken).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                    }

                    // Render state changes made immediately before disposal. The periodic
                    // loop may be cancelled before its next scheduled refresh.
                    UpdateProgressTickers();
                    ctx.Refresh();
                })
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected on cancellation
        }
        catch (Exception)
        {
            // Suppress exceptions from progress display
        }
        finally
        {
            _started.TrySetResult();
        }
    }

    private void RegisterIgnoredModules(ProgressContext ctx)
    {
        foreach (var ignored in _modules.IgnoredModules)
        {
            var name = FormatModuleName(ignored.Module.GetType().Name);
            ctx.AddTask(ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Skipped)).StopTask();
        }
    }

    private void RegisterPendingModules(ProgressContext ctx)
    {
        lock (_progressLock)
        {
            foreach (var runnableModule in _modules.RunnableModules)
            {
                var name = FormatModuleName(runnableModule.Module.GetType().Name);

                // Create task in paused state with dim waiting status
                // autoStart: false means task is paused, NOT stopped (stopped tasks can't restart)
                var task = ctx.AddTask(
                    ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Pending),
                    autoStart: false);

                // Store for lookup when module actually starts
                _moduleTasks[runnableModule.Module] = task;
            }
        }
    }

    /// <inheritdoc />
    public void OnModuleStarted(ModuleState state, TimeSpan estimatedDuration)
    {
        if (_progressContext == null)
        {
            return;
        }

        lock (_progressLock)
        {
            var name = FormatModuleName(state.ModuleType.Name);

            // Check if module was pre-registered (normal case)
            if (_moduleTasks.TryGetValue(state.Module, out var task))
            {
                // Check if task was already stopped (e.g., if progress session was disposed
                // but module is still running - shouldn't happen with proper lifecycle management)
                if (task.IsFinished)
                {
                    // Task already stopped - nothing we can do, module runs without progress display
                    return;
                }

                // Update to running status with cyan color
                task.Description = ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Running);
                task.Value = 0;
                task.StartTask();
            }
            else
            {
                // Fallback: create new task if not pre-registered
                task = _progressContext.AddTask(
                    ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Running),
                    autoStart: true);
                _moduleTasks[state.Module] = task;
            }

            RegisterProgressTicker(task, estimatedDuration);
        }
    }

    /// <inheritdoc />
    public void OnModuleCompleted(ModuleState state, bool isSuccessful)
    {
        if (_progressContext == null)
        {
            return;
        }

        lock (_progressLock)
        {
            if (_moduleTasks.TryGetValue(state.Module, out var task))
            {
                // Complete the progress bar
                if (!task.IsFinished)
                {
                    task.Increment(100 - task.Value);

                    var name = FormatModuleName(state.ModuleType.Name);
                    task.Description = isSuccessful
                        ? ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Succeeded)
                        : ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Failed);

                    task.StopTask();
                }

                _progressTickers.Remove(task);

                // Update total progress
                _completedModuleCount++;
                var increment = 100.0 / _totalModuleCount;
                _totalTask?.Increment(increment);

                // Check if all done
                if (_completedModuleCount >= _totalModuleCount)
                {
                    _totalTask?.StopTask();
                }
            }
        }
    }

    /// <inheritdoc />
    public void OnModuleSkipped(ModuleState state)
    {
        if (_progressContext == null)
        {
            return;
        }

        lock (_progressLock)
        {
            var name = FormatModuleName(state.ModuleType.Name);

            if (_moduleTasks.TryGetValue(state.Module, out var task))
            {
                task.Description = ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Skipped);
                if (!task.IsFinished)
                {
                    task.StopTask();
                }

                _progressTickers.Remove(task);
            }
            else
            {
                var newTask = _progressContext.AddTask(
                    ProgressDisplayStyle.FormatTask(name, ProgressTaskStatus.Skipped));
                newTask.StopTask();
            }

            _completedModuleCount++;
            var increment = 100.0 / _totalModuleCount;
            _totalTask?.Increment(increment);

            if (_completedModuleCount >= _totalModuleCount)
            {
                _totalTask?.StopTask();
            }
        }
    }

    /// <inheritdoc />
    public void OnSubModuleCreated(IModule parentModule, SubModuleBase subModule, TimeSpan estimatedDuration)
    {
        if (_progressContext == null)
        {
            return;
        }

        lock (_progressLock)
        {
            if (!_moduleTasks.TryGetValue(parentModule, out var parentTask))
            {
                return;
            }

            var subModuleName = SpectreMarkupEscaper.Escape(subModule.Name);
            var task = _progressContext.AddTaskAfter(
                ProgressDisplayStyle.FormatTask(
                    subModuleName,
                    ProgressTaskStatus.Running,
                    isSubModule: true),
                new ProgressTaskSettings { AutoStart = true },
                parentTask);

            _subModuleTasks[subModule] = task;

            RegisterProgressTicker(task, estimatedDuration);
        }
    }

    /// <inheritdoc />
    public void OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful)
    {
        if (_progressContext == null)
        {
            return;
        }

        lock (_progressLock)
        {
            if (_subModuleTasks.TryGetValue(subModule, out var task))
            {
                if (!task.IsFinished)
                {
                    if (isSuccessful)
                    {
                        task.Increment(100 - task.Value);
                    }

                    var subModuleName = SpectreMarkupEscaper.Escape(subModule.Name);
                    task.Description = isSuccessful
                        ? ProgressDisplayStyle.FormatTask(
                            subModuleName,
                            ProgressTaskStatus.Succeeded,
                            isSubModule: true)
                        : ProgressDisplayStyle.FormatTask(
                            subModuleName,
                            ProgressTaskStatus.Failed,
                            isSubModule: true);

                    task.StopTask();
                }

                _progressTickers.Remove(task);
            }
        }
    }

    private void RegisterProgressTicker(ProgressTask task, TimeSpan estimatedDuration)
    {
        var totalTicks = 95.0;
        var seconds = estimatedDuration.TotalSeconds > 0 ? estimatedDuration.TotalSeconds : 10.0;
        var ticksPerSecond = Math.Clamp(totalTicks / seconds, 0.5, 20.0);
        _progressTickers[task] = new ProgressTicker(ticksPerSecond);
    }

    private void UpdateProgressTickers()
    {
        lock (_progressLock)
        {
            List<ProgressTask>? finishedTasks = null;
            var timestamp = Stopwatch.GetTimestamp();

            foreach (var (task, ticker) in _progressTickers)
            {
                if (task.IsFinished || task.Value >= 95)
                {
                    finishedTasks ??= [];
                    finishedTasks.Add(task);
                    continue;
                }

                var elapsed = Stopwatch.GetElapsedTime(ticker.LastUpdatedTimestamp, timestamp);
                task.Increment(Math.Min(ticker.TicksPerSecond * elapsed.TotalSeconds, 95 - task.Value));
                ticker.LastUpdatedTimestamp = timestamp;
            }

            if (finishedTasks is null)
            {
                return;
            }

            foreach (var task in finishedTasks)
            {
                _progressTickers.Remove(task);
            }
        }
    }

    #region IProgressController Implementation

    /// <inheritdoc />
    public bool IsInteractive => true;

    /// <inheritdoc />
    public Task PauseAsync()
    {
        lock (_pauseStateLock)
        {
            if (_isPaused)
            {
                // The first caller already waits for any active refresh. Reusing its
                // completion source here could wait forever if that source is stale.
                return Task.CompletedTask;
            }

            _isPaused = true;
            _resumeSignal = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            if (_inRefresh)
            {
                _refreshCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                return _refreshCompleted.Task;
            }

            return Task.CompletedTask;
        }
    }

    /// <inheritdoc />
    public Task ResumeAsync()
    {
        lock (_pauseStateLock)
        {
            _isPaused = false;
            _resumeSignal?.TrySetResult();
            _resumeSignal = null;
            _refreshCompleted = null;
        }

        return Task.CompletedTask;
    }

    #endregion

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        lock (_progressLock)
        {
            _disposeTask ??= DisposeCoreAsync();
            return new ValueTask(_disposeTask);
        }
    }

    private async Task DisposeCoreAsync()
    {
        TaskCompletionSource? refreshCompletion;

        // Signal resume in case we're disposed while paused
        // Also clear the signals to prevent any concurrent PauseAsync from hanging
        lock (_pauseStateLock)
        {
            _isPaused = false;
            _resumeSignal?.TrySetResult();
            _resumeSignal = null;
            refreshCompletion = _refreshCompleted;
            _refreshCompleted = null;
        }

        // Signal tasks to stop, but preserve AlwaysRun module tasks
        // AlwaysRun modules may still execute after pipeline failure, so we need to keep
        // their progress tasks available for updates
        lock (_progressLock)
        {
            _totalTask?.StopTask();

            foreach (var (module, task) in _moduleTasks)
            {
                if (!task.IsFinished)
                {
                    // Don't stop AlwaysRun module tasks - they may still need to run
                    if (!module.Configuration.AlwaysRun)
                    {
                        task.StopTask();
                    }
                }
            }

            foreach (var task in _subModuleTasks.Values)
            {
                if (!task.IsFinished)
                {
                    task.StopTask();
                }
            }

            _progressTickers.Clear();
        }

        // Cancel only after terminal task state is recorded so the progress loop's
        // final refresh renders those changes before its live callback exits.
        _progressLoopCancellationTokenSource.Cancel();

        if (_progressLoopTask is not null)
        {
            try
            {
                await _progressLoopTask
                    .WaitAsync(TimeSpan.FromSeconds(5))
                    .ConfigureAwait(false);
            }
            catch (TimeoutException)
            {
                _logger.LogWarning("Progress display did not stop within 5 seconds.");
            }
        }

        refreshCompletion?.TrySetResult();
        _progressLoopCancellationTokenSource.Dispose();

        // NOW it's safe to end the progress phase
        _coordinator.EndProgressPhase();
    }

    /// <summary>
    /// Formats a module name for display by stripping the "Module" suffix.
    /// </summary>
    private static string FormatModuleName(string typeName)
    {
        var escaped = SpectreMarkupEscaper.Escape(typeName);
        return escaped.EndsWith("Module", StringComparison.Ordinal)
            ? escaped[..^6]
            : escaped;
    }

    private sealed class ProgressTicker(double ticksPerSecond)
    {
        public double TicksPerSecond { get; } = ticksPerSecond;

        public long LastUpdatedTimestamp { get; set; } = Stopwatch.GetTimestamp();
    }
}

/// <summary>
/// No-op progress session when progress display is disabled.
/// </summary>
[ExcludeFromCodeCoverage]
internal class NoOpProgressSession : IProgressSession
{
    public void OnModuleStarted(ModuleState state, TimeSpan estimatedDuration)
    {
    }

    public void OnModuleCompleted(ModuleState state, bool isSuccessful)
    {
    }

    public void OnModuleSkipped(ModuleState state)
    {
    }

    public void OnSubModuleCreated(IModule parentModule, SubModuleBase subModule, TimeSpan estimatedDuration)
    {
    }

    public void OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful)
    {
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
