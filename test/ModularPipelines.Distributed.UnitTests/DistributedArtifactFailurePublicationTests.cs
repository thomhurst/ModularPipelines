using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests;

public class DistributedArtifactFailurePublicationTests
{
    [ProducesArtifact("package", "package.nupkg")]
    private sealed class Producer : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(4);
    }

    [Test]
    [Arguments("timeout")]
    [Arguments("cancellation")]
    [Arguments("lease-loss")]
    [Timeout(10_000)]
    public async Task Failed_Publication_Retains_Artifact_Upload_Failure(
        string interruption, CancellationToken cancellationToken)
    {
        var directory = Directory.CreateTempSubdirectory("artifact-failure-publication-");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "package.nupkg"), "content", cancellationToken);
            await using var pipeline = await TestPipelineBuilder.Create().AddModule<Producer>().BuildAsync(cancellationToken);
            var module = pipeline.Services.GetServices<IModule>().OfType<Producer>().Single();
            var types = new ModuleTypeRegistry();
            types.Register(typeof(Producer));
            var serializer = new ModuleResultSerializer(types);
            var registry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
            var inner = new InMemoryDistributedCoordinator();
            await inner.EnqueueModuleAsync(DistributedTestData.Assignment(module, types), cancellationToken);
            var lease = (await inner.DequeueModuleAsync(new WorkerId("worker"), new HashSet<Capability>(), cancellationToken))!;
            using var execution = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            using var leaseLost = new CancellationTokenSource();
            var publications = new List<SerializedModuleResult>();
            var publicationTokens = new List<CancellationToken>();
            var coordinator = new Mock<IDistributedWorkerCoordinator>();
            coordinator.Setup(value => value.PublishResultAsync(
                    It.IsAny<SerializedModuleResult>(), It.IsAny<ModuleLease?>(), It.IsAny<CancellationToken>()))
                .Returns<SerializedModuleResult, ModuleLease?, CancellationToken>((result, claim, token) =>
                {
                    publications.Add(result);
                    publicationTokens.Add(token);
                    token.ThrowIfCancellationRequested();
                    if (publications.Count == 1)
                    {
                        if (interruption == "lease-loss")
                        {
                            leaseLost.Cancel();
                        }
                        else if (interruption == "cancellation")
                        {
                            execution.Cancel();
                            return Task.FromCanceled(execution.Token);
                        }

                        return Task.FromException(new TimeoutException("Transient publication failure."));
                    }

                    return inner.PublishResultAsync(result, claim, token);
                });
            var storeFailure = new IOException("store unavailable");
            var store = new Mock<IDistributedArtifactStore>(MockBehavior.Strict);
            store.Setup(value => value.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(storeFailure);
            var artifacts = new ArtifactLifecycleManager(
                store.Object, MsOptions.Create(new DistributedOptions()),
                NullLogger<ArtifactLifecycleManager>.Instance, directory.FullName);
            var executor = new DistributedAssignmentExecutor(
                coordinator.Object, types, serializer,
                pipeline.Services.GetRequiredService<IModuleRunner>(), registry,
                pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
                pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
                pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
                artifacts, null, null, NullLogger.Instance);

            await executor.ExecuteAsync(
                lease, DateTimeOffset.UtcNow,
                DependencyResultApplicator.BuildModuleLookup([module]),
                new DependencyResultCache(coordinator.Object, cancellationToken),
                execution.Token, leaseLost.Token).WaitAsync(cancellationToken);

            // The module completed successfully, but its required upload failed afterwards.
            await Assert.That(module.AsInternal().ResultTask.Result.Status).IsEqualTo(ModuleStatus.Succeeded);
            var local = registry.GetResult(typeof(Producer))!;
            await Assert.That(local.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(local.ExceptionOrDefault).IsTypeOf<ModuleFailedException>();
            await Assert.That(local.ExceptionOrDefault!.InnerException).IsSameReferenceAs(storeFailure);
            await Assert.That(publications.All(result => serializer.Deserialize(result)!.Status == ModuleStatus.Failed)).IsTrue();
            await Assert.That(publicationTokens.All(token => token.CanBeCanceled && token != execution.Token)).IsTrue();

            var accepted = inner.WaitForResultAsync(lease.Assignment.ModuleId, cancellationToken);
            if (interruption == "lease-loss")
            {
                await Assert.That(publications.Count).IsEqualTo(1);
                await Assert.That(accepted.IsCompleted).IsFalse();
            }
            else
            {
                await Assert.That(publications.Count).IsEqualTo(2);
                await Assert.That(publicationTokens[1]).IsNotEqualTo(publicationTokens[0]);
                var result = serializer.Deserialize(await accepted.WaitAsync(cancellationToken))!;
                await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
                await Assert.That(result.ExceptionOrDefault).IsTypeOf<ModuleFailedException>();
                await Assert.That(result.ExceptionOrDefault!.InnerException!.Message).Contains(storeFailure.Message);
                await Assert.That(publications[1].Payload).IsEqualTo(publications[0].Payload);
            }
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
