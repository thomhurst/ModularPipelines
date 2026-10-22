using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Events;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;
using PipelineEngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class ShutdownPipelineTests
{
    [Test]
    [Timeout(30_000)]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Shutdown_Cancels_The_Shared_Probe_And_Notifies_Workers(bool sigterm, CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var provider = new WaitingCapabilityProvider();
        var endHandler = new RecordingEndHandler();
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { TotalInstances = 1 });
        builder.Services.AddSingleton<IDistributedMasterCoordinator>(coordinator);
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        builder.Services.AddSingleton<ICapabilityProvider>(provider);
        builder.Services.AddSingleton<IPipelineEventHandler>(endHandler);
        builder.AddModule<ProbeModule>();

        await using var pipeline = await builder.BuildAsync();
        var run = pipeline.RunAsync(cancellationToken);
        await provider.Started.Task.WaitAsync(cancellationToken);
        var engineCancellation = pipeline.Services.GetRequiredService<PipelineEngineCancellationToken>();

        var handled = sigterm ? engineCancellation.HandleSigterm() : engineCancellation.HandleCancelKeyPress();

        await Assert.That(handled).IsTrue();
        await provider.Canceled.Task.WaitAsync(cancellationToken);
        await Assert.That(await coordinator.WaitForCancellationAsync(cancellationToken))
            .IsEqualTo(DistributedCancellationReason.Stopped);
        try
        {
            await run;
        }
        catch (OperationCanceledException)
        {
            // The pipeline was interrupted while resolving capabilities.
        }

        await Assert.That(pipeline.Services.GetRequiredService<IHostApplicationLifetime>()
            .ApplicationStopping.IsCancellationRequested).IsTrue();
        await Assert.That(endHandler.Calls).IsEqualTo(1);
        await Assert.That(endHandler.WasCanceled).IsFalse();
        await Assert.That(engineCancellation.ExplicitCancellationToken.IsCancellationRequested).IsFalse();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Normal_Completion_Does_Not_Broadcast_Stopped(CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { TotalInstances = 1 });
        builder.Services.AddSingleton<IDistributedMasterCoordinator>(coordinator);
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        builder.AddModule<ProbeModule>();

        await using var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync(cancellationToken);

        await Assert.That(summary.Succeeded).IsTrue();
        await Assert.That(pipeline.Services.GetRequiredService<IHostApplicationLifetime>()
            .ApplicationStopping.IsCancellationRequested).IsFalse();
        using var waiterCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var notification = coordinator.WaitForCancellationAsync(waiterCancellation.Token);
        await Assert.That(notification.IsCompleted).IsFalse();
        await waiterCancellation.CancelAsync();
        await Assert.That(async () => await notification).Throws<OperationCanceledException>();
    }

    private sealed class ProbeModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class WaitingCapabilityProvider : ICapabilityProvider
    {
        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource Canceled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public async Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken)
        {
            Started.TrySetResult();
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return [];
            }
            finally
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Canceled.TrySetResult();
                }
            }
        }
    }

    private sealed class RecordingEndHandler : IPipelineEventHandler
    {
        public int Calls { get; private set; }

        public bool WasCanceled { get; private set; }

        public Task OnPipelineEndAsync(IPipelineContext context, PipelineSummary pipelineSummary, CancellationToken cancellationToken)
        {
            Calls++;
            WasCanceled = cancellationToken.IsCancellationRequested;
            return Task.CompletedTask;
        }
    }
}
