using ModularPipelines.Models;
using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.Helpers;

internal class PipelineConsoleProgressPrinter : IPipelineConsolePrinter
{
    public void PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        AnsiConsole.Progress()
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new ElapsedTimeColumn(), new SpinnerColumn())
            .StartAsync(async progressContext =>
            {
                var totalTask = progressContext.AddTask($"[green]Total[/]");
                
                RegisterModules(organizedModules.RunnableModules, progressContext, totalTask, cancellationToken);

                RegisterIgnoredModules(organizedModules.IgnoredModules, progressContext);

                CompleteTotalWhenFinished(organizedModules.RunnableModules, totalTask, cancellationToken);

                progressContext.Refresh();
                
                while (!progressContext.IsFinished)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    
                    await Task.Delay(100);
                }
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                
                progressContext.Refresh();
            });
    }

    private static void RegisterModules(IReadOnlyCollection<ModuleBase> modulesToProcess, ProgressContext progressContext,
        ProgressTask totalTask, CancellationToken cancellationToken)
    {
        foreach (var moduleToProcess in modulesToProcess)
        {
            var moduleName = moduleToProcess.GetType().Name;
            
            var progressTask = progressContext.AddTask($"[[Waiting]] {moduleName}");

            _ = moduleToProcess.ResultTaskInternal.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    progressTask.Increment(100);
                }

                progressTask.Description = t.IsCompletedSuccessfully ? $"[green]{moduleName}[/]" : $"[red][[Failed]] {moduleName}[/]";

                progressTask.StopTask();
                totalTask.Increment(100.0 / modulesToProcess.Count);
            }, cancellationToken);

            _ = moduleToProcess.StartTask.ContinueWith(async t =>
            {
                progressTask.Description = moduleName;
                while (progressTask is { IsFinished: false, Value: < 70 })
                {
                    progressTask.Increment(0.1);
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            }, cancellationToken);

            _ = moduleToProcess.IgnoreTask.ContinueWith(t =>
            {
                progressTask.Description = $"[yellow][[Ignored]] {moduleName}[/]";
                progressTask.StopTask();
            }, cancellationToken);
        }
    }

    private static void CompleteTotalWhenFinished(IEnumerable<ModuleBase> modulesToProcess, ProgressTask totalTask, CancellationToken cancellationToken)
    {
        _ = Task.WhenAll(modulesToProcess.Select(x => x.ResultTaskInternal)).ContinueWith(x =>
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