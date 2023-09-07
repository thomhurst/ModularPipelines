using Microsoft.Extensions.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.Helpers;

internal class ProgressPrinter : IProgressPrinter
{
    private readonly IOptions<PipelineOptions> _options;

    public ProgressPrinter(IOptions<PipelineOptions> options)
    {
        _options = options;
    }
    
    public Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        if (!_options.Value.ShowProgressInConsole || !AnsiConsole.Profile.Capabilities.Interactive)
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
                    if (cancellationToken.IsCancellationRequested)
                    {
                        progressContext.Refresh();
                        return;
                    }

                    await Task.Delay(1000);
                    progressContext.Refresh();
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
        var table = new Table();
        
        table.AddColumn("Module");
        table.AddColumn("Duration");
        table.AddColumn("Status");
        table.AddColumn("[red]Exception[/]");
        table.AddColumn("Start");
        table.AddColumn("End");

        foreach (var module in pipelineSummary.Modules)
        {
            table.AddRow(
                $"[cyan]{module.GetType().Name}[/]", 
                module.Duration.ToDisplayString(), 
                module.Status.ToDisplayString(),
                $"[red]{module.Exception?.GetType().Name}[/]",
                module.StartTime.ToString(),
                module.EndTime.ToString());
        }

        table.AddEmptyRow();

        table.AddRow(
            "Total",
            pipelineSummary.TotalDuration.ToDisplayString(),
            pipelineSummary.Status.ToDisplayString(),
            string.Empty,
            pipelineSummary.Start.ToString(),
            pipelineSummary.End.ToString());
        
        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        Console.Out.Flush();
    }

    private static void RegisterModules(IReadOnlyList<RunnableModule> modulesToProcess, ProgressContext progressContext,
        ProgressTask totalTask, CancellationToken cancellationToken)
    {
        foreach (var moduleToProcess in modulesToProcess)
        {
            var moduleName = moduleToProcess.Module.GetType().Name;

            var progressTask = progressContext.AddTask($"[[Waiting]] {moduleName}", new ProgressTaskSettings
            {
                AutoStart = false
            });

            // Callback for Module has started
            _ = moduleToProcess.Module.StartTask.ContinueWith(async t =>
            {
                progressTask.StartTask();
                var estimatedDuration = moduleToProcess.EstimatedDuration * 1.1; // Give 10% headroom

                var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1 ? estimatedDuration.TotalSeconds : 1;

                var ticksPerSecond = 100 / totalEstimatedSeconds;

                progressTask.Description = moduleName;
                while (progressTask is { IsFinished: false, Value: < 95 })
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    progressTask.Increment(ticksPerSecond);
                }
            }, cancellationToken);
            
            SetupSkippedCallback(cancellationToken, moduleToProcess, progressTask, moduleName);
            SetupFinishedSuccessfullyCallback(modulesToProcess, totalTask, cancellationToken, moduleToProcess, progressTask, moduleName);
            
            RegisterSubModules(moduleToProcess, progressContext, cancellationToken);
        }
    }

    private static void SetupSkippedCallback(CancellationToken cancellationToken, RunnableModule moduleToProcess,
        ProgressTask progressTask, string moduleName)
    {
        // Callback for Module has been ignored
        _ = moduleToProcess.Module.SkippedTask.ContinueWith(t =>
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
        _ = moduleToProcess.Module.ResultTaskInternal.ContinueWith(t =>
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

    private static void RegisterSubModules(RunnableModule moduleToProcess, ProgressContext progressContext, CancellationToken cancellationToken)
    {
        moduleToProcess.Module.OnSubModuleCreated += (sender, subModule) =>
        {
            var moduleName = moduleToProcess.Module.GetType().Name;

            var progressTask = progressContext.AddTask($"   > {subModule.Name}", new ProgressTaskSettings
            {
                AutoStart = true
            });

            Task.Run(async () =>
            {
                var subModuleEstimation =
                    moduleToProcess.SubModuleEstimations.FirstOrDefault(x => x.SubModuleName == subModule.Name)
                        ?.EstimatedDuration ?? TimeSpan.FromMinutes(2);
                
                var estimatedDuration = subModuleEstimation * 1.1; // Give 10% headroom

                var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1 ? estimatedDuration.TotalSeconds : 1;

                var ticksPerSecond = 100 / totalEstimatedSeconds;

                progressTask.Description = subModule.Name;
                
                while (progressTask is { IsFinished: false, Value: < 95 })
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    progressTask.Increment(ticksPerSecond);
                }
            }, cancellationToken);
            
            // Callback for Module has finished
            _ = subModule.Task.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    progressTask.Increment(100);
                }

                progressTask.Description = t.IsCompletedSuccessfully ? $"[green]    > {subModule.Name}[/]" : $"[red][[Failed]]   > {moduleName} - {subModule.Name}[/]";

                progressTask.StopTask();
            }, cancellationToken);
        };
    }

    private static void CompleteTotalWhenFinished(IReadOnlyList<RunnableModule> modulesToProcess, ProgressTask totalTask, CancellationToken cancellationToken)
    {
        _ = Task.WhenAll(modulesToProcess.Select(x => x.Module.ResultTaskInternal)).ContinueWith(x =>
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
