using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Api;

public class ArtifactContextApiTests
{
    [Test]
    public async Task Artifact_Context_Is_A_Property_With_Optional_Cancellation_Tokens()
    {
        var artifactsProperty = typeof(IPipelineContext).GetProperty(nameof(IPipelineContext.Artifacts));
        var cancellationParameters = typeof(IArtifactContext)
            .GetMethods()
            .SelectMany(static method => method.GetParameters())
            .Where(static parameter => parameter.ParameterType == typeof(CancellationToken))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(artifactsProperty).IsNotNull();
            await Assert.That(artifactsProperty!.PropertyType).IsEqualTo(typeof(IArtifactContext));
            await Assert.That(cancellationParameters).IsNotEmpty();
            await Assert.That(cancellationParameters.All(static parameter => parameter.IsOptional))
                .IsTrue();
            await Assert.That(typeof(IPipelineContext).Assembly.GetType(
                    "ModularPipelines.Distributed.Extensions.ArtifactContextExtensions"))
                .IsNull();
        }
    }

    [Test]
    public async Task Artifact_Store_Builder_Methods_Register_Symmetric_Services()
    {
        var storeBuilder = Pipeline.CreateBuilder()
            .AddDistributedArtifactStore<TestArtifactStore>();
        var factoryBuilder = Pipeline.CreateBuilder()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();

        using (Assert.Multiple())
        {
            await Assert.That(storeBuilder.Services.Any(descriptor =>
                    descriptor.ServiceType == typeof(IDistributedArtifactStore) &&
                    descriptor.ImplementationType == typeof(TestArtifactStore)))
                .IsTrue();
            await Assert.That(factoryBuilder.Services.Any(descriptor =>
                    descriptor.ServiceType == typeof(IDistributedArtifactStoreFactory) &&
                    descriptor.ImplementationType == typeof(TestArtifactStoreFactory)))
                .IsTrue();
        }
    }

    [Test]
    public async Task Distributed_Surface_Uses_TimeSpan_And_Run_Naming()
    {
        var assembly = typeof(DistributedOptions).Assembly;
        var typedDownload = typeof(IArtifactContext)
            .GetMethods()
            .Single(method => method.Name == nameof(IArtifactContext.DownloadAsync) && method.IsGenericMethod);

        using (Assert.Multiple())
        {
            await Assert.That(typeof(ArtifactOptions).GetProperty("TimeToLive")!.PropertyType)
                .IsEqualTo(typeof(TimeSpan));
            await Assert.That(typeof(ArtifactOptions).GetProperty("TimeToLiveSeconds")).IsNull();
            await Assert.That(typeof(DistributedOptions).GetProperty("CapabilityTimeout")!.PropertyType)
                .IsEqualTo(typeof(TimeSpan));
            await Assert.That(typeof(DistributedOptions).GetProperty("ModuleResultTimeout")!.PropertyType)
                .IsEqualTo(typeof(TimeSpan));
            await Assert.That(typeof(DistributedOptions).GetProperty("ExecutionIdentifier")).IsNull();
            await Assert.That(typeof(WorkerRegistration).GetProperty("ExecutionIdentifier")).IsNull();
            await Assert.That(typeof(WorkerRegistration).GetProperty("RunIdentifier")).IsNotNull();
            await Assert.That(assembly.GetType("ModularPipelines.Distributed.ModuleAssignmentConfig"))
                .IsNull();
            await Assert.That(assembly.GetType(
                    "ModularPipelines.Distributed.ModuleAssignmentConfiguration"))
                .IsNotNull();
            await Assert.That(typedDownload.GetGenericArguments()).HasSingleItem();
        }
    }

    [Test]
    public async Task Artifact_Store_Factory_Is_Active_Without_Distributed_Mode()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();

        await using var pipeline = await builder.BuildAsync();
        var factory = pipeline.Services.GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);

        await Assert.That(((TestArtifactStoreFactory) factory).CreateCount).IsEqualTo(1);
    }

    [Test]
    public async Task Direct_Artifact_Store_Overrides_Earlier_Factory()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>()
            .AddDistributedArtifactStore<TestArtifactStore>();

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipeline.Services.GetRequiredService<IDistributedArtifactStore>())
                .IsTypeOf<TestArtifactStore>();
            await Assert.That(pipeline.Services.GetService<IDistributedArtifactStoreFactory>())
                .IsNull();
        }
    }

    [Test]
    public async Task Artifact_Store_Factory_Disposes_Created_Store()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);
        await pipeline.DisposeAsync();

        await Assert.That(factory.CreatedStore!.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Artifact_Store_Factory_Synchronous_Disposal_Disposes_Async_Store()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);
        ((IDisposable) store).Dispose();

        await Assert.That(factory.CreatedStore!.DisposeCount).IsEqualTo(1);
        await pipeline.DisposeAsync();
    }

    [Test]
    public async Task Artifact_Store_Factory_Does_Not_Initialize_During_Disposal()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();

        _ = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();
        await pipeline.DisposeAsync();

        await Assert.That(factory.CreateCount).IsEqualTo(0);
    }

    [Test]
    public async Task Artifact_Store_Factory_Disposes_Synchronous_Store()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestDisposableArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestDisposableArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);
        await pipeline.DisposeAsync();

        await Assert.That(factory.CreatedStore!.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Artifact_Store_Factory_Synchronizes_Creation_With_Disposal()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<BlockingArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (BlockingArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        var accessTask = store.ListArtifactsAsync("module", CancellationToken.None);
        await factory.CreationStarted;
        var disposalTask = pipeline.DisposeAsync().AsTask();
        factory.CompleteCreation();

        _ = await Assert.ThrowsAsync<ObjectDisposedException>(() => accessTask);
        await disposalTask;
        await Assert.That(factory.CreatedStore.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Ready_Hook_Artifacts_Use_The_Hook_Module_Type()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ReadyArtifactModule>()
            .AddDistributedArtifactStore<TestArtifactStore>();

        await using var pipeline = await builder.BuildAsync();
        var store = (TestArtifactStore) pipeline.Services
            .GetRequiredService<IDistributedArtifactStore>();

        await pipeline.RunAsync();

        await Assert.That(store.UploadedDescriptor!.ModuleTypeName)
            .IsEqualTo(typeof(ReadyArtifactModule).FullName);
    }

    private sealed class ArtifactTestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    [PublishArtifactOnReady]
    private sealed class ReadyArtifactModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    private sealed class PublishArtifactOnReadyAttribute : Attribute, IModuleReadyHandler
    {
        public async Task OnModuleReadyAsync(IModuleHookContext context)
        {
            var path = Path.GetTempFileName();
            try
            {
                await context.Artifacts.PublishFileAsync("ready", path);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }

    public class TestArtifactStore : IDistributedArtifactStore
    {
        public ArtifactDescriptor? UploadedDescriptor { get; private set; }

        public Task<ArtifactReference> UploadAsync(
            ArtifactDescriptor descriptor,
            Stream data,
            CancellationToken cancellationToken)
        {
            UploadedDescriptor = descriptor;
            return Task.FromResult(new ArtifactReference(
                Guid.NewGuid().ToString("N"),
                descriptor.Name,
                descriptor.ModuleTypeName,
                data.Length,
                descriptor.ContentType,
                DateTimeOffset.UtcNow));
        }

        public Task<Stream> DownloadAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken)
            => throw new NotSupportedException();

        public Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(
            string moduleTypeName,
            CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<ArtifactReference>>([]);

        public Task DeleteAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken)
            => throw new NotSupportedException();
    }

    public sealed class TestAsyncDisposableArtifactStore : TestArtifactStore, IAsyncDisposable
    {
        public int DisposeCount { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposeCount++;
            return ValueTask.CompletedTask;
        }
    }

    public sealed class TestDisposableArtifactStore : TestArtifactStore, IDisposable
    {
        public int DisposeCount { get; private set; }

        public void Dispose() => DisposeCount++;
    }

    public sealed class TestArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public int CreateCount { get; private set; }

        public TestAsyncDisposableArtifactStore? CreatedStore { get; private set; }

        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
        {
            CreateCount++;
            CreatedStore = new TestAsyncDisposableArtifactStore();
            return Task.FromResult<IDistributedArtifactStore>(CreatedStore);
        }
    }

    public sealed class TestDisposableArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public TestDisposableArtifactStore? CreatedStore { get; private set; }

        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
        {
            CreatedStore = new TestDisposableArtifactStore();
            return Task.FromResult<IDistributedArtifactStore>(CreatedStore);
        }
    }

    public sealed class BlockingArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        private readonly TaskCompletionSource _creationStarted = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _completeCreation = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public Task CreationStarted => _creationStarted.Task;

        public TestAsyncDisposableArtifactStore CreatedStore { get; } = new();

        public void CompleteCreation() => _completeCreation.SetResult();

        public async Task<IDistributedArtifactStore> CreateAsync(
            CancellationToken cancellationToken)
        {
            _creationStarted.SetResult();
            await _completeCreation.Task.WaitAsync(cancellationToken);
            return CreatedStore;
        }
    }
}
