using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.UnitTests.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class RunCleanupPipelineTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Successful_Distributed_Run_Cleans_Up_Once_With_The_Run_Identifier(CancellationToken cancellationToken)
    {
        var cleanup = new RecordingCleanup();
        var builder = CreateDistributedBuilder(options => options);
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<SucceedingModule>();

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        var runId = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value.RunId;
        await Assert.That(cleanup.Calls).HasSingleItem();
        await Assert.That(cleanup.Calls.Single()).IsEqualTo(new DistributedRunCleanupContext
        {
            RunId = runId,
            PipelineSucceeded = true,
        });
    }

    [Test]
    [Timeout(30_000)]
    public async Task Local_Run_Keeps_Its_Data(CancellationToken cancellationToken)
    {
        var cleanup = new RecordingCleanup();
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<SucceedingModule>();

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        // Separate local runs can share a run identifier and its artifacts.
        await Assert.That(cleanup.Calls).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task OnSuccess_Policy_Keeps_A_Failed_Run_Data(CancellationToken cancellationToken)
    {
        var cleanup = new RecordingCleanup();
        var builder = CreateDistributedBuilder(options => options with { RunCleanup = DistributedRunCleanupPolicy.OnSuccess });
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<FailingModule>();

        await using var pipeline = await builder.BuildAsync();
        await RunIgnoringFailureAsync(pipeline, cancellationToken);

        await Assert.That(cleanup.Calls).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Failed_Run_Is_Cleaned_Up_By_Default(CancellationToken cancellationToken)
    {
        var cleanup = new RecordingCleanup();
        var builder = CreateDistributedBuilder(options => options);
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<FailingModule>();

        await using var pipeline = await builder.BuildAsync();
        await RunIgnoringFailureAsync(pipeline, cancellationToken);

        await Assert.That(cleanup.Calls).HasSingleItem();
        await Assert.That(cleanup.Calls.Single().PipelineSucceeded).IsFalse();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Never_Policy_Keeps_A_Successful_Run_Data(CancellationToken cancellationToken)
    {
        var cleanup = new RecordingCleanup();
        var builder = CreateDistributedBuilder(options => options with { RunCleanup = DistributedRunCleanupPolicy.Never });
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<SucceedingModule>();

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        await Assert.That(cleanup.Calls).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Coordinator_That_Opts_In_Is_Cleaned_Up_Once(CancellationToken cancellationToken)
    {
        var builder = CreateDistributedBuilder(options => options);
        builder.AddDistributedCoordinator<CleanableCoordinator>();

        // A backend registered both ways is still cleaned up once.
        builder.Services.AddSingleton<IDistributedRunCleanup>(serviceProvider =>
            serviceProvider.GetRequiredService<CleanableCoordinator>());
        builder.AddModule<SucceedingModule>();

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        var coordinator = pipeline.Services.GetRequiredService<CleanableCoordinator>();
        await Assert.That(coordinator.CleanupCount).IsEqualTo(1);
    }

    [Test]
    [Timeout(30_000)]
    public async Task Failing_Or_Hanging_Backend_Does_Not_Fail_The_Pipeline_Or_Stop_Other_Backends(
        CancellationToken cancellationToken)
    {
        var cleanup = new RecordingCleanup();
        var builder = CreateDistributedBuilder(options => options with { RunCleanupTimeout = TimeSpan.FromMilliseconds(200) });
        builder.Services.AddSingleton<IDistributedRunCleanup>(new ThrowingCleanup());
        builder.Services.AddSingleton<IDistributedRunCleanup>(new HangingCleanup());
        builder.Services.AddSingleton<IDistributedRunCleanup>(cleanup);
        builder.AddModule<SucceedingModule>();

        await using var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync(cancellationToken);

        await Assert.That(summary.Succeeded).IsTrue();
        await Assert.That(cleanup.Calls).HasSingleItem();
    }

    [Test]
    public async Task Worker_Never_Cleans_Up()
    {
        var cleanup = new RecordingCleanup();
        var services = new ServiceCollection()
            .AddSingleton<IDistributedRunCleanup>(cleanup)
            .BuildServiceProvider();
        var location = new Mock<IExecutionLocationContext>();
        location.SetupGet(context => context.IsWorker).Returns(true);
        var executor = new DistributedRunCleanupExecutor(
            services,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                RunId = "run",
                RunCleanup = DistributedRunCleanupPolicy.Always,
            }.EnableForTest()),
            location.Object,
            NullLogger<DistributedRunCleanupExecutor>.Instance);

        await executor.CleanupAsync(pipelineSucceeded: true);

        await Assert.That(cleanup.Calls).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Factory_Created_Coordinator_Also_Registered_Directly_Is_Cleaned_Up_Once(
        CancellationToken cancellationToken)
    {
        var builder = CreateDistributedBuilder(options => options);
        builder.Services.AddSingleton<CleanableCoordinator>();
        builder.AddDistributedCoordinatorFactory<CoordinatorFactory>();

        // The deferred wrapper and the coordinator it creates are both candidates.
        builder.Services.AddSingleton<IDistributedRunCleanup>(serviceProvider =>
            serviceProvider.GetRequiredService<CleanableCoordinator>());
        builder.AddModule<SucceedingModule>();

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        await Assert.That(pipeline.Services.GetRequiredService<IDistributedMasterCoordinator>())
            .IsTypeOf<DeferredMasterCoordinator>();
        await Assert.That(pipeline.Services.GetRequiredService<CleanableCoordinator>().CleanupCount).IsEqualTo(1);
    }

    private static PipelineBuilder CreateDistributedBuilder(Func<DistributedOptions, DistributedOptions> configure)
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => configure(options with { TotalInstances = 1 }));
        return builder;
    }

    private static async Task RunIgnoringFailureAsync(IPipeline pipeline, CancellationToken cancellationToken)
    {
        try
        {
            await pipeline.RunAsync(cancellationToken);
        }
        catch (Exception)
        {
            // The module failure is the scenario under test.
        }
    }

    private sealed class RecordingCleanup : IDistributedRunCleanup
    {
        private readonly ConcurrentQueue<DistributedRunCleanupContext> _calls = new();

        public IReadOnlyList<DistributedRunCleanupContext> Calls => [.. _calls];

        public Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
        {
            _calls.Enqueue(context);
            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingCleanup : IDistributedRunCleanup
    {
        public Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Simulated cleanup failure");
    }

    private sealed class HangingCleanup : IDistributedRunCleanup
    {
        public Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken) =>
            Task.Delay(Timeout.InfiniteTimeSpan, CancellationToken.None);
    }

    private sealed class CleanableCoordinator : InMemoryDistributedCoordinator, IDistributedRunCleanup
    {
        private int _cleanupCount;

        public int CleanupCount => Volatile.Read(ref _cleanupCount);

        public Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _cleanupCount);
            return Task.CompletedTask;
        }
    }

    private sealed class CoordinatorFactory(CleanableCoordinator coordinator) : IDistributedCoordinatorFactory
    {
        public Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IDistributedMasterCoordinator>(coordinator);

        public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IDistributedWorkerCoordinator>(coordinator);
    }

    private sealed class SucceedingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    private sealed class FailingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidDataException("Simulated module failure");
    }
}
