using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Extensions;

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

    public sealed class TestArtifactStore : IDistributedArtifactStore
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
            => throw new NotSupportedException();

        public Task DeleteAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken)
            => throw new NotSupportedException();
    }

    public sealed class TestArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
            => throw new NotSupportedException();
    }
}
