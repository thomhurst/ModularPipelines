using ModularPipelines.Models;
using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.Helpers;

internal class PipelineConsoleProgressPrinter : IPipelineConsolePrinter
{
    public void PrintProgress( OrganizedModules organizedModules, CancellationToken cancellationToken )
    {
        AnsiConsole.Progress()
            .Columns( new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new ElapsedTimeColumn(), new RemainingTimeColumn(), new SpinnerColumn() )
            .StartAsync( async progressContext =>
            {
                var totalProgressTask = progressContext.AddTask( $"[green]Total[/]" );

                RegisterModules( organizedModules.RunnableModules, progressContext, totalProgressTask, cancellationToken );

                RegisterIgnoredModules( organizedModules.IgnoredModules, progressContext );

                CompleteTotalWhenFinished( organizedModules.RunnableModules, totalProgressTask, cancellationToken );

                progressContext.Refresh();

                while (!progressContext.IsFinished)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await Task.Delay( 1000 );
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                progressContext.Refresh();
            } );
    }

    private static void RegisterModules( IReadOnlyList<RunnableModule> modulesToProcess, ProgressContext progressContext,
        ProgressTask totalTask, CancellationToken cancellationToken )
    {
        foreach (var moduleToProcess in modulesToProcess)
        {
            var moduleName = moduleToProcess.Module.GetType().Name;


            var progressTask = progressContext.AddTask( $"[[Waiting]] {moduleName}", new ProgressTaskSettings
            {
                AutoStart = false
            } );

            // Callback for Module has started
            _ = moduleToProcess.Module.StartTask.ContinueWith( async t =>
            {
                progressTask.StartTask();
                var estimatedDuration = moduleToProcess.EstimatedDuration * 1.1; // Give 10% headroom

                var totalEstimatedSeconds = estimatedDuration.TotalSeconds >= 1 ? estimatedDuration.TotalSeconds : 1;

                var ticksPerSecond = 100 / totalEstimatedSeconds;

                progressTask.Description = moduleName;
                while (progressTask is { IsFinished: false, Value: < 95 })
                {
                    await Task.Delay( TimeSpan.FromSeconds( 1 ) );
                    progressTask.Increment( ticksPerSecond );
                }
            }, cancellationToken );

            // Callback for Module has finished
            _ = moduleToProcess.Module.ResultTaskInternal.ContinueWith( t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    progressTask.Increment( 100 );
                }

                progressTask.Description = t.IsCompletedSuccessfully ? $"[green]{moduleName}[/]" : $"[red][[Failed]] {moduleName}[/]";

                progressTask.StopTask();
                totalTask.Increment( 100.0 / modulesToProcess.Count );
            }, cancellationToken );

            // Callback for Module has been ignored
            _ = moduleToProcess.Module.IgnoreTask.ContinueWith( t =>
            {
                progressTask.Description = $"[yellow][[Ignored]] {moduleName}[/]";
                progressTask.StopTask();
            }, cancellationToken );
        }
    }

    private static void CompleteTotalWhenFinished( IReadOnlyList<RunnableModule> modulesToProcess, ProgressTask totalTask, CancellationToken cancellationToken )
    {
        _ = Task.WhenAll( modulesToProcess.Select( x => x.Module.ResultTaskInternal ) ).ContinueWith( x =>
        {
            totalTask.Increment( 100 );
            totalTask.StopTask();
        }, cancellationToken );
    }

    private static void RegisterIgnoredModules( IReadOnlyList<ModuleBase> modulesToIgnore, ProgressContext progressContext )
    {
        foreach (var moduleToIgnore in modulesToIgnore)
        {
            progressContext.AddTask( $"[yellow][[Ignored]] {moduleToIgnore.GetType().Name}[/]" ).StopTask();
        }
    }
}
