using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests;

public class RejectedAttemptTests
{
    [ProducesArtifact("output", "output.txt")]
    private class Producer : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("local");
    }

    [Test]
    [Timeout(10_000)]
    public async Task Expired_Attempt_Cannot_Override_Accepted_Value_Or_Artifact(CancellationToken cancellationToken)
    {
        var directory = Directory.CreateTempSubdirectory("rejected-attempt-");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "output.txt"), "rejected", cancellationToken);
            var clock = new FakeTimeProvider();
            var coordinator = new InMemoryDistributedCoordinator(
                MsOptions.Create(new DistributedOptions { WorkerTimeout = TimeSpan.FromSeconds(10) }), clock);
            var module = new Producer();
            var types = new ModuleTypeRegistry();
            types.Register(typeof(Producer));
            var serializer = new ModuleResultSerializer(types);
            var assignment = DistributedTestData.Assignment(module, types);
            await coordinator.EnqueueModuleAsync(assignment, cancellationToken);
            var lease = await coordinator.DequeueModuleAsync(DistributedTestData.Worker, new HashSet<Capability>(), cancellationToken);
            var acceptedArtifact = new ArtifactReference
            {
                ArtifactId = "accepted-upload",
                Name = "output",
                ModuleId = assignment.ModuleId,
                SizeBytes = 1,
                UploadedAt = clock.GetUtcNow(),
            };
            var accepted = serializer.Serialize(Result("accepted"), assignment.ModuleId, WorkerId.FromInstanceIndex(2)) with
            {
                Artifacts = [acceptedArtifact],
            };
            var runner = new Mock<IModuleRunner>();
            runner.Setup(value => value.ExecuteWithoutDependencyWaitAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()))
                .Returns(async () =>
                {
                    ModuleCompletionSourceApplicator.TryApply(module, Result("rejected"));
                    clock.Advance(TimeSpan.FromSeconds(11));
                    await coordinator.RequeueExpiredLeasesAsync(cancellationToken);
                    var replacement = await coordinator.DequeueModuleAsync(accepted.WorkerId, new HashSet<Capability>(), cancellationToken);
                    await coordinator.PublishResultAsync(accepted, replacement, cancellationToken);
                });
            var artifacts = new AcceptedArtifactRegistry();
            var results = new ModuleResultRegistry();
            var store = new InMemoryDistributedArtifactStore();
            var lifecycle = new ArtifactLifecycleManager(store, MsOptions.Create(new DistributedOptions()),
                NullLogger<ArtifactLifecycleManager>.Instance, directory.FullName, artifacts);
            using var services = Services();
            var executor = new DistributedAssignmentExecutor(coordinator, types, serializer, runner.Object, results,
                new ModuleDependencyRegistry(), new ModuleMetadataRegistry(new ModuleAttributeEventService()),
                services.GetRequiredService<IServiceScopeFactory>(), lifecycle, artifacts, null, NullLogger.Instance);
            var lookup = DependencyResultApplicator.BuildModuleLookup([module]);
            var cache = new DependencyResultCache(coordinator, cancellationToken);

            await executor.ExecuteAsync(lease!, clock.GetUtcNow(), lookup, cache, cancellationToken);

            await Assert.That((await store.ListArtifactsAsync(assignment.ModuleId, cancellationToken)).Count).IsEqualTo(1);
            await Assert.That(artifacts.TryGet(assignment.ModuleId, out _)).IsFalse();
            await DependencyResultApplicator.FetchAndApplyAsync(
                [DistributedTestData.Dependency(assignment.ModuleId)], cache, lookup, serializer, results,
                NullLogger.Instance, acceptedArtifacts: artifacts);

            await Assert.That((await module).Value).IsEqualTo("accepted");
            await Assert.That(results.GetResult<string>(typeof(Producer))!.Value).IsEqualTo("accepted");
            await Assert.That(artifacts.TryGet(assignment.ModuleId, out var references)).IsTrue();
            await Assert.That(references.Single().ArtifactId).IsEqualTo("accepted-upload");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Timeout(10_000)]
    public async Task Worker_Summary_Reconciles_Rejected_Leaf_Attempt(CancellationToken cancellationToken)
    {
        var module = new Producer();
        var types = new ModuleTypeRegistry();
        types.Register(typeof(Producer));
        var serializer = new ModuleResultSerializer(types);
        var assignment = DistributedTestData.Assignment(module, types);
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        coordinator.SetupSequence(value => value.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedTestData.Lease(assignment))
            .ReturnsAsync((ModuleLease?) null);
        coordinator.Setup(value => value.WaitForCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(async token =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return DistributedCancellationReason.Stopped;
            });
        coordinator.Setup(value => value.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedMasterState.Completed);
        coordinator.Setup(value => value.WaitForResultAsync(assignment.ModuleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serializer.Serialize(Result("accepted"), assignment.ModuleId, WorkerId.FromInstanceIndex(2)));
        var runner = new Mock<IModuleRunner>();
        runner.Setup(value => value.ExecuteWithoutDependencyWaitAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()))
            .Callback(() => ModuleCompletionSourceApplicator.TryApply(module, Result("rejected")))
            .Returns(Task.CompletedTask);
        var registry = new ModuleResultRegistry();
        using var services = Services();
        var executor = new WorkerModuleExecutor(Mock.Of<IHostApplicationLifetime>(), coordinator.Object, [module], types,
            serializer, runner.Object, registry, new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            MsOptions.Create(new DistributedOptions { WorkerHeartbeatInterval = TimeSpan.FromMinutes(1) }),
            Mock.Of<IParallelLimitProvider>(value => value.GetMaxDegreeOfParallelism() == 1),
            services.GetRequiredService<IServiceScopeFactory>(), null, NullLogger<WorkerModuleExecutor>.Instance);

        var summary = await executor.ExecuteAsync(new ExecutionBackendRequest
        {
            Modules = [module],
            EstimatedDurations = new Dictionary<ModuleId, TimeSpan>(),
            Context = new ExecutionBackendContext(registry),
        }, cancellationToken);

        await Assert.That(((ModuleResult<string>) summary.Single()).Value).IsEqualTo("accepted");
        await Assert.That((await module).Value).IsEqualTo("accepted");
    }

    private static ServiceProvider Services()
    {
        var accessor = new Mock<IInternalModuleLoggerAccessor>();
        accessor.Setup(value => value.GetLogger(It.IsAny<Type>())).Returns(Mock.Of<IInternalModuleLogger>());
        return new ServiceCollection().AddSingleton(accessor.Object).BuildServiceProvider();
    }

    private static ModuleResult<string> Result(string value) => new ModuleResult<string>.Success(value)
    {
        Name = nameof(Producer),
        TypeName = typeof(Producer).FullName,
        Status = ModuleStatus.Succeeded,
        Duration = TimeSpan.Zero,
        StartTime = DateTimeOffset.UtcNow,
        EndTime = DateTimeOffset.UtcNow,
    };
}
