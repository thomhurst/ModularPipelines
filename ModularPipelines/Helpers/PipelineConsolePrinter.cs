using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.Helpers;

public class PipelineConsolePrinter : IPipelineConsolePrinter
{
    public void PrintProgress(List<IModule> modulesToProcess, List<IModule> modulesToIgnore)
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
                
                RegisterModules(modulesToProcess, ctx, totalTask);

                RegisterIgnoredModules(modulesToIgnore, ctx);

                CompleteTotalWhenFinished(modulesToProcess, totalTask, ctx);

                while (!ctx.IsFinished)
                {
                    await Task.Delay(100);
                }
            });
    }

    private static void RegisterModules(List<IModule> modulesToProcess, ProgressContext ctx, ProgressTask totalTask)
    {
        foreach (var moduleToProcess in modulesToProcess)
        {
            var task = ctx.AddTask(moduleToProcess.GetType().Name);

            _ = moduleToProcess.Task.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    task.Increment(100);
                    task.Description = $"[green]{task.Description}[/]";
                }
                else
                {
                    task.Description = $"[red]{task.Description}[/]";
                }

                task.StopTask();
                totalTask.Increment(100.0 / modulesToProcess.Count);
                ctx.Refresh();
            });

            _ = Task.Run(async () =>
            {
                while (task is { IsFinished: false, Value: < 70 })
                {
                    task.Increment(0.1);
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            });
        }
    }

    private static void CompleteTotalWhenFinished(List<IModule> modulesToProcess, ProgressTask totalTask,
        ProgressContext progressContext)
    {
        _ = Task.WhenAll(modulesToProcess.Select(x => x.Task)).ContinueWith(x =>
        {
            totalTask.Increment(100);
            totalTask.StopTask();
            progressContext.Refresh();
        });
    }

    private static void RegisterIgnoredModules(List<IModule> modulesToIgnore, ProgressContext ctx)
    {
        foreach (var moduleToIgnore in modulesToIgnore)
        {
            ctx.AddTask($"[yellow][[Ignored]] {moduleToIgnore.GetType().Name}[/]").StopTask();
        }
    }
}