using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests.Worker;

/// <summary>
/// Worker behaviour around pipeline failures, unresolvable assignments and artifact uploads.
/// </summary>
public class WorkerFailureHandlingTests
{
    private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);

    public sealed class Gate
    {
        public TaskCompletionSource NormalStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource AlwaysRunStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource ReleaseAlwaysRun { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    private sealed class NormalModule(Gate gate) : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            gate.NormalStarted.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return 1;
        }
    }

    private sealed class TeardownModule(Gate gate) : Module<int>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            gate.AlwaysRunStarted.TrySetResult();
            await gate.ReleaseAlwaysRun.Task.WaitAsync(cancellationToken);
            return 2;
        }
    }

    [ProducesArtifact("package", "does-not-exist/*.nupkg")]
    private sealed class ProducerModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(3);
    }

    [Test]
    public async Task Pipeline_Failure_Cancels_Normal_Work_But_Not_AlwaysRun_Work()
    {
        var gate = new Gate();
        await using var harness = await Harness.CreateAsync(
            builder =>
            {
                builder.Services.AddSingleton(gate);
                builder.AddModule<NormalModule>();
                builder.AddModule<TeardownModule>();
            },
            maxParallelism: 2);
        await harness.EnqueueAsync<NormalModule>();
        await harness.EnqueueAsync<TeardownModule>(alwaysRun: true);
        var run = harness.RunWorkerAsync();
        await gate.NormalStarted.Task.WaitAsync(TestTimeout);
        await gate.AlwaysRunStarted.Task.WaitAsync(TestTimeout);

        await harness.Coordinator.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed, CancellationToken.None);
        var normal = await harness.WaitForResultAsync<NormalModule>();
        var alwaysRunStillRunning = !harness.Coordinator.WaitForResultAsync(
            ModuleId.FromType(typeof(TeardownModule)),
            CancellationToken.None).IsCompleted;
        gate.ReleaseAlwaysRun.SetResult();
        var alwaysRun = await harness.WaitForResultAsync<TeardownModule>();
        await harness.Coordinator.SignalCompletionAsync(CancellationToken.None);
        await run.WaitAsync(TestTimeout);

        // The normal module stopped because its token was cancelled, and the worker published it.
        await Assert.That(normal.ExceptionOrDefault).IsNotNull();
        await Assert.That(normal.Status).IsNotEqualTo(ModuleStatus.Succeeded);
        await Assert.That(alwaysRunStillRunning).IsTrue();
        await Assert.That(alwaysRun.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Unresolvable_Assignment_Publishes_A_Descriptive_Failure()
    {
        await using var harness = await Harness.CreateAsync(builder => builder.AddModule<ProducerModule>());
        var masterRegistry = new ModuleTypeRegistry();
        masterRegistry.Register(typeof(ProducerModule));
        masterRegistry.Register(typeof(NormalModule));
        await harness.Coordinator.EnqueueModuleAsync(
            DistributedTestData.Assignment(ModuleId.FromType(typeof(NormalModule)), harness.SchemaVersion),
            CancellationToken.None);
        var run = harness.RunWorkerAsync();

        var serialized = await harness.Coordinator.WaitForResultAsync(
                ModuleId.FromType(typeof(NormalModule)),
                CancellationToken.None)
            .WaitAsync(TestTimeout);
        await harness.Coordinator.SignalCompletionAsync(CancellationToken.None);
        await run.WaitAsync(TestTimeout);
        var result = new ModuleResultSerializer(masterRegistry).Deserialize(serialized)!;

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(result.ExceptionOrDefault!.Message).Contains("cannot execute distributed module");
        await Assert.That(serialized.Payload).IsNotEqualTo("null");
    }

    [Test]
    public async Task Missing_Required_Artifact_Fails_The_Module()
    {
        var store = new InMemoryDistributedArtifactStore();
        await using var harness = await Harness.CreateAsync(
            builder => builder.AddModule<ProducerModule>(),
            artifactStore: store);
        await harness.Coordinator.EnqueueModuleAsync(
            DistributedTestData.Assignment(ModuleId.FromType(typeof(ProducerModule)), harness.SchemaVersion) with
            {
                RequiredArtifacts = ["package"],
            },
            CancellationToken.None);
        var run = harness.RunWorkerAsync();

        var result = await harness.WaitForResultAsync<ProducerModule>();
        await harness.Coordinator.SignalCompletionAsync(CancellationToken.None);
        await run.WaitAsync(TestTimeout);

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(result.ExceptionOrDefault).IsTypeOf<ModuleFailedException>();
        await Assert.That(result.ExceptionOrDefault!.InnerException!.Message).Contains("did not produce required artifacts");
    }

    [Test]
    public async Task Artifact_Upload_Failure_Fails_The_Module()
    {
        var directory = Directory.CreateTempSubdirectory("upload-failure");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "package.nupkg"), "content");
            await using var harness = await Harness.CreateAsync(
                builder => builder.AddModule<UploadingProducerModule>(),
                artifactStore: new FailingArtifactStore(),
                workingDirectory: directory.FullName);
            await harness.EnqueueAsync<UploadingProducerModule>();
            var run = harness.RunWorkerAsync();

            var result = await harness.WaitForResultAsync<UploadingProducerModule>();
            await harness.Coordinator.SignalCompletionAsync(CancellationToken.None);
            await run.WaitAsync(TestTimeout);

            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result.ExceptionOrDefault).IsTypeOf<ModuleFailedException>();
            await Assert.That(result.ExceptionOrDefault!.InnerException!.Message).Contains("store unavailable");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [ProducesArtifact("package", "*.nupkg")]
    private sealed class UploadingProducerModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(4);
    }

    private sealed class FailingArtifactStore : IDistributedArtifactStore
    {
        public Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken) =>
            throw new IOException("store unavailable");

        public Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<ArtifactReference>>([]);

        public Task DeleteAsync(ArtifactReference reference, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class Harness : IAsyncDisposable
    {
        private readonly IPipeline _pipeline;
        private readonly WorkerModuleExecutor _executor;
        private readonly IModule[] _modules;
        private readonly ModuleResultSerializer _serializer;

        private Harness(
            IPipeline pipeline,
            IModule[] modules,
            ModuleTypeRegistry typeRegistry,
            InMemoryDistributedCoordinator coordinator,
            WorkerModuleExecutor executor)
        {
            _pipeline = pipeline;
            _modules = modules;
            _executor = executor;
            _serializer = new ModuleResultSerializer(typeRegistry);
            Coordinator = coordinator;
            SchemaVersion = typeRegistry.GetPipelineSchemaVersion();
        }

        public InMemoryDistributedCoordinator Coordinator { get; }

        public string SchemaVersion { get; }

        public static async Task<Harness> CreateAsync(
            Action<PipelineBuilder> configure,
            int maxParallelism = 1,
            IDistributedArtifactStore? artifactStore = null,
            string? workingDirectory = null)
        {
            var builder = TestPipelineBuilder.Create();
            configure(builder);
            var pipeline = await builder.BuildAsync();
            var modules = pipeline.Services.GetServices<IModule>().ToArray();
            var typeRegistry = new ModuleTypeRegistry();
            foreach (var module in modules)
            {
                typeRegistry.Register(module.GetType());
            }

            var coordinator = new InMemoryDistributedCoordinator();
            var artifacts = artifactStore is null
                ? null
                : new ArtifactLifecycleManager(
                    artifactStore,
                    MsOptions.Create(new ArtifactOptions()),
                    NullLogger<ArtifactLifecycleManager>.Instance,
                    workingDirectory ?? Directory.GetCurrentDirectory());
            var executor = new WorkerModuleExecutor(
                pipeline.Services.GetRequiredService<IHostApplicationLifetime>(),
                coordinator,
                modules,
                typeRegistry,
                new ModuleResultSerializer(typeRegistry),
                pipeline.Services.GetRequiredService<IModuleRunner>(),
                pipeline.Services.GetRequiredService<IModuleResultRegistry>(),
                pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
                pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
                MsOptions.Create(new DistributedOptions { InstanceIndex = 1, MaxParallelism = maxParallelism }),
                pipeline.Services.GetRequiredService<IParallelLimitProvider>(),
                pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
                artifacts,
                NullLogger<WorkerModuleExecutor>.Instance);
            return new Harness(pipeline, modules, typeRegistry, coordinator, executor);
        }

        public Task EnqueueAsync<TModule>(bool alwaysRun = false) =>
            Coordinator.EnqueueModuleAsync(
                DistributedTestData.Assignment(ModuleId.FromType(typeof(TModule)), SchemaVersion) with
                {
                    AlwaysRun = alwaysRun,
                },
                CancellationToken.None);

        public Task<IReadOnlyList<IModuleResult>> RunWorkerAsync() => _executor.ExecuteAsync(_modules);

        public async Task<IModuleResult> WaitForResultAsync<TModule>()
        {
            var serialized = await Coordinator.WaitForResultAsync(ModuleId.FromType(typeof(TModule)), CancellationToken.None)
                .WaitAsync(TestTimeout);
            return _serializer.Deserialize(serialized)!;
        }

        public async ValueTask DisposeAsync() => await _pipeline.DisposeAsync();
    }
}
