using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.Helpers;

/// <summary>
/// Displays real-time progress of module execution using Spectre.Console.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. Notification handlers can be called
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
internal class ProgressPrinter : IProgressPrinter,
    INotificationHandler<ModuleStartedNotification>,
    INotificationHandler<ModuleCompletedNotification>,
    INotificationHandler<ModuleSkippedNotification>,
    INotificationHandler<SubModuleCreatedNotification>,
    INotificationHandler<SubModuleCompletedNotification>
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly ConcurrentDictionary<IModule, ProgressTask> _progressTasks = new();
    private readonly ConcurrentDictionary<ModuleState, ProgressTask> _moduleStateProgressTasks = new();
    private readonly ConcurrentDictionary<SubModuleBase, ProgressTask> _subModuleProgressTasks = new();
    private ProgressContext? _progressContext;
    private ProgressTask? _totalProgressTask;
    private int _totalModuleCount;
    private int _completedModuleCount;
    private readonly object _progressLock = new();

    public ProgressPrinter(IOptions<PipelineOptions> options)
    {
        _options = options;
    }

    public async Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken)
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
                _totalProgressTask = progressContext.AddTask($"[green]Total[/]");

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

    public ValueTask Handle(ModuleStartedNotification notification, CancellationToken cancellationToken)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return ValueTask.CompletedTask;
        }

        lock (_progressLock)
        {
            var moduleState = notification.ModuleState;
            var moduleName = moduleState.ModuleType.Name;
            var progressTask = _progressContext.AddTask(moduleName, new ProgressTaskSettings
            {
                AutoStart = true,
            });

            _progressTasks[moduleState.Module] = progressTask;
            _moduleStateProgressTasks[moduleState] = progressTask;

            // Start ticking progress based on estimated duration
            _ = Task.Run(async () =>
            {
                try
                {
                    var estimatedDuration = notification.EstimatedDuration * 1.1; // Give 10% headroom
                    var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1.0 ? estimatedDuration.TotalSeconds : 1.0;
                    var ticksPerSecond = 100.0 / totalEstimatedSeconds;

                    while (progressTask is { IsFinished: false, Value: < 95 } && ticksPerSecond + progressTask.Value < 95)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None).ConfigureAwait(false);
                        progressTask.Increment(ticksPerSecond);
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

        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(ModuleCompletedNotification notification, CancellationToken cancellationToken)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return ValueTask.CompletedTask;
        }

        lock (_progressLock)
        {
            var moduleState = notification.ModuleState;
            if (_progressTasks.TryGetValue(moduleState.Module, out var progressTask))
            {
                if (!progressTask.IsFinished)
                {
                    if (notification.IsSuccessful)
                    {
                        progressTask.Increment(100 - progressTask.Value);
                    }

                    var moduleName = moduleState.ModuleType.Name;
                    progressTask.Description = notification.IsSuccessful
                        ? GetSuccessColor(moduleState) + moduleName + "[/]"
                        : $"[red][[Failed]] {moduleName}[/]";

                    progressTask.StopTask();
                }

                _completedModuleCount++;
                _totalProgressTask?.Increment(100.0 / _totalModuleCount);

                if (_completedModuleCount >= _totalModuleCount)
                {
                    _totalProgressTask?.StopTask();
                }
            }
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(ModuleSkippedNotification notification, CancellationToken cancellationToken)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return ValueTask.CompletedTask;
        }

        lock (_progressLock)
        {
            var moduleState = notification.ModuleState;
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
            _totalProgressTask?.Increment(100.0 / _totalModuleCount);

            if (_completedModuleCount >= _totalModuleCount)
            {
                _totalProgressTask?.StopTask();
            }
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(SubModuleCreatedNotification notification, CancellationToken cancellationToken)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return ValueTask.CompletedTask;
        }

        lock (_progressLock)
        {
            if (!_progressTasks.TryGetValue(notification.ParentModule, out var parentTask))
            {
                return ValueTask.CompletedTask;
            }

            var progressTask = _progressContext.AddTaskAfter($"- {notification.SubModule.Name}",
                new ProgressTaskSettings { AutoStart = true }, parentTask);

            _subModuleProgressTasks[notification.SubModule] = progressTask;

            // Start ticking progress based on estimated duration
            _ = Task.Run(async () =>
            {
                try
                {
                    var estimatedDuration = notification.EstimatedDuration * 1.1; // Give 10% headroom
                    var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1 ? estimatedDuration.TotalSeconds : 1;
                    var ticksPerSecond = 100 / totalEstimatedSeconds;

                    while (progressTask is { IsFinished: false, Value: < 95 })
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None).ConfigureAwait(false);
                        progressTask.Increment(ticksPerSecond);
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Expected when progress context is disposed during submodule completion
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

        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(SubModuleCompletedNotification notification, CancellationToken cancellationToken)
    {
        if (_progressContext == null || !_options.Value.ShowProgressInConsole)
        {
            return ValueTask.CompletedTask;
        }

        lock (_progressLock)
        {
            if (_subModuleProgressTasks.TryGetValue(notification.SubModule, out var progressTask))
            {
                if (notification.IsSuccessful)
                {
                    progressTask.Increment(100 - progressTask.Value);
                }

                progressTask.Description = notification.IsSuccessful
                    ? $"[green]- {notification.SubModule.Name}[/]"
                    : $"[red][[Failed]] - {notification.SubModule.Name}[/]";

                progressTask.StopTask();
            }
        }

        return ValueTask.CompletedTask;
    }

    public void PrintResults(PipelineSummary pipelineSummary)
    {
        if (!_options.Value.PrintResults)
        {
            return;
        }

        var table = new Table
        {
            Expand = true,
        };

        table.AddColumn("Module");
        table.AddColumn("Duration");
        table.AddColumn("Status");
        table.AddColumn("Start");
        table.AddColumn("End");

        // Create a lookup for module timelines by module name
        var timelineLookup = pipelineSummary.ModuleTimelines?
            .ToDictionary(t => t.ModuleName, t => t)
            ?? new Dictionary<string, ModuleTimeline>();

        foreach (var module in pipelineSummary.Modules)
        {
            var moduleName = module.GetType().Name;
            var hasTimeline = timelineLookup.TryGetValue(moduleName, out var timeline);

            var duration = hasTimeline && timeline!.ExecutionDuration.HasValue
                ? timeline.ExecutionDuration.Value.ToDisplayString()
                : "-";

            var status = hasTimeline
                ? timeline!.Status.ToDisplayString()
                : "-";

            var isSameDay = hasTimeline
                && timeline!.StartTime.HasValue
                && timeline.EndTime.HasValue
                && timeline.StartTime.Value.Date == timeline.EndTime.Value.Date;

            var start = hasTimeline && timeline!.StartTime.HasValue
                ? GetTime(timeline.StartTime.Value, isSameDay)
                : "-";

            var end = hasTimeline && timeline!.EndTime.HasValue
                ? GetTime(timeline.EndTime.Value, isSameDay)
                : "-";

            table.AddRow(
                $"[cyan]{moduleName}[/]",
                duration,
                status,
                start,
                end);

            table.AddEmptyRow();
        }

        var isSameDayTotal = pipelineSummary.Start.Date == pipelineSummary.End.Date;

        table.AddRow(
            "Total",
            pipelineSummary.TotalDuration.ToDisplayString(),
            pipelineSummary.Status.ToDisplayString(),
            GetTime(pipelineSummary.Start, isSameDayTotal),
            GetTime(pipelineSummary.End, isSameDayTotal));

        Console.WriteLine();
        AnsiConsole.Write(table.Expand());

        // Print execution metrics if available
        PrintMetrics(pipelineSummary);

        Console.WriteLine();
    }

    private static void PrintMetrics(PipelineSummary pipelineSummary)
    {
        var metrics = pipelineSummary.Metrics;
        if (metrics == null)
        {
            return;
        }

        Console.WriteLine();
        AnsiConsole.MarkupLine("[bold underline]Execution Metrics[/]");

        var metricsTable = new Table
        {
            Border = TableBorder.Rounded,
            ShowHeaders = false,
        };

        metricsTable.AddColumn("Metric");
        metricsTable.AddColumn("Value");

        metricsTable.AddRow("[cyan]Parallelism Factor[/]", $"[bold]{metrics.ParallelismFactor:F2}x[/]");
        metricsTable.AddRow("[cyan]Peak Concurrency[/]", $"[bold]{metrics.PeakConcurrency}[/] modules");
        metricsTable.AddRow("[cyan]Avg Concurrency[/]", $"[bold]{metrics.AverageConcurrency:F2}[/] modules");
        metricsTable.AddRow("[cyan]Efficiency[/]", $"[bold]{metrics.Efficiency * 100:F0}%[/]");
        metricsTable.AddRow("[cyan]Sequential Time[/]", $"[dim]{metrics.TotalModuleExecutionTime.ToDisplayString()}[/]");
        metricsTable.AddRow("[cyan]Wall-Clock Time[/]", $"[dim]{metrics.WallClockDuration.ToDisplayString()}[/]");

        AnsiConsole.Write(metricsTable);
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

    private static string GetTime(DateTimeOffset dateTimeOffset, bool isSameDay)
    {
        if (dateTimeOffset == DateTimeOffset.MinValue)
        {
            return string.Empty;
        }

        return isSameDay
            ? dateTimeOffset.ToTimeOnly().ToString("h:mm:ss tt")
            : dateTimeOffset.ToString("yyyy/MM/dd h:mm:ss tt");
    }
}