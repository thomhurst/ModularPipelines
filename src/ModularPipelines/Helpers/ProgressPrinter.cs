using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.Helpers;

[ExcludeFromCodeCoverage]
internal class ProgressPrinter : IProgressPrinter
{
    private readonly IOptions<PipelineOptions> _options;

    public ProgressPrinter(IOptions<PipelineOptions> options)
    {
        _options = options;
    }

    public Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        if (!_options.Value.ShowProgressInConsole)
        {
            return Task.CompletedTask;
        }

        return AnsiConsole.Progress()
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new ElapsedTimeColumn(), new RemainingTimeColumn(), new SpinnerColumn())
            .StartAsync(async progressContext =>
            {
                var totalProgressTask = progressContext.AddTask($"[green]Total[/]");

                RegisterModules(organizedModules.RunnableModules, progressContext, totalProgressTask, cancellationToken);

                RegisterIgnoredModules(organizedModules.IgnoredModules, progressContext);

                CompleteTotalWhenFinished(organizedModules.RunnableModules, totalProgressTask, cancellationToken);

                progressContext.Refresh();

                while (!progressContext.IsFinished)
                {
                    progressContext.Refresh();

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    progressContext.Refresh();
                    return;
                }

                progressContext.Refresh();
            });
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

        foreach (var module in pipelineSummary.Modules.OrderBy(x => x.EndTime))
        {
            var isSameDay = module.StartTime.Date == module.EndTime.Date;

            table.AddRow(
                $"[cyan]{module.GetType().Name}[/]",
                module.Duration.ToDisplayString(),
                module.Status.ToDisplayString(),
                GetTime(module.StartTime, isSameDay),
                GetTime(module.EndTime, isSameDay),
                GetModuleExtraInformation(module));

            foreach (var subModule in module.SubModuleBases)
            {
                table.AddRow(
                    $"[lightcyan1]--{subModule.Name}[/]",
                    subModule.Duration.ToDisplayString(),
                    subModule.Status.ToDisplayString(),
                    GetTime(subModule.StartTime, isSameDay),
                    GetTime(subModule.EndTime, isSameDay),
                    string.Empty);
            }

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
        AnsiConsole.Write(table);
        Console.WriteLine();
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

    private static string GetModuleExtraInformation(ModuleBase module)
    {
        if (module.SkipResult.ShouldSkip && !string.IsNullOrWhiteSpace(module.SkipResult.Reason))
        {
            return $"[yellow]{module.SkipResult.Reason}[/]";
        }

        if (module.Exception != null)
        {
            return $"[red]{module.Exception?.GetType().Name}[/]";
        }

        return string.Empty;
    }

    private static void RegisterModules(IReadOnlyList<RunnableModule> modulesToProcess, ProgressContext progressContext,
        ProgressTask totalTask, CancellationToken cancellationToken)
    {
        foreach (var moduleToProcess in modulesToProcess)
        {
            var moduleName = moduleToProcess.Module.GetType().Name;

            var progressTask = progressContext.AddTask($"[[Waiting]] {moduleName}", new ProgressTaskSettings
            {
                AutoStart = false,
            });

            // Callback for Module has started
            _ = moduleToProcess.Module.StartTask.ContinueWith(async t =>
            {
                progressTask.StartTask();
                var estimatedDuration = moduleToProcess.EstimatedDuration * 1.1; // Give 10% headroom

                var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1.0 ? estimatedDuration.TotalSeconds : 1.0;

                var ticksPerSecond = 100.0 / totalEstimatedSeconds;

                progressTask.Description = moduleName;
                while (progressTask is { IsFinished: false, Value: < 95 } && ticksPerSecond + progressTask.Value < 95)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
                    progressTask.Increment(ticksPerSecond);
                }
            }, cancellationToken);

            SetupSkippedCallback(cancellationToken, moduleToProcess, progressTask, moduleName);
            SetupFinishedSuccessfullyCallback(modulesToProcess, totalTask, cancellationToken, moduleToProcess, progressTask, moduleName);

            RegisterSubModules(moduleToProcess, progressContext, cancellationToken, progressTask);
        }
    }

    private static void SetupSkippedCallback(CancellationToken cancellationToken, RunnableModule moduleToProcess,
        ProgressTask progressTask, string moduleName)
    {
        // Callback for Module has been ignored
        _ = moduleToProcess.Module.SkipHandler.CallbackTask.ContinueWith(t =>
        {
            lock (moduleToProcess)
            {
                progressTask.Description = $"[yellow][[Skipped]] {moduleName}[/]";
                progressTask.StopTask();
            }
        }, cancellationToken);
    }

    private static void SetupFinishedSuccessfullyCallback(IReadOnlyList<RunnableModule> modulesToProcess, ProgressTask totalTask,
        CancellationToken cancellationToken, RunnableModule moduleToProcess, ProgressTask progressTask, string moduleName)
    {
        // Callback for Module has finished
        _ = moduleToProcess.Module.ExecutionTask.ContinueWith(t =>
        {
            lock (moduleToProcess)
            {
                if (progressTask.IsFinished)
                {
                    return;
                }

                if (t.IsCompletedSuccessfully)
                {
                    progressTask.Increment(100);
                }

                progressTask.Description =
                    t.IsCompletedSuccessfully ? $"{GetColour()}{moduleName}[/]" : $"[red][[Failed]] {moduleName}[/]";

                progressTask.StopTask();

                totalTask.Increment(100.0 / modulesToProcess.Count);
            }
        }, cancellationToken);

        string GetColour()
        {
            return moduleToProcess.Module.Status == Status.Successful ? "[green]" : "[orange3]";
        }
    }

    private static void RegisterSubModules(RunnableModule moduleToProcess, ProgressContext progressContext,
        CancellationToken cancellationToken, ProgressTask parentModuleTask)
    {
        var lastTask = parentModuleTask;
        
        moduleToProcess.Module.OnSubModuleCreated += (_, subModule) =>
        {
            var moduleName = moduleToProcess.Module.GetType().Name;

            var progressTask = lastTask = progressContext.AddTaskAfter($"- {subModule.Name}", new ProgressTaskSettings
            {
                AutoStart = true,
            }, lastTask);

            Task.Run(async () =>
            {
                var subModuleEstimation =
                    moduleToProcess.SubModuleEstimations.FirstOrDefault(x => x.SubModuleName == subModule.Name)
                        ?.EstimatedDuration ?? TimeSpan.FromMinutes(2);

                var estimatedDuration = subModuleEstimation * 1.1; // Give 10% headroom

                var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1 ? estimatedDuration.TotalSeconds : 1;

                var ticksPerSecond = 100 / totalEstimatedSeconds;

                while (progressTask is { IsFinished: false, Value: < 95 })
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
                    progressTask.Increment(ticksPerSecond);
                }
            }, cancellationToken);

            // Callback for Module has finished
            subModule.CallbackTask.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    progressTask.Increment(100);
                }

                progressTask.Description = t.IsCompletedSuccessfully ? $"[green]- {subModule.Name}[/]" : $"[red][[Failed]]   > {moduleName} - {subModule.Name}[/]";

                progressTask.StopTask();
            }, cancellationToken);
        };
    }

    private static void CompleteTotalWhenFinished(IReadOnlyList<RunnableModule> modulesToProcess, ProgressTask totalTask, CancellationToken cancellationToken)
    {
        _ = Task.WhenAll(modulesToProcess.Select(x => x.Module.ExecutionTask)).ContinueWith(x =>
        {
            totalTask.Increment(100);
            totalTask.StopTask();
        }, cancellationToken);
    }

    private static void RegisterIgnoredModules(IReadOnlyList<ModuleBase> modulesToIgnore, ProgressContext progressContext)
    {
        foreach (var moduleToIgnore in modulesToIgnore)
        {
            progressContext.AddTask($"[yellow][[Ignored]] {moduleToIgnore.GetType().Name}[/]").StopTask();
        }
    }
}