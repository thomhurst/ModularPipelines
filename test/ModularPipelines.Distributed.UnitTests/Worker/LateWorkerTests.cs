using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Enums;
using ModularPipelines.Events;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class LateWorkerTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task Late_Worker_Succeeds_Without_Pipeline_Handlers_Modules_Or_Final_Status(bool showProgress, CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        await coordinator.SignalCompletionAsync(CancellationToken.None);
        var builder = TestPipelineBuilder.Create(new TestHostSettings { ShowProgressInConsole = showProgress });
        builder.AddDistributedMode(options => options with
        {
            Role = DistributedRole.Worker,
            InstanceIndex = 1,
            TotalInstances = 2,
            RunId = "completed-run",
        });
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        builder.AddModule<UnexpectedModule>();
        builder.AddPipelineEventHandler<UnexpectedHandler>();
        await using var pipeline = await builder.BuildAsync();

        var summary = await pipeline.RunAsync(cancellationToken);

        await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(summary.Results).IsEmpty();
        await Assert.That(await coordinator.GetRegisteredWorkersAsync(CancellationToken.None)).IsEmpty();
        await Assert.That(await coordinator.GetWorkerStatusesAsync(CancellationToken.None)).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Cleanup_Cannot_Gain_A_New_Participant_After_Reading_Worker_Statuses(CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var cleanup = new RegisteringCleanup(coordinator);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { TotalInstances = 1, RunId = "cleanup-run" });
        builder.Services.AddSingleton<IDistributedMasterCoordinator>(coordinator);
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<SucceedingModule>();
        await using var pipeline = await builder.BuildAsync();

        var summary = await pipeline.RunAsync(cancellationToken);

        await Assert.That(summary.Succeeded).IsTrue();
        await Assert.That(cleanup.RegistrationRejected).IsTrue();
        await Assert.That(await coordinator.GetWorkerStatusesAsync(CancellationToken.None)).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Accepted_Worker_Stays_Live_During_Start_Handlers_And_Reuses_Its_Session(CancellationToken cancellationToken)
    {
        var clock = new FakeTimeProvider();
        var coordinator = new InMemoryDistributedCoordinator(Microsoft.Extensions.Options.Options.Create(
            new DistributedOptions { WorkerTimeout = TimeSpan.FromSeconds(30) }), clock);
        var handler = new CompletingStartHandler(coordinator, clock);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with
        {
            Role = DistributedRole.Worker,
            InstanceIndex = 1,
            TotalInstances = 2,
            RunId = "finishing-run",
            WorkerHeartbeatInterval = TimeSpan.FromMilliseconds(10),
        });
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        builder.Services.AddSingleton<IPipelineEventHandler>(handler);
        builder.AddModule<UnexpectedModule>();
        await using var pipeline = await builder.BuildAsync();

        var summary = await pipeline.RunAsync(cancellationToken);

        await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(summary.Results).IsEmpty();
        await Assert.That(handler.EndCalled).IsTrue();
        var statuses = await coordinator.GetWorkerStatusesAsync(CancellationToken.None);
        await Assert.That(statuses).HasSingleItem();
        await Assert.That(statuses[0].IsFinal).IsTrue();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Cancellation_During_Start_Handlers_Publishes_Final_Status_And_Allows_Cleanup(CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        using var callerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with
        {
            Role = DistributedRole.Worker,
            InstanceIndex = 1,
            TotalInstances = 2,
            RunId = "canceled-start-run",
        });
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        builder.Services.AddSingleton<IPipelineEventHandler>(new CancelingStartHandler(coordinator, callerCancellation));
        builder.AddModule<UnexpectedModule>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.That(async () =>
        {
            await pipeline.RunAsync(callerCancellation.Token);
        }).Throws<OperationCanceledException>();

        var statuses = await coordinator.GetWorkerStatusesAsync(cancellationToken);
        await Assert.That(statuses).HasSingleItem();
        await Assert.That(statuses[0].IsFinal).IsTrue();
        await Assert.That(statuses[0].InFlightModules).IsEmpty();

        var cleanup = new RecordingCleanup();
        var masterBuilder = TestPipelineBuilder.Create();
        masterBuilder.AddDistributedMode(options => options with
        {
            TotalInstances = 2,
            RunId = "canceled-start-run",
            RunCleanupTimeout = TimeSpan.FromSeconds(1),
        });
        masterBuilder.Services.AddSingleton<IDistributedMasterCoordinator>(coordinator);
        masterBuilder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
        masterBuilder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        masterBuilder.AddModule<SucceedingModule>();
        await using var masterPipeline = await masterBuilder.BuildAsync();

        await masterPipeline.RunAsync(cancellationToken);

        await Assert.That(cleanup.Called).IsTrue();
    }

    private sealed class CancelingStartHandler(
        InMemoryDistributedCoordinator coordinator,
        CancellationTokenSource callerCancellation) : IPipelineEventHandler
    {
        public async Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Assert.That(await coordinator.GetRegisteredWorkersAsync(cancellationToken)).HasSingleItem();
            await callerCancellation.CancelAsync();
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private sealed class RecordingCleanup : IDistributedRunCleanup
    {
        public bool Called { get; private set; }

        public Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
        {
            Called = true;
            return Task.CompletedTask;
        }
    }

    private sealed class RegisteringCleanup(InMemoryDistributedCoordinator coordinator) : IDistributedRunCleanup
    {
        public bool RegistrationRejected { get; private set; }

        public async Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
        {
            // Cleanup reaches its backends only after reading the final participant set.
            await Assert.That(async () => await coordinator.RegisterWorkerAsync(
                    DistributedTestData.Registration(new WorkerId("late-cleanup-worker")) with { RunId = context.RunId },
                    cancellationToken))
                .Throws<DistributedRunCompletedException>();
            RegistrationRejected = true;
        }
    }

    private sealed class SucceedingModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(1);
    }

    private sealed class CompletingStartHandler(InMemoryDistributedCoordinator coordinator, FakeTimeProvider clock) : IPipelineEventHandler
    {
        public bool EndCalled { get; private set; }

        public async Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Assert.That(await coordinator.GetRegisteredWorkersAsync(cancellationToken)).HasSingleItem();
            // Advance beyond the timeout while startup still owns the pipeline lifecycle.
            clock.Advance(TimeSpan.FromMinutes(1));
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(5));
            while ((await coordinator.GetRegisteredWorkersAsync(timeout.Token)).Count == 0)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10), timeout.Token);
            }

            await coordinator.SignalCompletionAsync(cancellationToken);
        }

        public Task OnPipelineEndAsync(IPipelineContext context, PipelineSummary summary, CancellationToken cancellationToken)
        {
            EndCalled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class UnexpectedModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A late worker must not run modules.");
    }

    private sealed class UnexpectedHandler : IPipelineEventHandler
    {
        public Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A late worker must not run start handlers.");

        public Task OnPipelineEndAsync(IPipelineContext context, PipelineSummary summary, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A late worker must not run end handlers.");
    }
}
