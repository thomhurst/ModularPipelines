using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.Extensions.Options;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.Helpers;

[ExcludeFromCodeCoverage]
internal class ProgressPrinter : IProgressPrinter,
    INotificationHandler<ModuleStartedNotification>,
    INotificationHandler<ModuleCompletedNotification>,
    INotificationHandler<ModuleSkippedNotification>
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly ConcurrentDictionary<IModule, ProgressTask> _progressTasks = new();
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
                    await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
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
            var moduleName = notification.Module.GetType().Name;
            var progressTask = _progressContext.AddTask(moduleName, new ProgressTaskSettings
            {
                AutoStart = true,
            });

            _progressTasks[notification.Module] = progressTask;

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
                        await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
                        progressTask.Increment(ticksPerSecond);
                    }
                }
                catch
                {
                    // Ignore exceptions in progress updates to prevent unobserved task exceptions
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
            if (_progressTasks.TryGetValue(notification.Module, out var progressTask))
            {
                if (!progressTask.IsFinished)
                {
                    if (notification.IsSuccessful)
                    {
                        progressTask.Increment(100 - progressTask.Value);
                    }

                    var moduleName = notification.Module.GetType().Name;
                    progressTask.Description = notification.IsSuccessful
                        ? GetSuccessColor(notification.Module) + moduleName + "[/]"
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
            var moduleName = notification.Module.GetType().Name;

            if (_progressTasks.TryGetValue(notification.Module, out var progressTask))
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
                _progressTasks[notification.Module] = task;
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
        table.AddColumn(string.Empty);

        // All modules in the summary
        foreach (var module in pipelineSummary.Modules.OrderBy(x => x.ModuleType.Name))
        {
            // For Module<T> instances, we don't have timing information stored on the module itself
            // This will be handled by services in the future
            table.AddRow(
                $"[cyan]{module.ModuleType.Name}[/]",
                "-",
                module.Status.ToDisplayString(),
                "-",
                "-",
                string.Empty);

            table.AddEmptyRow();
        }

        var isSameDayTotal = pipelineSummary.Start.Date == pipelineSummary.End.Date;

        table.AddRow(
            "Total",
            pipelineSummary.TotalDuration.ToDisplayString(),
            pipelineSummary.Status.ToDisplayString(),
            GetTime(pipelineSummary.Start, isSameDayTotal),
            GetTime(pipelineSummary.End, isSameDayTotal),
            "...");

        Console.WriteLine();
        AnsiConsole.Write(table.Expand());
        Console.WriteLine();
    }

    private static string GetSuccessColor(IModule module)
    {
        return module.Status == Status.Successful ? "[green]" : "[orange3]";
    }

    private static void RegisterIgnoredModules(IReadOnlyList<IModule> modulesToIgnore, ProgressContext progressContext)
    {
        foreach (var moduleToIgnore in modulesToIgnore)
        {
            progressContext.AddTask($"[yellow][[Ignored]] {moduleToIgnore.GetType().Name}[/]").StopTask();
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
