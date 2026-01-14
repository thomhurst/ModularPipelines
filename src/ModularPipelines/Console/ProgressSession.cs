using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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
internal class ProgressSession : IProgressSession
{
    private readonly ConsoleCoordinator _coordinator;
    private readonly OrganizedModules _modules;
    private readonly IOptions<PipelineOptions> _options;
    private readonly CancellationToken _cancellationToken;

    private readonly ConcurrentDictionary<IModule, ProgressTask> _moduleTasks = new();
    private readonly ConcurrentDictionary<SubModuleBase, ProgressTask> _subModuleTasks = new();
    private readonly object _progressLock = new();

    private ProgressContext? _progressContext;
    private ProgressTask? _totalTask;
    private readonly TaskCompletionSource _progressCompleted = new();
    private int _totalModuleCount;
    private int _completedModuleCount;

    public ProgressSession(
        ConsoleCoordinator coordinator,
        OrganizedModules modules,
        IOptions<PipelineOptions> options,
        CancellationToken cancellationToken)
    {
        _coordinator = coordinator;
        _modules = modules;
        _options = options;
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Starts the progress display loop.
    /// </summary>
    public void Start()
    {
        _totalModuleCount = _modules.RunnableModules.Count;

        // Fire and forget - progress runs until disposed
        _ = RunProgressLoopAsync();
    }

    private async Task RunProgressLoopAsync()
    {
        try
        {
            await AnsiConsole.Progress()
                .AutoRefresh(true)
                .AutoClear(false)
                .HideCompleted(false)
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new ElapsedTimeColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn())
                .StartAsync(async ctx =>
                {
                    _progressContext = ctx;
                    _totalTask = ctx.AddTask("[green]Total[/]");

                    // Register ignored modules immediately
                    RegisterIgnoredModules(ctx);

                    // Keep alive until all modules complete or cancellation
                    while (!ctx.IsFinished && !_cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(100, CancellationToken.None).ConfigureAwait(false);
                    }
                });
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
            _progressCompleted.TrySetResult();
        }
    }

    private void RegisterIgnoredModules(ProgressContext ctx)
    {
        foreach (var ignored in _modules.IgnoredModules)
        {
            var name = SpectreMarkupEscaper.Escape(ignored.Module.GetType().Name);
            ctx.AddTask($"[yellow][[Ignored]] {name}[/]").StopTask();
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
            var name = SpectreMarkupEscaper.Escape(state.ModuleType.Name);
            var task = _progressContext.AddTask(name, autoStart: true);
            _moduleTasks[state.Module] = task;

            // Start background ticker for progress animation
            StartProgressTicker(task, estimatedDuration);
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

                    var name = SpectreMarkupEscaper.Escape(state.ModuleType.Name);
                    task.Description = isSuccessful
                        ? $"[green]{name}[/]"
                        : $"[red][[Failed]] {name}[/]";

                    task.StopTask();
                }

                // Update total progress
                _completedModuleCount++;
                var increment = 100.0 / _totalModuleCount;
                _totalTask?.Increment(increment);

                // Mark buffer as completed for ordering
                _coordinator.GetModuleBuffer(state.ModuleType).MarkCompleted();

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
            var name = SpectreMarkupEscaper.Escape(state.ModuleType.Name);

            if (_moduleTasks.TryGetValue(state.Module, out var task))
            {
                task.Description = $"[yellow][[Skipped]] {name}[/]";
                if (!task.IsFinished)
                {
                    task.StopTask();
                }
            }
            else
            {
                var newTask = _progressContext.AddTask($"[yellow][[Skipped]] {name}[/]");
                newTask.StopTask();
            }

            _completedModuleCount++;
            var increment = 100.0 / _totalModuleCount;
            _totalTask?.Increment(increment);

            _coordinator.GetModuleBuffer(state.ModuleType).MarkCompleted();

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
                $"  - {subModuleName}",
                new ProgressTaskSettings { AutoStart = true },
                parentTask);

            _subModuleTasks[subModule] = task;

            // Start background ticker for progress animation
            StartProgressTicker(task, estimatedDuration);
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
                        ? $"[green]  - {subModuleName}[/]"
                        : $"[red][[Failed]]   - {subModuleName}[/]";

                    task.StopTask();
                }
            }
        }
    }

    private void StartProgressTicker(ProgressTask task, TimeSpan estimatedDuration)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                // Calculate tick rate based on estimate
                var totalTicks = 95.0; // Go to 95%, completion fills the rest
                var seconds = estimatedDuration.TotalSeconds > 0 ? estimatedDuration.TotalSeconds : 10.0;
                var ticksPerSecond = Math.Clamp(totalTicks / seconds, 0.5, 20.0);

                while (task is { IsFinished: false, Value: < 95 })
                {
                    await Task.Delay(1000, CancellationToken.None).ConfigureAwait(false);

                    lock (_progressLock)
                    {
                        if (!task.IsFinished && task.Value < 95)
                        {
                            task.Increment(ticksPerSecond);
                        }
                    }
                }
            }
            catch
            {
                // Ignore - progress ticking is best-effort
            }
        }, CancellationToken.None);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        // Signal all tasks to stop
        lock (_progressLock)
        {
            _totalTask?.StopTask();

            foreach (var task in _moduleTasks.Values)
            {
                if (!task.IsFinished)
                {
                    task.StopTask();
                }
            }

            foreach (var task in _subModuleTasks.Values)
            {
                if (!task.IsFinished)
                {
                    task.StopTask();
                }
            }
        }

        // Wait for progress loop to finish (with timeout)
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
        await Task.WhenAny(_progressCompleted.Task, timeoutTask).ConfigureAwait(false);

        // NOW it's safe to end the progress phase
        _coordinator.EndProgressPhase();
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
