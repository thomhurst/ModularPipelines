using ModularPipelines.Models;
using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.Helpers;

internal class PipelineConsoleProgressPrinter : IPipelineConsolePrinter
{
    public void PrintProgress(OrganizedModules organizedModules)
    {
        AnsiConsole.Progress()
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(), 
                new ProgressBarColumn(), 
                new PercentageColumn(), 
                new ElapsedTimeColumn(), 
                new SpinnerColumn(),
            })
            .StartAsync(async ctx =>
            {
                var totalTask = ctx.AddTask($"[green]Total[/]");
                
                RegisterModules(organizedModules.RunnableModules, ctx, totalTask);

                RegisterIgnoredModules(organizedModules.IgnoredModules, ctx);

                CompleteTotalWhenFinished(organizedModules.RunnableModules, totalTask);

                ctx.Refresh();
                
                while (!ctx.IsFinished)
                {
                    await Task.Delay(100);
                }
                
                ctx.Refresh();
            });
    }

    private static void RegisterModules(IReadOnlyList<ModuleBase> modulesToProcess, ProgressContext ctx, ProgressTask totalTask)
    {
        foreach (var moduleToProcess in modulesToProcess)
        {
            var moduleName = moduleToProcess.GetType().Name;
            
            var task = ctx.AddTask($"[[Waiting]] {moduleName}");

            _ = moduleToProcess.Task.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    task.Increment(100);
                }

                task.Description = t.IsCompletedSuccessfully ? $"[green]{moduleName}[/]" : $"[red][[Failed]] {moduleName}[/]";

                task.StopTask();
                totalTask.Increment(100.0 / modulesToProcess.Count);
            });

            _ = moduleToProcess.StartTask.ContinueWith(async t =>
            {
                task.Description = moduleName;
                while (task is { IsFinished: false, Value: < 70 })
                {
                    task.Increment(0.1);
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            });

            _ = moduleToProcess.IgnoreTask.ContinueWith(t =>
            {
                task.Description = $"[yellow][[Ignored]] {moduleName}[/]";
                task.StopTask();
            });
        }
    }

    private static void CompleteTotalWhenFinished(IReadOnlyList<ModuleBase> modulesToProcess, ProgressTask totalTask)
    {
        _ = Task.WhenAll(modulesToProcess.Select(x => x.Task)).ContinueWith(x =>
        {
            totalTask.Increment(100);
            totalTask.StopTask();
        });
    }

    private static void RegisterIgnoredModules(IReadOnlyList<ModuleBase> modulesToIgnore, ProgressContext ctx)
    {
        foreach (var moduleToIgnore in modulesToIgnore)
        {
            ctx.AddTask($"[yellow][[Ignored]] {moduleToIgnore.GetType().Name}[/]").StopTask();
        }
    }
}