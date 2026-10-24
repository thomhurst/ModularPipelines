using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests;

public class DistributedAssignmentPublicationTests
{
    [ProducesArtifact("output", "output.txt")]
    private sealed class Producer : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("produced");
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Timeout(10_000)]
    public async Task Failed_Publication_Preserves_Prepared_Result_Unless_Lease_Is_Lost(
        bool cancelExecution, bool loseLease, CancellationToken cancellationToken)
    {
        var directory = Directory.CreateTempSubdirectory("publication-retry-");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "output.txt"), "artifact", cancellationToken);
            var builder = TestPipelineBuilder.Create().AddModule<Producer>();
            await using var pipeline = await builder.BuildAsync(cancellationToken);
            var module = pipeline.Services.GetServices<IModule>().OfType<Producer>().Single();
            var types = new ModuleTypeRegistry();
            types.Register(typeof(Producer));
            var serializer = new ModuleResultSerializer(types);
            var assignment = DistributedTestData.Assignment(module, types);
            var lease = DistributedTestData.Lease(assignment);
            using var execution = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            using var leaseLost = new CancellationTokenSource();
            var publications = new List<SerializedModuleResult>();
            SerializedModuleResult? acceptedResult = null;
            var coordinator = new Mock<IDistributedWorkerCoordinator>();
            coordinator.Setup(value => value.PublishResultAsync(
                    It.IsAny<SerializedModuleResult>(), It.IsAny<ModuleLease?>(), It.IsAny<CancellationToken>()))
                .Returns<SerializedModuleResult, ModuleLease?, CancellationToken>((result, _, token) =>
                {
                    publications.Add(result);
                    if (publications.Count == 1)
                    {
                        if (loseLease)
                        {
                            leaseLost.Cancel();
                        }

                        if (cancelExecution)
                        {
                            execution.Cancel();
                            return Task.FromCanceled(execution.Token);
                        }

                        return Task.FromException(new TimeoutException("Transient publication failure."));
                    }

                    token.ThrowIfCancellationRequested();
                    acceptedResult = result;
                    return Task.CompletedTask;
                });
            var store = new InMemoryDistributedArtifactStore();
            var lifecycle = new ArtifactLifecycleManager(store, MsOptions.Create(new DistributedOptions()),
                NullLogger<ArtifactLifecycleManager>.Instance, directory.FullName);
            var executor = new DistributedAssignmentExecutor(
                coordinator.Object, types, serializer,
                pipeline.Services.GetRequiredService<IModuleRunner>(),
                pipeline.Services.GetRequiredService<IModuleResultRegistry>(),
                pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
                pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
                pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
                lifecycle, null, null, NullLogger.Instance);

            await executor.ExecuteAsync(lease, DateTimeOffset.UtcNow,
                DependencyResultApplicator.BuildModuleLookup([module]),
                new DependencyResultCache(coordinator.Object, cancellationToken), execution.Token, leaseLost.Token);

            await Assert.That(publications.Count).IsEqualTo(loseLease ? 1 : 2);
            var first = publications[0];
            await Assert.That(first.Artifacts.Count).IsEqualTo(1);
            await Assert.That(first.ExecutionTelemetry).IsNotNull();
            await Assert.That(serializer.Deserialize(first)!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That((await store.ListArtifactsAsync(assignment.ModuleId, cancellationToken)).Count).IsEqualTo(1);
            if (!loseLease)
            {
                var retry = publications[1];
                await Assert.That(retry.Artifacts.Count).IsEqualTo(1);
                await Assert.That(acceptedResult).IsSameReferenceAs(first);
                var acceptedArtifacts = new AcceptedArtifactRegistry();
                acceptedArtifacts.Record(retry);
                var artifact = await ArtifactLifecycleManager.ResolveArtifactAsync(
                    store, acceptedArtifacts, assignment.ModuleId, "output", cancellationToken);
                await Assert.That(artifact).IsEqualTo(first.Artifacts[0]);
            }
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
