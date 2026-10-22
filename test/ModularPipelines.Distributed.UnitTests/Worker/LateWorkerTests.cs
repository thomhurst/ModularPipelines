using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Moq;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Worker;
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
        // The master's execution-start heartbeat is an existing participant, not a late worker.
        var statuses = await coordinator.GetWorkerStatusesAsync(CancellationToken.None);
        await Assert.That(statuses.Select(status => status.WorkerId))
            .IsEquivalentTo([WorkerId.FromInstanceIndex(0)]);
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
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    [Timeout(30_000)]
    public async Task Cancellation_During_Worker_Startup_Publishes_Final_Status_And_Allows_Cleanup(
        bool cancelRegistrationAcknowledgment,
        bool cancelBeforeCommit,
        CancellationToken cancellationToken)
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
        if (cancelRegistrationAcknowledgment)
        {
            var transport = new Mock<IDistributedWorkerCoordinator>(MockBehavior.Strict);
            transport.Setup(instance => instance.RegisterWorkerAsync(It.IsAny<WorkerRegistration>(), It.IsAny<CancellationToken>()))
                .Returns<WorkerRegistration, CancellationToken>(async (registration, token) =>
                {
                    if (cancelBeforeCommit)
                    {
                        await callerCancellation.CancelAsync();
                    }
                    // The master commits the participant before the transport loses its acknowledgment.
                    await coordinator.RegisterWorkerAsync(registration, token);
                    await callerCancellation.CancelAsync();
                    token.ThrowIfCancellationRequested();
                });
            transport.Setup(instance => instance.SendHeartbeatAsync(It.IsAny<WorkerStatus>(), It.IsAny<CancellationToken>()))
                .Returns<WorkerStatus, CancellationToken>(coordinator.SendHeartbeatAsync);
            builder.Services.AddSingleton(transport.Object);
            builder.AddPipelineEventHandler<UnexpectedHandler>();
        }
        else
        {
            builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
            builder.Services.AddSingleton<IPipelineEventHandler>(new CancelingStartHandler(coordinator, callerCancellation));
        }
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

    [Test]
    [Timeout(30_000)]
    public async Task Cancellation_Before_Registration_Does_Not_Register_Or_Publish_Final_Status(CancellationToken cancellationToken)
    {
        var coordinator = new Mock<IDistributedWorkerCoordinator>(MockBehavior.Strict);
        using var callerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { Role = DistributedRole.Worker });
        builder.Services.AddSingleton(coordinator.Object);
        builder.AddModule<UnexpectedModule>();
        builder.AddPipelineEventHandler<UnexpectedHandler>();
        await using var pipeline = await builder.BuildAsync();
        await callerCancellation.CancelAsync();

        await Assert.That(async () =>
        {
            await pipeline.RunAsync(callerCancellation.Token);
        }).Throws<OperationCanceledException>();

        coordinator.VerifyNoOtherCalls();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task Unconfirmed_Registration_Does_Not_Overwrite_A_Live_Worker_Status(
        bool cancelBeforeCommit,
        CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var registration = DistributedTestData.Registration(WorkerId.FromInstanceIndex(1)) with
        {
            RunId = "duplicate-run",
            RegisteredAt = DateTimeOffset.UtcNow.AddMinutes(-1),
        };
        await coordinator.RegisterWorkerAsync(registration, cancellationToken);
        var liveStatus = new WorkerStatus
        {
            WorkerId = registration.WorkerId,
            RunId = registration.RunId,
            InFlightModules = [ModuleId.FromType(typeof(UnexpectedModule))],
            UnattributedCommandCount = 7,
        };
        await coordinator.SendHeartbeatAsync(liveStatus, cancellationToken);
        using var callerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var transport = new Mock<IDistributedWorkerCoordinator>(MockBehavior.Strict);
        transport.Setup(instance => instance.RegisterWorkerAsync(It.IsAny<WorkerRegistration>(), It.IsAny<CancellationToken>()))
            .Returns<WorkerRegistration, CancellationToken>(async (attempt, token) =>
            {
                if (cancelBeforeCommit)
                {
                    await callerCancellation.CancelAsync();
                    // The transport fails before the master accepts this session.
                    throw new OperationCanceledException(callerCancellation.Token);
                }
                await coordinator.RegisterWorkerAsync(attempt, token);
            });
        transport.Setup(instance => instance.SendHeartbeatAsync(It.IsAny<WorkerStatus>(), It.IsAny<CancellationToken>()))
            .Returns<WorkerStatus, CancellationToken>(coordinator.SendHeartbeatAsync);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with
        {
            Role = DistributedRole.Worker,
            InstanceIndex = 1,
            TotalInstances = 2,
            RunId = registration.RunId,
        });
        builder.Services.AddSingleton(transport.Object);
        builder.AddModule<UnexpectedModule>();
        builder.AddPipelineEventHandler<UnexpectedHandler>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.That(async () => await pipeline.RunAsync(callerCancellation.Token)).ThrowsException();

        var statuses = await coordinator.GetWorkerStatusesAsync(cancellationToken);
        await Assert.That(statuses).HasSingleItem();
        await Assert.That(statuses[0]).IsEqualTo(liveStatus);
        var workers = await coordinator.GetRegisteredWorkersAsync(cancellationToken);
        await Assert.That(workers).HasSingleItem();
        await Assert.That(workers[0]).IsEqualTo(registration);
        transport.Verify(instance => instance.SendHeartbeatAsync(It.IsAny<WorkerStatus>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Timeout(30_000)]
    public async Task Slow_Coordinator_Initialization_Leaves_A_Full_Registration_Window(CancellationToken cancellationToken)
    {
        var clock = new FakeTimeProvider();
        var coordinator = new Mock<IDistributedWorkerCoordinator>(MockBehavior.Strict);
        coordinator.Setup(instance => instance.RegisterWorkerAsync(It.IsAny<WorkerRegistration>(), It.IsAny<CancellationToken>()))
            .Returns<WorkerRegistration, CancellationToken>((_, token) =>
            {
                clock.Advance(TimeSpan.FromSeconds(29));
                token.ThrowIfCancellationRequested();
                return Task.CompletedTask;
            });
        var factory = new Mock<IDistributedCoordinatorFactory>(MockBehavior.Strict);
        factory.Setup(instance => instance.CreateWorkerAsync(It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(token =>
            {
                // Discovery and connection may legitimately take longer than the handshake budget.
                clock.Advance(TimeSpan.FromSeconds(31));
                token.ThrowIfCancellationRequested();
                return Task.FromResult(coordinator.Object);
            });
        await using var deferred = new DeferredWorkerCoordinator(factory.Object);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { Role = DistributedRole.Worker });
        builder.Services.AddSingleton<TimeProvider>(clock);
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(deferred);
        builder.AddModule<UnexpectedModule>();
        await using var pipeline = await builder.BuildAsync();
        var executor = pipeline.Services.GetRequiredService<WorkerModuleExecutor>();

        var registration = await executor.RegisterAsync(cancellationToken);

        await Assert.That(registration).IsNotNull();
        await Assert.That(executor.RegistrationConfirmed).IsTrue();
        coordinator.Verify(instance => instance.RegisterWorkerAsync(It.IsAny<WorkerRegistration>(), It.IsAny<CancellationToken>()), Times.Once);
        coordinator.VerifyNoOtherCalls();
        factory.Verify(instance => instance.CreateWorkerAsync(It.IsAny<CancellationToken>()), Times.Once);
        factory.VerifyNoOtherCalls();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Cancellation_During_Coordinator_Initialization_Does_Not_Register_Or_Publish_Status(CancellationToken cancellationToken)
    {
        var coordinator = new Mock<IDistributedWorkerCoordinator>(MockBehavior.Strict);
        var creationStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var creation = new TaskCompletionSource<IDistributedWorkerCoordinator>(TaskCreationOptions.RunContinuationsAsynchronously);
        var factory = new Mock<IDistributedCoordinatorFactory>(MockBehavior.Strict);
        factory.Setup(instance => instance.CreateWorkerAsync(It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                creationStarted.SetResult();
                return creation.Task;
            });
        await using var deferred = new DeferredWorkerCoordinator(factory.Object);
        using var callerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { Role = DistributedRole.Worker });
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(deferred);
        builder.AddModule<UnexpectedModule>();
        builder.AddPipelineEventHandler<UnexpectedHandler>();
        await using var pipeline = await builder.BuildAsync();

        try
        {
            var run = pipeline.RunAsync(callerCancellation.Token);
            await creationStarted.Task.WaitAsync(cancellationToken);
            await callerCancellation.CancelAsync();

            await Assert.That(async () => await run.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken))
                .Throws<OperationCanceledException>();
            await Assert.That(pipeline.Services.GetRequiredService<WorkerModuleExecutor>().RegistrationConfirmed).IsFalse();
            coordinator.VerifyNoOtherCalls();
        }
        finally
        {
            // Shared initialization can finish after its caller stops waiting.
            creation.TrySetResult(coordinator.Object);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task Unresponsive_Registration_Is_Bounded_Without_Publishing_Final_Status(bool deferCoordinator, CancellationToken cancellationToken)
    {
        var clock = new FakeTimeProvider();
        var pendingRegistration = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new Mock<IDistributedWorkerCoordinator>(MockBehavior.Strict);
        coordinator.Setup(instance => instance.RegisterWorkerAsync(It.IsAny<WorkerRegistration>(), It.IsAny<CancellationToken>()))
            .Returns<WorkerRegistration, CancellationToken>((_, _) =>
            {
                clock.Advance(TimeSpan.FromSeconds(31));
                // A coordinator that ignores cancellation must not keep startup alive indefinitely.
                return pendingRegistration.Task;
            });
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options with { Role = DistributedRole.Worker });
        builder.Services.AddSingleton<TimeProvider>(clock);
        var factory = new Mock<IDistributedCoordinatorFactory>(MockBehavior.Strict);
        factory.Setup(instance => instance.CreateWorkerAsync(It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                clock.Advance(TimeSpan.FromSeconds(31));
                return Task.FromResult(coordinator.Object);
            });
        await using var deferred = new DeferredWorkerCoordinator(factory.Object);
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(deferCoordinator ? deferred : coordinator.Object);
        builder.AddModule<UnexpectedModule>();
        builder.AddPipelineEventHandler<UnexpectedHandler>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.That(async () => await pipeline.RunAsync(cancellationToken)).Throws<OperationCanceledException>();

        coordinator.Verify(instance => instance.RegisterWorkerAsync(It.IsAny<WorkerRegistration>(), It.IsAny<CancellationToken>()), Times.Once);
        coordinator.VerifyNoOtherCalls();
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
