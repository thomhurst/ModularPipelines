using Microsoft.Extensions.DependencyInjection;
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

    private sealed class ArtifactTestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    public class TestArtifactStore : IDistributedArtifactStore
    {
        public Task<ArtifactReference> UploadAsync(
            ArtifactDescriptor descriptor,
            Stream data,
            CancellationToken cancellationToken)
            => throw new NotSupportedException();

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
}
