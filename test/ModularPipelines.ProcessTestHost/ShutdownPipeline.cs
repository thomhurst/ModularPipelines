using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.ProcessTestHost;

internal static class ShutdownPipeline
{
    public static async Task<int> RunAsync(string mode, string directory)
    {
        Host.RecordStartupStage(directory, "shutdown-entered");
        var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings { WorkingDirectory = directory });
        builder.Services.AddSingleton(new ShutdownState(directory, mode));
        builder.Services.AddSingleton<DisposalMarker>();
        builder.AddModule<WaitingModule>()
            .AddModule<CleanupModule>()
            .AddPipelineEventHandler<EndHandler>()
            .ConfigureOptions(options => options with
            {
                ThrowOnPipelineFailure = false,
                Console = options.Console with { ShowProgress = false, PrintLogo = false, PrintResults = false },
                RunReport = options.RunReport with
                {
                    ReportPath = Path.Combine(directory, "report.json"),
                    AutoWriteInCi = false,
                    HistoryRetention = 0,
                },
            });

        await using var pipeline = await builder.BuildAsync();
        Host.RecordStartupStage(directory, "pipeline-built");
        // Activate the container-owned marker before shutdown begins.
        _ = pipeline.Services.GetRequiredService<DisposalMarker>();
        if (mode == "pipeline-sigterm-before-run")
        {
            await File.WriteAllTextAsync(Path.Combine(directory, "ready"), string.Empty);
            await Task.Delay(Timeout.Infinite);
        }

        if (mode.StartsWith("pipeline-exit", StringComparison.Ordinal))
        {
            _ = Task.Run(async () =>
            {
                while (!File.Exists(Path.Combine(directory, "exit.trigger")))
                {
                    await Task.Delay(10);
                }

                Environment.Exit(0);
            });
        }

        try
        {
            Host.RecordStartupStage(directory, "pipeline-run-starting");
            await pipeline.RunAsync();
        }
        catch (OperationCanceledException)
        {
            // Cancellation is the expected outcome of this fixture.
        }

        if (mode == "pipeline-sigterm-after-run")
        {
            await File.WriteAllTextAsync(Path.Combine(directory, "ready"), string.Empty);
            await Task.Delay(Timeout.Infinite);
        }

        return 0;
    }

    internal sealed record ShutdownState(string Directory, string Mode)
    {
        public void Record(string name) => File.AppendAllText(Path.Combine(Directory, "events.log"), name + Environment.NewLine);
    }

    internal sealed class DisposalMarker(ShutdownState state) : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            state.Record(File.Exists(Path.Combine(state.Directory, "report.json")) ? "disposed-after-report" : "disposed-before-report");
        }
    }

    internal sealed class WaitingModule(ShutdownState state) : Module<bool>
    {
        protected override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            Host.RecordStartupStage(state.Directory, "waiting-module-entered");
            if (state.Mode == "pipeline-sigterm-after-run")
            {
                return true;
            }

            using var registration = state.Mode == "pipeline-exit-blocked-cancellation"
                ? cancellationToken.Register(() => Thread.Sleep(Timeout.Infinite))
                : default;
            Host.RecordStartupStage(state.Directory, "cancellation-registered");
            await File.WriteAllTextAsync(Path.Combine(state.Directory, "ready"), string.Empty, cancellationToken);
            await Task.Delay(Timeout.Infinite, cancellationToken);
            return true;
        }
    }

    [DependsOn<WaitingModule>]
    internal sealed class CleanupModule(ShutdownState state, DisposalMarker marker) : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, CancellationToken.None);
            ObjectDisposedException.ThrowIf(marker.IsDisposed, marker);
            state.Record("cleanup");
            return true;
        }
    }

    internal sealed class EndHandler(ShutdownState state) : IPipelineEventHandler
    {
        public Task OnPipelineEndAsync(IPipelineContext context, PipelineSummary pipelineSummary, CancellationToken cancellationToken)
        {
            state.Record("end");
            return Task.CompletedTask;
        }
    }
}
