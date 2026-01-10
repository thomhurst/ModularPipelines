using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Helpers;

/// <summary>
/// Spectre.Console implementation of progress display.
/// Handles all console rendering and Spectre.Console-specific operations.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. Methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// <b>Synchronization Strategy:</b> Uses a lock (<c>_progressLock</c>) to protect
/// Spectre.Console ProgressContext operations which are not thread-safe.
/// ConcurrentDictionary is used for progress task lookups to allow lock-free reads
/// from background tasks that tick progress independently. The lock is only held
/// during ProgressContext mutations (AddTask, StopTask, Increment).
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
[ExcludeFromCodeCoverage]
internal class SpectreProgressDisplay : IProgressDisplay
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly IProgressCalculator _progressCalculator;
    private readonly ConcurrentDictionary<IModule, ProgressTask> _progressTasks = new();
    private readonly ConcurrentDictionary<ModuleState, ProgressTask> _moduleStateProgressTasks = new();
    private readonly ConcurrentDictionary<SubModuleBase, ProgressTask> _subModuleProgressTasks = new();
    private ProgressContext? _progressContext;
    private ProgressTask? _totalProgressTask;
    private int _totalModuleCount;
    private int _completedModuleCount;
    private readonly object _progressLock = new();

    public SpectreProgressDisplay(
        IOptions<PipelineOptions> options,
        IProgressCalculator progressCalculator)
    {
        _options = options;
        _progressCalculator = progressCalculator;
    }

    public async Task RunAsync(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        if (!_options.Value.ShowProgressInConsole)
        {
            return;
        }

        _totalModuleCount = organizedModules.RunnableModules.Count;

        await AnsiConsole.Progress()
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
                     new ElapsedTimeColumn(), new RemainingTimeColumn(), new SpinnerColumn())
            .StartAsync(async progressContext =>
            {
                _progressContext = progressContext;
                _totalProgressTask = progressContext.AddTask("[green]Total[/]");

                // Register ignored modules immediately
                RegisterIgnoredModules(organizedModules.IgnoredModules, progressContext);

                progressContext.Refresh();

                // Keep the progress display alive until all modules complete
                while (!progressContext.IsFinished && !cancellationToken.IsCancellationRequested)
                {
                    progressContext.Refresh();
                    await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    progressContext.Refresh();
                }
            });
    }

    public void OnModuleStarted(ModuleState moduleState, TimeSpan estimatedDuration)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return;
        }

        lock (_progressLock)
        {
            var moduleName = moduleState.ModuleType.Name;
            var progressTask = _progressContext.AddTask(moduleName, new ProgressTaskSettings
            {
                AutoStart = true,
            });

            _progressTasks[moduleState.Module] = progressTask;
            _moduleStateProgressTasks[moduleState] = progressTask;

            // Start ticking progress based on estimated duration
            StartProgressTicker(progressTask, estimatedDuration);
        }
    }

    public void OnModuleCompleted(ModuleState moduleState, bool isSuccessful)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return;
        }

        lock (_progressLock)
        {
            if (_progressTasks.TryGetValue(moduleState.Module, out var progressTask))
            {
                if (!progressTask.IsFinished)
                {
                    if (isSuccessful)
                    {
                        progressTask.Increment(100 - progressTask.Value);
                    }

                    var moduleName = moduleState.ModuleType.Name;
                    progressTask.Description = isSuccessful
                        ? GetSuccessColor(moduleState) + moduleName + "[/]"
                        : $"[red][[Failed]] {moduleName}[/]";

                    progressTask.StopTask();
                }

                _completedModuleCount++;
                _totalProgressTask?.Increment(_progressCalculator.CalculateProgressIncrement(_totalModuleCount));

                if (_completedModuleCount >= _totalModuleCount)
                {
                    _totalProgressTask?.StopTask();
                }
            }
        }
    }

    public void OnModuleSkipped(ModuleState moduleState)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return;
        }

        lock (_progressLock)
        {
            var moduleName = moduleState.ModuleType.Name;

            if (_progressTasks.TryGetValue(moduleState.Module, out var progressTask))
            {
                progressTask.Description = $"[yellow][[Skipped]] {moduleName}[/]";
                if (!progressTask.IsFinished)
                {
                    progressTask.StopTask();
                }
            }
            else
            {
                // Module was skipped before it started
                var task = _progressContext.AddTask($"[yellow][[Skipped]] {moduleName}[/]");
                task.StopTask();
                _progressTasks[moduleState.Module] = task;
            }

            _completedModuleCount++;
            _totalProgressTask?.Increment(_progressCalculator.CalculateProgressIncrement(_totalModuleCount));

            if (_completedModuleCount >= _totalModuleCount)
            {
                _totalProgressTask?.StopTask();
            }
        }
    }

    public void OnSubModuleCreated(IModule parentModule, SubModuleBase subModule, TimeSpan estimatedDuration)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return;
        }

        lock (_progressLock)
        {
            if (!_progressTasks.TryGetValue(parentModule, out var parentTask))
            {
                return;
            }

            var progressTask = _progressContext.AddTaskAfter($"- {subModule.Name}",
                new ProgressTaskSettings { AutoStart = true }, parentTask);

            _subModuleProgressTasks[subModule] = progressTask;

            // Start ticking progress based on estimated duration
            StartProgressTicker(progressTask, estimatedDuration);
        }
    }

    public void OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return;
        }

        lock (_progressLock)
        {
            if (_subModuleProgressTasks.TryGetValue(subModule, out var progressTask))
            {
                if (isSuccessful)
                {
                    progressTask.Increment(100 - progressTask.Value);
                }

                progressTask.Description = isSuccessful
                    ? $"[green]- {subModule.Name}[/]"
                    : $"[red][[Failed]] - {subModule.Name}[/]";

                progressTask.StopTask();
            }
        }
    }

    private void StartProgressTicker(ProgressTask progressTask, TimeSpan estimatedDuration)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var tickInfo = _progressCalculator.CalculateTickInfo(estimatedDuration);

                while (progressTask is { IsFinished: false, Value: < 95 } &&
                       tickInfo.TicksPerSecond + progressTask.Value < 95)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None).ConfigureAwait(false);
                    progressTask.Increment(tickInfo.TicksPerSecond);
                }
            }
            catch (ObjectDisposedException)
            {
                // Expected when progress context is disposed during module completion
            }
            catch (InvalidOperationException)
            {
                // Expected when progress task is stopped or context is in invalid state
            }
            catch (Exception)
            {
                // Suppress other exceptions to prevent unobserved task exceptions
                // Progress updates are non-critical UI feedback
            }
        }, CancellationToken.None);
    }

    private static string GetSuccessColor(ModuleState moduleState)
    {
        // Module state indicates success if State is Completed without issues
        return moduleState.State == ModuleExecutionState.Completed ? "[green]" : "[orange3]";
    }

    private static void RegisterIgnoredModules(IReadOnlyList<IgnoredModule> modulesToIgnore, ProgressContext progressContext)
    {
        foreach (var ignoredModule in modulesToIgnore)
        {
            progressContext.AddTask($"[yellow][[Ignored]] {ignoredModule.Module.GetType().Name}[/]").StopTask();
        }
    }
}
